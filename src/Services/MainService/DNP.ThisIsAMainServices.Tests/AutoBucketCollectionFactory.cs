using System.Collections.Concurrent;
using System.Reflection;

namespace DNP.ThisIsAMainServices.Tests;

/// <summary>
/// Distributes test classes that have no explicit <c>[Collection]</c> attribute evenly across
/// a fixed set of "bucket" collections. Classes decorated with <c>[Collection(...)]</c> keep
/// their explicit collection (handled by <see cref="TestCollectionFactoryBase"/>).
/// </summary>
public class AutoBucketCollectionFactory : TestCollectionFactoryBase
{
    private readonly ConcurrentDictionary<string, IXunitTestCollection> _collections = new();
    private readonly Dictionary<string, int> _classToCollection;
    private readonly int _bucketCount;

    public override string DisplayName => "Auto Bucket Collection Factory";

    public AutoBucketCollectionFactory(IXunitTestAssembly testAssembly)
        : base(testAssembly)
    {
        // Pre-scan: find all test classes in the assembly
        var testClasses = DiscoverTestClasses(testAssembly);

        // Drive bucket count from env, or default to logical CPU count
        _bucketCount = GetBucketCount(testClasses.Count);

        // Distribute the sorted classes round-robin across the buckets.
        // IMPORTANT: the assignment must be deterministic. The factory is instantiated
        // separately for the discovery and execution phases (and in separate processes by
        // Test Explorer / `dotnet test`), and the collection is part of each test case's
        // unique ID. Random bucketing changes the IDs between phases, so previously
        // discovered tests can no longer be matched and silently don't run.
        _classToCollection = testClasses
            .Select((className, index) => (className, index))
            .ToDictionary(
                x => x.className,
                x => x.index % _bucketCount
            );
    }

    // Called by TestCollectionFactoryBase only for classes WITHOUT an explicit [Collection]
    protected override IXunitTestCollection GetDefaultTestCollection(Type testClass)
    {
        var bucket = _classToCollection.TryGetValue(testClass.FullName ?? testClass.Name, out var b) ? b : 0;
        var collectionName = $"Auto Bucket {bucket}";

        return _collections.GetOrAdd(collectionName, name =>
            new XunitTestCollection(
                TestAssembly,
                collectionDefinition: typeof(DefaultTestCollection),
                disableParallelization: false,
                displayName: name));
    }

    private static List<string> DiscoverTestClasses(IXunitTestAssembly testAssembly)
    {
        return [..
            GetLoadableTypes(testAssembly.Assembly)
                .Where(t =>
                    t.IsClass &&
                    !t.IsAbstract &&
                    !t.GetCustomAttributes().OfType<ICollectionAttribute>().Any() && // skip explicit collections
                    t.GetMethods().Any(m => m.GetCustomAttributes().OfType<IFactAttribute>().Any()))
                .Select(t => t.FullName!)
                .OrderBy(name => name, StringComparer.Ordinal)
        ];
    }

    private static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException ex)
        {
            // Fall back to whatever types could be loaded instead of discovering nothing
            return ex.Types.Where(t => t is not null)!;
        }
    }

    private static int GetBucketCount(int classCount)
    {
        // From env variable
        var env = Environment.GetEnvironmentVariable("TEST_BUCKET_COUNT") ?? "1";
        if (int.TryParse(env, out var n) && n > 0)
            return Math.Min(n, Math.Max(classCount, 3)); // can't have more buckets than classes

        // Otherwise: one bucket per CPU, capped at class count
        return Math.Max(Math.Min(Environment.ProcessorCount, classCount), 1);
    }
}

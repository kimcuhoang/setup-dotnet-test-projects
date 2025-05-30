
namespace DNP.PeopleService.Tests.xUnitV3;

public abstract class ServiceTestBase(ServiceTestAssemblyFixture testCollectionFixture, ITestOutputHelper testOutputHelper) : IAsyncLifetime
{
    protected readonly ServiceApplicationFactory Factory = testCollectionFixture.Factory;

    protected readonly ITestOutputHelper TestOutputHelper = testOutputHelper;

    protected readonly IServiceProvider ServiceProvider = testCollectionFixture.Factory.Services;

    protected readonly CancellationToken CancellationToken = TestContext.Current.CancellationToken;

    protected readonly Faker Faker = new();

    public virtual async ValueTask DisposeAsync()
    {
        await this.ExecuteDbContextAsync(async dbContext =>
        {
            await dbContext.Set<PersonDomain>()
                .Where(_ => _.Id != PersonDomain.Default.Id)
                .ExecuteDeleteAsync(this.CancellationToken);
        });
    }

    public virtual ValueTask InitializeAsync() => ValueTask.CompletedTask;

    protected async Task ExecuteTransactionDbContextAsync(Func<DbContext, Task> func)
    {
        await this.Factory.ExecuteServiceAsync(async serviceProvider =>
        {
            var dbContext = serviceProvider.GetRequiredService<DbContext>();

            var strategy = dbContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                // Achieving atomicity
                await using var transaction = await dbContext.Database.BeginTransactionAsync();
                try
                {
                    await func.Invoke(dbContext);
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        });
    }

    protected async Task ExecuteDbContextAsync(Func<DbContext, Task> func)
    {
        await this.Factory.ExecuteServiceAsync(async serviceProvider =>
        {
            var dbContext = serviceProvider.GetRequiredService<DbContext>();

            await func.Invoke(dbContext);
        });
    }

    protected async Task ExecuteHttpClientAsync(Func<HttpClient, Task> func)
    {
        using var httpClient = this.Factory.CreateClient();
        await func.Invoke(httpClient);
    }

    protected async Task<T?> ParseResponse<T>(HttpResponseMessage response, bool writeConsole = true)
    {
        var content = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(content)) return default(T);

        if (writeConsole)
        {
            this.TestOutputHelper.WriteLine(content);
        }

        var jsonSerializerOptions = this.Factory.JsonSerializerSettings;
        return JsonSerializer.Deserialize<T>(content, jsonSerializerOptions);
    }
}

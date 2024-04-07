using System.Diagnostics;
using Testcontainers.MsSql;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = false)]

namespace DNP.PeopleService.Tests;


[CollectionDefinition(nameof(PersonalServiceTestCollection))]
public class PersonalServiceTestCollection : ICollectionFixture<PersonalServiceTestCollectionFixture>
{

}

public class PersonalServiceTestCollectionFixture : IAsyncLifetime
{
    public MsSqlContainer Container { get; }

    public PeopleServiceWebApplicationFactory Factory { get; private set; } = default!;

    private const string MsSqlPassword = "P@ssw0rd-01";


    public PersonalServiceTestCollectionFixture()
    {
        Debug.WriteLine($"{nameof(PersonalServiceTestCollectionFixture)} constructor");

        this.Container = new MsSqlBuilder()
                .WithAutoRemove(true)
                .WithCleanUp(true)
                .WithHostname("test")
                .WithPortBinding(1433, assignRandomHostPort: true)
                .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                .WithPassword(MsSqlPassword)
                .WithStartupCallback(async (container, cancellationToken) =>
                {
                    Debug.WriteLine($"{nameof(PersonalServiceTestCollectionFixture)} - after container started");

                    this.Factory = new PeopleServiceWebApplicationFactory(this.GetConnectionString(container));
                    
                    await Task.Yield();
                })
                .Build();
    }

    /// <summary>
    /// Override the GetConnectionString from MsSqlBuilder
    /// </summary>
    /// <param name="container"></param>
    /// <returns></returns>
    private string GetConnectionString(MsSqlContainer container)
    {
        var properties = new Dictionary<string, string>
        {
            { "Server", container.Hostname + "," + container.GetMappedPublicPort(MsSqlBuilder.MsSqlPort) },
            { "Database", MsSqlBuilder.DefaultDatabase },
            { "User Id", MsSqlBuilder.DefaultUsername },
            { "Password", MsSqlPassword },
            { "Encrypt", bool.FalseString },
            { "MultipleActiveResultSets", bool.TrueString }
        };
        return string.Join(";", properties.Select(property => string.Join("=", property.Key, property.Value)));
    }

    public async Task DisposeAsync()
    {
        await this.Container.DisposeAsync();
        Debug.WriteLine($"{nameof(PersonalServiceTestCollectionFixture)} {nameof(DisposeAsync)}");
    }

    public async Task InitializeAsync()
    {
        Debug.WriteLine($"{nameof(PersonalServiceTestCollectionFixture)} {nameof(InitializeAsync)}");

        await this.Container.StartAsync();
    }
}

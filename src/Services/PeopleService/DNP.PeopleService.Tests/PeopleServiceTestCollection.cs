using System.Diagnostics;
using Testcontainers.MsSql;

namespace DNP.PeopleService.Tests;


[CollectionDefinition(nameof(PeopleServiceTestCollection))]
public class PeopleServiceTestCollection : ICollectionFixture<PersonalServiceTestCollectionFixture>
{

}

public class PersonalServiceTestCollectionFixture : IAsyncLifetime
{
    public MsSqlContainer Container { get; private set; } = default!;

    public PeopleServiceWebApplicationFactory Factory { get; private set; } = default!;

    private const string MsSqlPassword = "P@ssw0rd-01";


    public PersonalServiceTestCollectionFixture()
    {
        Debug.WriteLine($"{nameof(PersonalServiceTestCollectionFixture)} constructor");
    }

    public async ValueTask DisposeAsync()
    {
        await this.Container.DisposeAsync();
        Debug.WriteLine($"{nameof(PersonalServiceTestCollectionFixture)} {nameof(DisposeAsync)}");
    }

    public async ValueTask InitializeAsync()
    {
        Debug.WriteLine($"{nameof(PersonalServiceTestCollectionFixture)} {nameof(InitializeAsync)}");

        await new MsSqlBuilder()
                .WithAutoRemove(true)
                .WithCleanUp(true)
                .WithHostname("test")
                .WithPortBinding(1433, assignRandomHostPort: true)
                // Temporarily use the default image which is: mcr.microsoft.com/mssql/server:2019-CU18-ubuntu-20.04
                // Due to the issues
                // https://github.com/testcontainers/testcontainers-dotnet/issues/1220
                // 
                //.WithImage("mcr.microsoft.com/mssql/server:2022-latest")
                .WithStartupCallback(async (c, cancellationToken) =>
                {
                    Debug.WriteLine($"{nameof(PersonalServiceTestCollectionFixture)} - after container started");

                    this.Container = c;
                    this.Factory = new PeopleServiceWebApplicationFactory(c.GetConnectionString());
                    await Task.Yield();
                })
                .Build()
                .StartAsync();
    }
}

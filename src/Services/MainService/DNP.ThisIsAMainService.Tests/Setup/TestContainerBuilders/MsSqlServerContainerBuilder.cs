using Microsoft.Extensions.Configuration;
using Testcontainers.MsSql;

namespace DNP.ThisIsAMainService.Tests.Setup.TestContainerBuilders;

public class MsSqlServerContainerBuilder(string uniqueName, string configurationKey, string? databaseName = null) : ITestContainerBuilder, IHasConnectionString
{
    private const string MsSqlServerDockerImage = "mcr.microsoft.com/mssql/server:2022-latest";
    private MsSqlContainer? _msSqlContainer = default;

    public string ConnectionString => this._msSqlContainer?.GetConnectionString()
        ?? throw new InvalidOperationException("MsSqlContainer is not initialized.");

    public void ConfigureConnectionString(IConfigurationBuilder configuration)
    {
        configuration.AddInMemoryCollection(new Dictionary<string, string?>
        {
            { configurationKey, this.ConnectionString }
        });
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        this._msSqlContainer = new MsSqlBuilder(MsSqlServerDockerImage)
            .WithName($"BTL-MsSqlServer-{uniqueName}")
            .WithPortBinding(1433, assignRandomHostPort: true)
            .WithDatabase(databaseName ?? "test")
            .WithCleanUp(true)
            .WithAutoRemove(true)
            .Build();

        await this._msSqlContainer.StartAsync(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken = default)
    {
        if (this._msSqlContainer is not null)
        {
            await this._msSqlContainer.StopAsync(cancellationToken);
            await this._msSqlContainer.DisposeAsync();
        }
    }
}


using Azure.Storage.Blobs;
using Testcontainers.Azurite;
using Testcontainers.MsSql;

namespace DNP.PeopleService.Tests.xUnitV3.BeforeTests;

public class TestCollectionFixture : IAsyncLifetime
{
    public TestApplicationFactory Factory { get; private set; }
    private readonly MsSqlContainer _sqlContainer;
    private readonly AzuriteContainer _azuriteContainer;

    public TestCollectionFixture()
    {
        this._sqlContainer = new MsSqlBuilder()
            .WithAutoRemove(true)
            .WithCleanUp(true)
            .WithPortBinding(1433, assignRandomHostPort: true)
            .Build();

        this._azuriteContainer = new AzuriteBuilder()
            .WithImage("mcr.microsoft.com/azure-storage/azurite:3.34.0")
            .WithAutoRemove(true)
            .WithCleanUp(true)
            .DependsOn(this._sqlContainer)
            .WithStartupCallback(async (container, cancellationToken) =>
            {
                this.Factory = new TestApplicationFactory
                {
                    ConnectionString = this._sqlContainer.GetConnectionString(),
                    AzureStorageConnectionString = container.GetConnectionString()
                };

                await this.Factory.ExecuteServiceAsync(async services =>
                {
                    var blobContainerClient = services.GetRequiredService<BlobContainerClient>();

                    await blobContainerClient.CreateIfNotExistsAsync(
                        publicAccessType: Azure.Storage.Blobs.Models.PublicAccessType.Blob,
                        cancellationToken: TestContext.Current.CancellationToken);
                });
                await Task.Yield();
            })
            .Build();
    }


    public async ValueTask InitializeAsync()
    {
        await this._sqlContainer.StartAsync();
        await this._azuriteContainer.StartAsync();

    }

    public async ValueTask DisposeAsync()
    {
        this.Factory?.Dispose();
        await this._azuriteContainer.DisposeAsync();
        await this._sqlContainer.DisposeAsync();
    }
}

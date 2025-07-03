using DNP.PeopleService.Infrastructures.FileStorages;
using Microsoft.Extensions.Options;

namespace DNP.PeopleService.Tests.xUnitV3.TestFileStorages;
public class TestFileStoragesConfiguration(TestCollectionFixture testCollectionFixture, ITestOutputHelper testOutputHelper, ITestContextAccessor testContextAccessor) : IntegrationTestBase(testCollectionFixture, testOutputHelper, testContextAccessor)
{
    [Fact]
    public async Task ShouldUseAzureBlobStorage()
    {
        await this.ExecuteServiceAsync(async serviceProvider =>
        {
            await Task.Yield();

            var fileStorageOptions = serviceProvider.GetRequiredService<IOptions<FileStorageOptions>>().Value;
            fileStorageOptions.ShouldSatisfyAllConditions(_ =>
            {
                _.ShouldNotBeNull();
                _.FileStorageType.ShouldBe(FileStorageType.AzureBlobStorage);
                _.AzureBlobOptions.ShouldSatisfyAllConditions(o =>
                {
                    o.ConnectionString.ShouldNotBeNullOrWhiteSpace();
                    o.ContainerName.ShouldNotBeNullOrWhiteSpace();
                });
                _.FileSystemOptions.ShouldSatisfyAllConditions(o =>
                {
                    o.BasePath.ShouldNotBeNullOrWhiteSpace();
                });
            });

            this.TestOutputHelper.WriteLine($"Type: {fileStorageOptions.FileStorageType.ToString()}");
            this.TestOutputHelper.WriteLine(JsonSerializer.Serialize(fileStorageOptions, this.Factory.JsonSerializerSettings));

            var fileStorage = serviceProvider.GetRequiredKeyedService<IFileStorage>(fileStorageOptions.FileStorageType);
            fileStorage.ShouldBeOfType<AzureBlobStorage>();
        });
    }
}

using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;

namespace DNP.PeopleService.Infrastructures.FileStorages;

public static class FileStoragesRegistration
{
    public static WebApplicationBuilder AddFileStorages(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        builder.Services
            .AddOptions<FileStorageOptions>()
            .Bind(configuration.GetSection(nameof(FileStorageOptions)))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        builder.Services
            .AddKeyedScoped<IFileStorage, AzureBlobStorage>(FileStorageType.AzureBlobStorage)
            .AddKeyedScoped<IFileStorage, FileSystemStorage>(FileStorageType.FileSystem);

        builder.Services
            .AddSingleton(services =>
            {
                var azureBlobOptions = services
                    .GetRequiredService<IOptions<FileStorageOptions>>()
                    .Value.AzureBlobOptions;

                return new BlobContainerClient(
                    azureBlobOptions.ConnectionString,
                    azureBlobOptions.ContainerName,
                    new BlobClientOptions
                    {
                        Retry =
                        {
                            MaxRetries = 5,
                            Delay = TimeSpan.FromSeconds(2)
                        }
                    });
            });
        
        return builder;
    }
}

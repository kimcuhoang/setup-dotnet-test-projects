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
            //.Validate(_ => _.FileStorageType is FileStorageType.AzureBlobStorage or FileStorageType.FileSystem)
            //.Validate(_ => _.AzureBlobOptions is not null)
            //.Validate(_ => _.FileSystemOptions is not null);
            .ValidateDataAnnotations()
            .ValidateOnStart();

        builder.Services
            .AddKeyedScoped<IFileStorage, AzureBlobStorage>(FileStorageType.AzureBlobStorage)
            .AddKeyedScoped<IFileStorage, FileSystemStorage>(FileStorageType.FileSystem);

        builder.Services
            .AddSingleton(services =>
            {
                var connectionString = services
                    .GetRequiredService<IOptions<FileStorageOptions>>()
                    .Value
                    .AzureBlobOptions
                    .ConnectionString;

                return new BlobServiceClient(connectionString, new BlobClientOptions
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


using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;

namespace DNP.PeopleService.Infrastructures.FileStorages;

public class AzureBlobStorage(BlobContainerClient blobContainerClient,
                              IOptions<FileStorageOptions> fileStorageOptions) : IFileStorage
{
    private readonly AzureBlobOptions _azureBlobOptions = fileStorageOptions.Value.AzureBlobOptions;

    public async Task DeleteAsync(string fileName, CancellationToken cancellationToken = default)
    {
        var blobClient = blobContainerClient.GetBlobClient(fileName);
        var deleteResponse = await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
        
        var deleteResult = deleteResponse.GetRawResponse();
        if (deleteResult.IsError)
        {
            throw new InvalidOperationException($"Failed to delete file '{fileName}' from Azure Blob Storage. Status: {deleteResult.Status}, Error: {deleteResult.ReasonPhrase}");
        }
    }

    public async Task<byte[]> ReadAsync(string fileName, CancellationToken cancellationToken = default)
    {
        var blobClient = blobContainerClient.GetBlobClient(fileName);

        using var memoryStream = new MemoryStream();
        var downloadResult = await blobClient.DownloadToAsync(memoryStream, cancellationToken: cancellationToken);
        
        return downloadResult.IsError
            ? throw new InvalidOperationException($"Failed to download file '{fileName}' from Azure Blob Storage. Status: {downloadResult.Status}, Error: {downloadResult.ReasonPhrase}")
            : memoryStream.ToArray();
    }

    public async Task<string> SaveAsync(IFormFile formFile, CancellationToken cancellationToken = default)
    {
        var blobClient = blobContainerClient.GetBlobClient(formFile.FileName);
        var uploadResponse = await blobClient.UploadAsync(formFile.OpenReadStream(), overwrite: true, cancellationToken: cancellationToken);
        var uploadResult = uploadResponse.GetRawResponse();
        return uploadResult.IsError
            ? throw new InvalidOperationException($"Failed to upload file '{formFile.FileName}' to Azure Blob Storage. Status: {uploadResult.Status}, Error: {uploadResult.ReasonPhrase}")
            : formFile.FileName;
    }
}

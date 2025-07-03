using System.ComponentModel.DataAnnotations;

namespace DNP.PeopleService.Infrastructures.FileStorages;

public enum FileStorageType
{
    FileSystem,
    AzureBlobStorage
}

public class FileStorageOptions
{
    [Required]
    public FileStorageType FileStorageType { get; init; }
    [Required]
    public AzureBlobOptions AzureBlobOptions { get; init; }
    [Required]
    public FileSystemOptions FileSystemOptions { get; init; }
}


public class AzureBlobOptions
{
    [Required]
    public string ConnectionString { get; init; }
    [Required]
    public string ContainerName { get; init; }
}

public class FileSystemOptions
{
    [Required]
    public string BasePath { get; init; }
}
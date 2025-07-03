

namespace DNP.PeopleService.Infrastructures.FileStorages;

public class FileSystemStorage : IFileStorage
{
    public Task DeleteAsync(string fileName, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<byte[]> ReadAsync(string fileName, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<string> SaveAsync(IFormFile formFile, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

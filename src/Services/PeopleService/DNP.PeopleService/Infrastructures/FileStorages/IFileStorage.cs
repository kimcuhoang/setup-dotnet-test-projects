namespace DNP.PeopleService.Infrastructures.FileStorages;

public interface IFileStorage
{
    public Task<string> SaveAsync(IFormFile formFile, CancellationToken cancellationToken = default);
    public Task<byte[]> ReadAsync(string fileName, CancellationToken cancellationToken = default);
    public Task DeleteAsync(string fileName, CancellationToken cancellationToken = default);
}

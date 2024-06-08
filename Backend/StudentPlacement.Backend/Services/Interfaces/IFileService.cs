namespace StudentPlacement.Backend.Services.Interfaces
{
    public interface IFileService
    {
        bool DeleteFile(string path);
        
        Task AddFile(string path, IFormFile file);

        Task<byte[]> GetByteFile(string path);
    }
}

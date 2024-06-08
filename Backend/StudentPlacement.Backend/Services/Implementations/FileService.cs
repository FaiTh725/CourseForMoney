using StudentPlacement.Backend.Services.Interfaces;

namespace StudentPlacement.Backend.Services.Implementations
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment environment;

        public FileService(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        public async Task AddFile(string path, IFormFile file)
        {
            var rootPath = Path.Combine(environment.WebRootPath, path);

            using (var fileStream = new FileStream(rootPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
        }

        public bool DeleteFile(string path)
        {
            var rootPath = Path.Combine(environment.WebRootPath, path);

            if (File.Exists(rootPath))
            {
                File.Delete(rootPath);
                return true;
            }

            return false;
        }

        public async Task<byte[]> GetByteFile(string path)
        {
            var rootPath = Path.Combine(environment.WebRootPath, path);

            if(File.Exists(rootPath))
            {
                return await File.ReadAllBytesAsync(rootPath);
            }

            return new byte[0];
        }
    }
}

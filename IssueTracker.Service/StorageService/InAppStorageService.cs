using IssueTracker.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace IssueTracker.Service.StorageService
{
    public class InAppStorageService : IInAppStorageService
    {
        private readonly IHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public InAppStorageService(IHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<(string imagePath, Guid imageGuid)> SaveFile(byte[] content, string extension, string containerName)
        {
            var imageGuid = Guid.NewGuid();
            var fileName = $"{imageGuid}{extension}";
            string folder = Path.Combine(_env.ContentRootPath, containerName);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            string savingPath = Path.Combine(folder, fileName);
            await File.WriteAllBytesAsync(savingPath, content);

            var currentUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            var pathForDb = Path.Combine(currentUrl, containerName, fileName);
            return (pathForDb, imageGuid);
        }

        public Task DeleteFile(string fileRoute, string containerName)
        {
            var fileName = Path.GetFileName(fileRoute);
            fileName = fileName + ".jpg";
            var fileDirectory = Path.Combine(_env.ContentRootPath, containerName, fileName);
            //File.Delete(fileDirectory);
            if (File.Exists(fileDirectory))
            {
                File.Delete(fileDirectory);
            }
            return Task.FromResult(0);
        }

        public async Task<(string imagePath, string fileName)> Upload(IFormFile formFile)
        {
            var extension = Path.GetExtension(formFile.FileName);
            var fileName = $"{Guid.NewGuid().ToString()}{extension}";

            string folder = Path.Combine(_env.ContentRootPath, "IssueFiles");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var pathForDb = Path.Combine(_env.ContentRootPath, "IssueFiles/", fileName);

            using FileStream stream = new FileStream(pathForDb, FileMode.Create);
            await formFile.CopyToAsync(stream);
            stream.Close();

            return (pathForDb, fileName);
        }
    }
}

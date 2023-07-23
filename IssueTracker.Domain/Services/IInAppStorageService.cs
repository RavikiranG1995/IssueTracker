using Microsoft.AspNetCore.Http;

namespace IssueTracker.Domain.Services
{
    public interface IInAppStorageService
    {
        Task<(string imagePath, Guid imageGuid)> SaveFile(byte[] content, string extension, string containerName);
        Task DeleteFile(string fileRoute, string containerName);
        Task<(string filePath, Guid fileGuid)> Upload(IFormFile formFile);
    }
}

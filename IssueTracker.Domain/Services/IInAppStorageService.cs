using Microsoft.AspNetCore.Http;

namespace IssueTracker.Domain.Services
{
    public interface IInAppStorageService
    {
        Task DeleteFile(string fileRoute, string containerName);
        Task<(string filePath, Guid fileGuid)> SaveFile(IFormFile formFile);
    }
}

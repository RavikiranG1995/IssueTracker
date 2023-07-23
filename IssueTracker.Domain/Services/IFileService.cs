using IssueTracker.Domain.Entities.IssueFiles;
using Microsoft.AspNetCore.Http;

namespace IssueTracker.Domain.Services
{
    public interface IFileService
    {
        Task Delete(Guid fileGuid);
        Task<List<IFile>> Upload(List<IFormFile> formFiles);
        Task Upload(int issueId, List<IFormFile> formFiles);
    }
}

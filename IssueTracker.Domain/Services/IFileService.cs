using IssueTracker.Domain.Entities.IssueImage;
using Microsoft.AspNetCore.Http;

namespace IssueTracker.Domain.Services
{
    public interface IFileService
    {
        Task Delete(Guid imageGuid);
        Task<List<IFile>> Upload(List<IFormFile> formFiles);
        Task Upload(int issueId, List<IFormFile> formFiles);
    }
}

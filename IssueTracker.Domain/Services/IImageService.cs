using IssueTracker.Domain.Entities.IssueImage;
using Microsoft.AspNetCore.Http;

namespace IssueTracker.Domain.Services
{
    public interface IImageService
    {
        Task Delete(Guid imageGuid);
        Task Save(int issueId, IImage image);
        Task<List<IImage>> Upload(List<IFormFile> formFiles);
        Task Upload(int issueId, List<IFormFile> formFiles);
    }
}

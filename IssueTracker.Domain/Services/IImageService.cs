using IssueTracker.Domain.Entities.IssueImage;

namespace IssueTracker.Domain.Services
{
    public interface IImageService
    {
        Task Delete(Guid imageGuid);
        Task Save(int issueId, IImage image);
    }
}

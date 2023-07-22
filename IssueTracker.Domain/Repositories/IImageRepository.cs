using IssueTracker.Domain.Entities.IssueImage;

namespace IssueTracker.Domain.Repositories
{
    public interface IImageRepository
    {
        Task Delete(Guid imageGuid);
        Task SaveImage(int issueId, IImage image);
        Task<IList<IImage>> GetAllIssueImages(int issueId);
    }
}

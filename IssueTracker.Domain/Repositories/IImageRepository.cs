using IssueTracker.Domain.Entities.IssueImage;

namespace IssueTracker.Domain.Repositories
{
    public interface IImageRepository
    {
        Task Delete(Guid imageGuid);
        Task SaveImage(int issueId, IFile file);
        Task<IList<IFile>> GetAllIssueFiles(int issueId);
    }
}

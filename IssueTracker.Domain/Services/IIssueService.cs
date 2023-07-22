using IssueTracker.Domain.Entities.IssueImage;
using IssueTracker.Domain.Entities.Issues;

namespace IssueTracker.Domain.Services
{
    public interface IIssueService
    {
        Task<int> Upsert(IIssue issue, List<IImage> files);
        Task<IIssue> GetIssueById(int id);
        Task<IEnumerable<IIssue>> GetAllIssues();
        Task DeleteIssue(int id);
    }
}

using IssueTracker.Domain.Entities.IssueFiles;

namespace IssueTracker.Domain.Repositories
{
    public interface IFileRepository
    {
        Task Delete(Guid fileGuid);
        Task SaveFile(int issueId, IFile file);
        Task<IList<IFile>> GetAllIssueFiles(int issueId);
    }
}

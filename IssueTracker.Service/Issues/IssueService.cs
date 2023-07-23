using IssueTracker.Domain.Entities.Issues;
using IssueTracker.Domain.Repositories;
using IssueTracker.Domain.Services;

namespace IssueTracker.Service.Issues
{
    public class IssueService : IIssueService
    {
        private readonly IInAppStorageService _inAppStorageService;
        private readonly IIssueRepository _issueRepository;
        private readonly IFileRepository _fileRepository;
        private string containerName = "IssueFiles";
        public IssueService(IInAppStorageService inAppStorageService, IIssueRepository issueRepository, IFileRepository fileRepository)
        {
            _inAppStorageService = inAppStorageService;
            _issueRepository = issueRepository;
            _fileRepository = fileRepository;
        }
        public async Task<int> Upsert(IIssue issue)
        {
            return await _issueRepository.Upsert(issue);
        }

        public async Task<IIssue> GetIssueById(int id)
        {
            return await _issueRepository.GetIssueById(id);
        }

        public async Task<IEnumerable<IIssue>> GetAllIssues()
        {
            return await _issueRepository.GetAllIssues();
        }

        public async Task DeleteIssue(int id)
        {
            var filesToRemove = await _fileRepository.GetAllIssueFiles(id);
            foreach (var file in filesToRemove)
            {
                await _inAppStorageService.DeleteFile(file.FileGuid.ToString(), containerName);
            }
            await _issueRepository.DeleteIssue(id);
        }
    }
}

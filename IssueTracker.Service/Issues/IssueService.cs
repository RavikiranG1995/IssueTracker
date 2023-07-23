using IssueTracker.Domain.Entities.IssueImage;
using IssueTracker.Domain.Entities.Issues;
using IssueTracker.Domain.Repositories;
using IssueTracker.Domain.Services;

namespace IssueTracker.Service.Issues
{
    public class IssueService : IIssueService
    {
        private readonly IInAppStorageService _inAppStorageService;
        private readonly IIssueRepository _issueRepository;
        private readonly IImageRepository _imageRepository;
        private string containerName = "IssueImages";
        public IssueService(IInAppStorageService inAppStorageService, IIssueRepository issueRepository, IImageRepository imageRepository)
        {
            _inAppStorageService = inAppStorageService;
            _issueRepository = issueRepository;
            _imageRepository = imageRepository;
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
            var imaggesToRemove = await _imageRepository.GetAllIssueFiles(id);
            foreach (var image in imaggesToRemove)
            {
                await _inAppStorageService.DeleteFile(image.ImageGuid.ToString(), containerName);
            }
            await _issueRepository.DeleteIssue(id);
        }
    }
}

using IssueTracker.Domain.Entities.IssueImage;
using IssueTracker.Domain.Repositories;
using IssueTracker.Domain.Services;

namespace IssueTracker.Service.Image
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;
        private readonly IIssueRepository _issueRepository;
        private readonly IInAppStorageService _inAppStorageService;
        private string containerName = "IssueImages";
        public ImageService(IImageRepository imageRepository, IInAppStorageService inAppStorageService, IIssueRepository issueRepository)
        {
            _imageRepository = imageRepository;
            _inAppStorageService = inAppStorageService;
            _issueRepository = issueRepository;
        }

        public async Task Delete(Guid imageGuid)
        {
            await _inAppStorageService.DeleteFile(imageGuid.ToString(), containerName);
            await _imageRepository.Delete(imageGuid);
        }
        public async Task Save(int issueId, IImage image)
        {
            var issue = await _issueRepository.GetIssueById(issueId);
            if (issue is null)
            {
                throw new Exception("Issue not found for the passed in IssueId");
            }

            var imageBytes = Convert.FromBase64String(image.Bas64Image);
            var imageData = await _inAppStorageService.SaveFile(imageBytes, ".jpg", containerName);
            image.ImagePath = imageData.imagePath;
            image.ImageGuid = imageData.imageGuid;
            await _imageRepository.SaveImage(issueId, image);
        }
    }
}

using IssueTracker.Domain.Entities.IssueImage;
using IssueTracker.Domain.Repositories;
using IssueTracker.Domain.Services;
using Microsoft.AspNetCore.Http;

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
                throw new NullReferenceException($"Issue was not found for the passed in IssueId {issueId}");
            }

            var imageBytes = Convert.FromBase64String(image.Bas64Image);
            var imageData = await _inAppStorageService.SaveFile(imageBytes, ".jpg", containerName);
            image.ImagePath = imageData.imagePath;
            image.ImageGuid = imageData.imageGuid;
            await _imageRepository.SaveImage(issueId, image);
        }

        public async Task<List<IImage>> Upload(List<IFormFile> formFiles)
        {
            var issueFiles = new List<IImage>();
            foreach (var formFile in formFiles)
            {
                var file = new Domain.Entities.Issues.Image();
                var fileData = await _inAppStorageService.Upload(formFile);
                file.ImagePath = fileData.imagePath;
                //add proper path
                file.ImageGuid = Guid.NewGuid();
                //await _imageRepository.SaveImage(issueId,file);
            }
            return issueFiles;
        }
        public async Task Upload(int issueId, List<IFormFile> formFiles)
        {
            var files = await Upload(formFiles);
            foreach (var file in files)
            {
                await _imageRepository.SaveImage(issueId, file);
            }
        }
    }
}

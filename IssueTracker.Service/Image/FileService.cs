﻿using IssueTracker.Domain.Entities.IssueImage;
using IssueTracker.Domain.Repositories;
using IssueTracker.Domain.Services;
using Microsoft.AspNetCore.Http;

namespace IssueTracker.Service.Image
{
    public class FileService : IFileService
    {
        private readonly IImageRepository _imageRepository;
        private readonly IIssueRepository _issueRepository;
        private readonly IInAppStorageService _inAppStorageService;
        private string containerName = "IssueFiles";
        public FileService(IImageRepository imageRepository, IInAppStorageService inAppStorageService, IIssueRepository issueRepository)
        {
            _imageRepository = imageRepository;
            _inAppStorageService = inAppStorageService;
            _issueRepository = issueRepository;
        }

        public async Task Delete(Guid fileGuid)
        {
            await _inAppStorageService.DeleteFile(fileGuid.ToString(), containerName);
            await _imageRepository.Delete(fileGuid);
        }

        public async Task<List<IFile>> Upload(List<IFormFile> formFiles)
        {
            var issueFiles = new List<IFile>();
            foreach (var formFile in formFiles)
            {
                var file = new Domain.Entities.Issues.File();
                var fileData = await _inAppStorageService.Upload(formFile);
                file.ImagePath = fileData.imagePath;
                file.ImageGuid = fileData.fileGuid;
                issueFiles.Add(file);
            }
            return issueFiles;
        }

        public async Task Upload(int issueId, List<IFormFile> formFiles)
        {
            var issue = await _issueRepository.GetIssueById(issueId);
            if (issue is null)
            {
                throw new NullReferenceException($"Issue was not found for the passed in IssueId {issueId}");
            }
            var files = await Upload(formFiles);
            foreach (var file in files)
            {
                await _imageRepository.SaveImage(issueId, file);
            }
        }
    }
}
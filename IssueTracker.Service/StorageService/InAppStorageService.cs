﻿using IssueTracker.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace IssueTracker.Service.StorageService
{
    public class InAppStorageService : IInAppStorageService
    {
        private readonly IHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public InAppStorageService(IHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task DeleteFile(string fileRoute, string containerName)
        {
            var fileName = Path.GetFileName(fileRoute);
            fileName = fileName + ".*";
            var fileDirectory = Path.Combine(_env.ContentRootPath, containerName);
            var file = Directory.GetFiles(fileDirectory, fileName).FirstOrDefault();
            if (!string.IsNullOrEmpty(file))
            {
                System.IO.File.Delete(file);
            }
            return Task.FromResult(0);
        }

        public async Task<(string filePath, Guid fileGuid)> SaveFile(IFormFile formFile)
        {
            var extension = Path.GetExtension(formFile.FileName);
            //var fileName = $"{Guid.NewGuid().ToString()}{extension}";
            var fileGuid = Guid.NewGuid();
            var fileName = $"{fileGuid}{extension}";

            string folder = Path.Combine(_env.ContentRootPath, "IssueFiles");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            var currentUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            var pathForDb = Path.Combine(currentUrl, "IssueFiles", fileName);
            //var pathForDb = Path.Combine(_env.ContentRootPath, "IssueFiles/", fileName);
            string savingPath = Path.Combine(folder, fileName);
            using FileStream stream = new FileStream(savingPath, FileMode.Create);
            await formFile.CopyToAsync(stream);
            stream.Close();

            return (pathForDb, fileGuid);
        }
    }
}

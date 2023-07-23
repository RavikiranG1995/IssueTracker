using IssueTracker.Service.File;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using File = IssueTracker.Domain.Entities.Issues.File;

namespace UnitTesting.Services
{
    public class FileServiceTests
    {
        private IFileService _service;
        private Mock<IIssueRepository> _mockIssueRepository;
        private Mock<IInAppStorageService> _mockInAppStorageService;
        private Mock<IFileRepository> _mockfileRepository;

        [Test]
        public async Task UploadFile_Throws_NullReferenceException_If_IssueIdIs_Invalid()
        {
            _mockInAppStorageService = new Mock<IInAppStorageService>();
            _mockfileRepository = new Mock<IFileRepository>();
            _mockIssueRepository = new Mock<IIssueRepository>();

            IIssue issue = null;
            IFile file = new File();

            _mockIssueRepository.Setup(x => x.GetIssueById(It.IsAny<int>())).ReturnsAsync(issue);

            int issueId = 500;
            _service = new FileService(_mockfileRepository.Object,_mockInAppStorageService.Object, _mockIssueRepository.Object);
            Assert.ThrowsAsync<NullReferenceException>(async () =>await _service.Upload(issueId, null));
        }

        [Test]
        public async Task UploadFile_Should_Save_AllFilesToDirectory()
        {
            _mockInAppStorageService = new Mock<IInAppStorageService>();
            _mockfileRepository = new Mock<IFileRepository>();
            _mockIssueRepository = new Mock<IIssueRepository>();

            var files = new List<IFormFile>();

            IFormFile formFile = new FormFile(null, 123, 123, "", "");
            files.Add(new FormFile(null, 123, 123, "", ""));
            files.Add(new FormFile(null, 123, 123, "", ""));
            var guid = Guid.NewGuid();
            var filePath = "testpath";
            _mockInAppStorageService.Setup(x => x.SaveFile(It.IsAny<IFormFile>())).ReturnsAsync((filePath, guid));

            _service = new FileService(_mockfileRepository.Object, _mockInAppStorageService.Object, _mockIssueRepository.Object);
            var actual = await _service.Upload(files);

            _mockInAppStorageService.Verify(x => x.SaveFile(It.IsAny<IFormFile>()), Times.Exactly(files.Count()));
            Assert.IsNotNull(actual);
            foreach (var item in actual)
            {
                Assert.AreEqual(item.FilePath, filePath);
                Assert.AreEqual(item.FileGuid, guid);
            }
        }
    }
}

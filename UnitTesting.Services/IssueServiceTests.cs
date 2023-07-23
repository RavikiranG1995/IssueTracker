using IssueTracker.Service.Issues;
using File = IssueTracker.Domain.Entities.Issues.File;

namespace UnitTesting.Services
{
    public class IssueServiceTests
    {
        private IIssueService _service;
        private Mock<IIssueRepository> _mockIssueRepository;
        private Mock<IInAppStorageService> _mockInAppStorageService;
        private Mock<IFileRepository> _mockIImageRepository;

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task DeleteIssue_Should_Delete_AllTheImages()
        {
            _mockInAppStorageService = new Mock<IInAppStorageService>();
            _mockIImageRepository = new Mock<IFileRepository>();
            _mockIssueRepository = new Mock<IIssueRepository>();

            var images = new List<IFile>();
            images.Add(new File { FileGuid = Guid.NewGuid(), FilePath = "" });
            images.Add(new File { FileGuid = Guid.NewGuid(), FilePath = "" });

            _mockIImageRepository.Setup(x => x.GetAllIssueFiles(It.IsAny<int>())).ReturnsAsync(images);
            _mockInAppStorageService.Setup(x => x.DeleteFile(It.IsAny<string>(), It.IsAny<string>()));

            _service = new IssueService(_mockInAppStorageService.Object, _mockIssueRepository.Object, _mockIImageRepository.Object);
            int issueId = 123;
            await _service.DeleteIssue(issueId);
            _mockInAppStorageService.Verify(x => x.DeleteFile(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(images.Count()));
            _mockIssueRepository.Verify(x => x.DeleteIssue(issueId), Times.Once());
        }
    }
}

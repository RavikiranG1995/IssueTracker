namespace UnitTesting.Services
{
    public class IssueServiceTests
    {
        private IIssueService _service;
        private Mock<IIssueRepository> _mockIssueRepository;
        private Mock<IInAppStorageService> _mockInAppStorageService;
        private Mock<IImageRepository> _mockIImageRepository;
        private const string SampleBytes = "R0lGODlhAQABAIAAAAAAAAAAACH5BAAAAAAALAAAAAABAAEAAAICTAEAOw==";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task CreateNewIssue_Should_Save_AllTheImages()
        {
            _mockInAppStorageService = new Mock<IInAppStorageService>();
            _mockIImageRepository = new Mock<IImageRepository>();
            _mockIssueRepository = new Mock<IIssueRepository>();

            _mockIssueRepository.Setup(x => x.Upsert(It.IsAny<IIssue>())).ReturnsAsync(It.IsAny<int>());
            _mockInAppStorageService.Setup(x => x.SaveFile(It.IsAny<byte[]>(), It.IsAny<string>(), It.IsAny<string>()));

            _service = new IssueService(_mockInAppStorageService.Object, _mockIssueRepository.Object, _mockIImageRepository.Object);

            Issue issue = new Issue();
            issue.Images.Add(new Image { Bas64Image = SampleBytes });
            issue.Images.Add(new Image { Bas64Image = SampleBytes });

            await _service.Upsert(issue);
            _mockInAppStorageService.Verify(x => x.SaveFile(It.IsAny<byte[]>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(issue.Images.Count()));
        }

        [Test]
        public async Task CreateNewIssue_Should_Return_IssueId()
        {
            _mockInAppStorageService = new Mock<IInAppStorageService>();
            _mockIImageRepository = new Mock<IImageRepository>();
            _mockIssueRepository = new Mock<IIssueRepository>();
            int expectedId = 500;
            _mockIssueRepository.Setup(x => x.Upsert(It.IsAny<IIssue>())).ReturnsAsync(expectedId);
            _mockInAppStorageService.Setup(x => x.SaveFile(It.IsAny<byte[]>(), It.IsAny<string>(), It.IsAny<string>()));

            _service = new IssueService(_mockInAppStorageService.Object, _mockIssueRepository.Object, _mockIImageRepository.Object);

            Issue issue = new Issue();
            issue.Images.Add(new Image { Bas64Image = SampleBytes });
            issue.Images.Add(new Image { Bas64Image = SampleBytes });

            var actualId = await _service.Upsert(issue);
            Assert.AreEqual(expectedId, actualId);
        }

        [Test]
        public async Task DeleteIssue_Should_Delete_AllTheImages()
        {
            _mockInAppStorageService = new Mock<IInAppStorageService>();
            _mockIImageRepository = new Mock<IImageRepository>();
            _mockIssueRepository = new Mock<IIssueRepository>();

            var images = new List<IImage>();
            images.Add(new Image { Bas64Image = SampleBytes });
            images.Add(new Image { Bas64Image = SampleBytes });

            _mockIImageRepository.Setup(x => x.GetAllIssueImages(It.IsAny<int>())).ReturnsAsync(images);
            _mockInAppStorageService.Setup(x => x.DeleteFile(It.IsAny<string>(), It.IsAny<string>()));

            _service = new IssueService(_mockInAppStorageService.Object, _mockIssueRepository.Object, _mockIImageRepository.Object);
            int issueId = 123;
            await _service.DeleteIssue(issueId);
            _mockInAppStorageService.Verify(x => x.DeleteFile(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(images.Count()));
            _mockIssueRepository.Verify(x=>x.DeleteIssue(issueId), Times.Once());
        }
    }
}

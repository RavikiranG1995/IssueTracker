namespace UnitTesting.Services
{
    public class ImageServiceTests
    {
        private IImageService _service;
        private Mock<IIssueRepository> _mockIssueRepository;
        private Mock<IInAppStorageService> _mockInAppStorageService;
        private Mock<IImageRepository> _mockIImageRepository;
        private const string SampleBytes = "R0lGODlhAQABAIAAAAAAAAAAACH5BAAAAAAALAAAAAABAAEAAAICTAEAOw==";

        [Test]
        public async Task SaveImage_Throws_NullReferenceException_If_IssueIdIs_Invalid()
        {
            _mockInAppStorageService = new Mock<IInAppStorageService>();
            _mockIImageRepository = new Mock<IImageRepository>();
            _mockIssueRepository = new Mock<IIssueRepository>();

            IIssue issue = null;
            IImage image = new Image();

            _mockIssueRepository.Setup(x => x.GetIssueById(It.IsAny<int>())).ReturnsAsync(issue);

            int issueId = 500;
            Assert.Throws<NullReferenceException>(() => _service.Save(issueId, image));
        }
    }
}

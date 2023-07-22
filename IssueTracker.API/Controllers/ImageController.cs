using IssueTracker.Domain.Models.Issue;
using IssueTracker.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace IssueTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;
        private readonly IInAppStorageService _inAppStorageService;
        public ImageController(IImageService imageService, IInAppStorageService inAppStorageService)
        {
            _imageService = imageService;
            _inAppStorageService = inAppStorageService;
        }

        [HttpPost("{issueId:int}")]
        public async Task<IActionResult> Save(int issueId, ImagesModel imagesModel)
        {
            try
            {
                Domain.Entities.Issues.Image image = new Domain.Entities.Issues.Image(imagesModel);
                await _imageService.Save(issueId, image);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{imageGuid:guid}")]
        public async Task<IActionResult> Delete(Guid imageGuid)
        {
            await _imageService.Delete(imageGuid);
            return NoContent();
        }

        [HttpPost("upload/{issueId}")]
        public async Task<IActionResult> Upload(int issueId, List<IFormFile> formFile)
        {
            try
            {
                await _imageService.Upload(issueId, formFile);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }
    }
}

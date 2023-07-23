using IssueTracker.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace IssueTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost("{issueId}")]
        public async Task<IActionResult> Upload(int issueId, List<IFormFile> formFile)
        {
            try
            {
                await _fileService.Upload(issueId, formFile);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpDelete("{fileGuid:guid}")]
        public async Task<IActionResult> Delete(Guid fileGuid)
        {
            await _fileService.Delete(fileGuid);
            return NoContent();
        }
    }
}

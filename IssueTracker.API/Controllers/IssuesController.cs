using IssueTracker.Domain.Entities.Issues;
using IssueTracker.Domain.Models.Issue;
using IssueTracker.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace IssueTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssueController : ControllerBase
    {
        private readonly IIssueService _issueService;
        public IssueController(IIssueService issueService)
        {
            _issueService = issueService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                return Ok(await _issueService.GetIssueById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _issueService.GetAllIssues());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(IssueModel issueModel)
        {
            try
            {
                Issue issue = new Issue(issueModel);
                var issueId = await _issueService.Upsert(issue);
                return CreatedAtRoute("", new { id = issueId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error occured while creating the Issue");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, IssueModel issueModel)
        {
            try
            {
                issueModel.Id = id;
                Issue issue = new Issue(issueModel);
                var issueId = await _issueService.Upsert(issue);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error occured while updating the Issue");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _issueService.DeleteIssue(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error occured while Deleting the Issue");
            }
        }
    }
}

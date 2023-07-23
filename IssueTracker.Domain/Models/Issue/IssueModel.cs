using IssueTracker.Domain.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Text.Json.Serialization;

namespace IssueTracker.Domain.Models.Issue
{
    public class IssueModel
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? AssignedTo { get; set; }
        public int CreatedBy { get; set; }
        public int? ClosedBy { get; set; }
        public DateTime? ClosedOn { get; set; }
        public DateTime? DeadLine { get; set; }
        public IssueStatus Status { get; set; }
        [ValidateNever]
        public List<IFormFile> files { get; set; }

    }
}

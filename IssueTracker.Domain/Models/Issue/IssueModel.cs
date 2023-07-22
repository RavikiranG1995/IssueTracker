using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace IssueTracker.Domain.Models.Issue
{
    public class IssueModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? AssignedTo { get; set; }
        public int CreatedBy { get; set; }
        public int? ClosedBy { get; set; }
        public DateTime? ClosedOn { get; set; }
        public DateTime? DeadLine { get; set; }
        public List<ImagesModel> Images { get; set; }
        public List<IFormFile> files { get; set; }

    }
    public class ImagesModel
    {
        [JsonIgnore]
        public string ImagePath { get; set; } = string.Empty;
        [JsonIgnore]
        public Guid? ImageGuid { get; set; }
        public string Base64Image { get; set; }
        public List<IFormFile> files { get; set; }
    }
}

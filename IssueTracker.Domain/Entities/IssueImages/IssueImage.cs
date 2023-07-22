using System.Text.Json.Serialization;

namespace IssueTracker.Domain.Entities.IssueImage
{
    public interface IImage
    {
        public string ImagePath { get; set; }
        public Guid? ImageGuid { get; set; }
        [JsonIgnore]
        public string Bas64Image { get; set; }
    }
}

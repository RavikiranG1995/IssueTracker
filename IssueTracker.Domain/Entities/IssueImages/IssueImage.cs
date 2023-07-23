using System.Text.Json.Serialization;

namespace IssueTracker.Domain.Entities.IssueImage
{
    public interface IFile
    {
        public string ImagePath { get; set; }
        public Guid? ImageGuid { get; set; }
    }
}

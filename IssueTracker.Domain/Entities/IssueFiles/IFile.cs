using System.Text.Json.Serialization;

namespace IssueTracker.Domain.Entities.IssueFiles
{
    public interface IFile
    {
        public string FilePath { get; set; }
        public Guid? FileGuid { get; set; }
    }
}

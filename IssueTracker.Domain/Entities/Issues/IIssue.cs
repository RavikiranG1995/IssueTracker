using IssueTracker.Domain.Constants;
using IssueTracker.Domain.Entities.IssueFiles;

namespace IssueTracker.Domain.Entities.Issues
{
    public interface IIssue
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? AssignedTo { get; set; }
        public int CreatedBy { get; set; }
        public int? ClosedBy { get; set; }
        public DateTime? ClosedOn { get; set; }
        public DateTime? DeadLine { get; set; }
        public IssueStatus Status { get; set; }
        public List<IFile> Files { get; set; }
    }
}

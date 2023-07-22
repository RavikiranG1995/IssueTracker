create proc usp_GetIssueBy_Id
(
@Id int
)
as
Select Id,Name,Description,DeadLine,IssueStatus,AssignedTo,CreatedBy,CreatedDate,LastUpdatedDate,ClosedBy,ClosedOn from Issues where Id=@Id
Select ImagePath,IssueId,ImageGuid from IssueImages where IssueId=@Id
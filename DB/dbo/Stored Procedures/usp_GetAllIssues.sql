create proc usp_GetAllIssues
as
Select Id,Name,Description,DeadLine,IssueStatus,AssignedTo,CreatedBy,CreatedDate,LastUpdatedDate,ClosedBy,ClosedOn from Issues
Select ImagePath,IssueId,ImageGuid from IssueImages
create proc usp_GetIssueImagesBy_IssueId
(
@IssueId int
)
as
Select ImagePath,IssueId,ImageGuid from IssueImages where IssueId=@IssueId
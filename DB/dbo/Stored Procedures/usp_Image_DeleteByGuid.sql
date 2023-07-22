Create proc usp_Image_DeleteByGuid
(
@ImageGuid uniqueidentifier
)
as
Delete from IssueImages where ImageGuid=@ImageGuid
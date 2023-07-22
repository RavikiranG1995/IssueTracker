create proc usp_Image_Save
(
@IssueId int,
@ImagePath varchar(300),
@ImageGuid uniqueidentifier
)
as
Insert into IssueImages values(@ImagePath,@IssueId,@ImageGuid)
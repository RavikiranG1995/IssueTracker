CREATE proc [dbo].[usp_Issues_Upsert]
(
@Id int=null,
@Name varchar(30)=null,
@Description varchar(300) =null,
@DeadLine datetime =null,
@IssueStatus int=null,
@AssignedTo int =null,
@CreatedBy int=null,
@ClosedBy int=null,
@ClosedOn datetime =null,
@ImageJsonData varchar(max)=null
)
AS
DECLARE @IssueId INT

IF EXISTS(SELECT 1 FROM Issues WHERE Id=@Id)
BEGIN
UPDATE Issues SET
				Name=ISNULL(@Name,Name),
				Description=ISNULL(@Description,Description),
				DeadLine=ISNULL(@DeadLine,DeadLine),
				IssueStatus=ISNULL(@IssueStatus,IssueStatus),
				AssignedTo=ISNULL(@AssignedTo,AssignedTo),
				LastUpdatedDate=GETDATE(),
				ClosedBy=ISNULL(@ClosedBy,ClosedBy),
				ClosedOn=ISNULL(@ClosedOn,ClosedOn)
				WHERE Id=@Id
				SET @IssueId=@Id
END
ELSE
BEGIN
DECLARE @table table (id int)
INSERT INTO Issues (
					 Name
					,Description
					,DeadLine
					,IssueStatus
					,AssignedTo
					,CreatedBy
					,ClosedBy
					,ClosedOn
					,Createddate
					,LastUpdatedDate
					)
					OUTPUT inserted.id into @table
					values
					(
					 @Name
					,@Description
					,@DeadLine
					,@IssueStatus
					,@AssignedTo
					,@CreatedBy
					,@ClosedBy
					,@ClosedOn
					,Getdate()
					,GETDATE()
					)
					SET @IssueId=(SELECT id FROM @table)
END
INSERT INTO IssueImages (
						IssueId
					   ,ImagePath
					   ,ImageGuid
					   )
				SELECT
					   @IssueId,
					   ImagePath,
					   ImageGuid
					   FROM OPENJSON(@ImageJsonData)
					   WITH(
							ImagePath varchar(300),
							ImageGuid uniqueidentifier
							)
SELECT @IssueId
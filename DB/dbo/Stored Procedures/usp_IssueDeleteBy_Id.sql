CREATE proc usp_IssueDeleteBy_Id  
(  
@Id int  
)  
as  
Delete from IssueImages where IssueId=@Id
Delete from Issues where Id=@Id  
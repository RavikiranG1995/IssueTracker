using IssueTracker.Domain.Constants;
using IssueTracker.Domain.Entities.IssueFiles;
using IssueTracker.Domain.Entities.Issues;
using IssueTracker.Domain.Repositories;
using IssueTracker.Infrastructure.Database.Helpers;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;

namespace IssueTracker.Infrastructure.Database.Issue
{
    public class IssueRepository : IIssueRepository
    {
        private readonly IDataBaseWrapper _dataBaseProxy;
        public IssueRepository(IDataBaseWrapper dataBaseProxy)
        {
            _dataBaseProxy = dataBaseProxy;
        }
        public async Task<int> Upsert(IIssue issue)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Id",issue.Id),
                new SqlParameter("@Name",issue.Name),
                new SqlParameter("@Description",issue.Description),
                new SqlParameter("@DeadLine",issue.DeadLine.HasValue?issue.DeadLine:DBNull.Value),
                new SqlParameter("@IssueStatus",(int)issue.Status),
                new SqlParameter("@AssignedTo",issue.AssignedTo.HasValue?issue.AssignedTo:DBNull.Value),
                new SqlParameter("@CreatedBy",issue.CreatedBy),
                new SqlParameter("@ClosedBy",issue.ClosedBy.HasValue?issue.ClosedBy:DBNull.Value),
                new SqlParameter("@ClosedOn",issue.ClosedOn.HasValue?issue.ClosedOn:DBNull.Value),
                new SqlParameter("@ImageJsonData",JsonConvert.SerializeObject(issue.Files))
            };

            var returnObject = await _dataBaseProxy.ExecuteScalarAsync("usp_Issues_Upsert", parameters, CancellationToken.None);
            var data = (int)returnObject;
            return data;
        }

        public async Task<IIssue> GetIssueById(int id)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Id",id)
            };
            using var issueDS = await _dataBaseProxy.GetDataSetAsync("usp_GetIssueBy_Id", parameters, CancellationToken.None);
            var issueDT = issueDS.Tables[0];
            Domain.Entities.Issues.Issue issue = null;
            foreach (DataRow row in issueDT.Rows)
            {
                issue = new Domain.Entities.Issues.Issue
                {
                    Id = (int)row["Id"],
                    Name = (string)row["Name"],
                    Description = (string)row["Description"],
                    AssignedTo = Convert.IsDBNull(row["AssignedTo"]) ? null : (int)row["AssignedTo"],
                    DeadLine = Convert.IsDBNull(row["DeadLine"]) ? null : (DateTime?)row["DeadLine"],
                    ClosedBy = Convert.IsDBNull(row["ClosedBy"]) ? null : (int)row["ClosedBy"],
                    ClosedOn = Convert.IsDBNull(row["ClosedOn"]) ? null : (DateTime?)row["ClosedOn"],
                    CreatedBy = (int)row["CreatedBy"],
                    Status = (IssueStatus)row["IssueStatus"]
                };

                var fileDT = issueDS.Tables[1];
                foreach (DataRow fileRow in fileDT.Rows)
                {
                    var file = new Domain.Entities.Issues.File
                    {
                        FilePath = Convert.IsDBNull(fileRow["ImagePath"]) ? null : fileRow["ImagePath"].ToString().Replace("\\", "/"),
                        FileGuid = Convert.IsDBNull(fileRow["ImageGuid"]) ? null : (Guid)fileRow["ImageGuid"],
                    };
                    issue.Files.Add(file);
                }
            }
            return issue;
        }

        public async Task<IEnumerable<IIssue>> GetAllIssues()
        {
            using var issueDS = await _dataBaseProxy.GetDataSetAsync("usp_GetAllIssues", null, CancellationToken.None);
            var issueDT = issueDS.Tables[0];
            var fileDT = issueDS.Tables[1];
            var issues = new List<Domain.Entities.Issues.Issue>();
            foreach (DataRow row in issueDT.Rows)
            {
                var issue = new Domain.Entities.Issues.Issue
                {
                    Id = (int)row["Id"],
                    Name = (string)row["Name"],
                    Description = (string)row["Description"],
                    AssignedTo = Convert.IsDBNull(row["AssignedTo"]) ? null : (int)row["AssignedTo"],
                    DeadLine = Convert.IsDBNull(row["DeadLine"]) ? null : (DateTime?)row["DeadLine"],
                    ClosedBy = Convert.IsDBNull(row["ClosedBy"]) ? null : (int)row["ClosedBy"],
                    ClosedOn = Convert.IsDBNull(row["ClosedOn"]) ? null : (DateTime?)row["ClosedOn"],
                    CreatedBy = (int)row["CreatedBy"],
                    Status = (IssueStatus)row["IssueStatus"]
                };
                var issueFiles = fileDT.AsEnumerable().Where(x => x.Field<int>("IssueId") == issue.Id).ToArray();
                foreach (DataRow? fileRow in issueFiles)
                {
                    var path = Path.GetFullPath((string)fileRow["ImagePath"]);
                    var file = new Domain.Entities.Issues.File
                    {
                        FilePath = Convert.IsDBNull(fileRow["ImagePath"]) ? null : fileRow["ImagePath"].ToString().Replace("\\","/"),
                        FileGuid = Convert.IsDBNull(fileRow["ImageGuid"]) ? null : (Guid)fileRow["ImageGuid"],
                    };
                    issue.Files.Add(file);
                }
                issues.Add(issue);
            }
            return issues;
        }

        public async Task DeleteIssue(int id)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Id",id)
            };
            await _dataBaseProxy.ExecuteScalarAsync("usp_IssueDeleteBy_Id", parameters, CancellationToken.None);
        }
    }
}

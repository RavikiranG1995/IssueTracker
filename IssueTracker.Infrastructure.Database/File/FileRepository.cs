using IssueTracker.Domain.Entities.IssueFiles;
using IssueTracker.Domain.Repositories;
using IssueTracker.Infrastructure.Database.Helpers;
using Microsoft.Data.SqlClient;
using System.Data;

namespace IssueTracker.Infrastructure.Database.File
{
    public class FileRepository : IFileRepository
    {
        private readonly IDataBaseWrapper _dataBaseProxy;
        public FileRepository(IDataBaseWrapper dataBaseProxy)
        {
            _dataBaseProxy = dataBaseProxy;
        }

        public async Task Delete(Guid fileGuid)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@ImageGuid",fileGuid)
            };
            var returnObject = await _dataBaseProxy.ExecuteScalarAsync("usp_Image_DeleteByGuid", parameters, CancellationToken.None);
        }
        public async Task<IList<IFile>> GetAllIssueFiles(int issueId)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@IssueId",issueId)
            };
            using var issueDS = await _dataBaseProxy.GetDataSetAsync("usp_GetIssueImagesBy_IssueId", parameters, CancellationToken.None);
            var filesDT = issueDS.Tables[0];
            var issueFiles = new List<IFile>();
            foreach (DataRow fileRow in filesDT.Rows)
            {
                var issueFile = new Domain.Entities.Issues.File
                {
                    FilePath = Convert.IsDBNull(fileRow["ImagePath"]) ? null : (string)fileRow["ImagePath"],
                    FileGuid = Convert.IsDBNull(fileRow["ImageGuid"]) ? null : (Guid)fileRow["ImageGuid"],
                };
                issueFiles.Add(issueFile);
            }
            return issueFiles;
        }

        public async Task SaveFile(int issueId, IFile file)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@IssueId",issueId),
                new SqlParameter("@ImageGuid",file.FileGuid),
                new SqlParameter("@ImagePath",file.FilePath),
            };
            var returnObject = await _dataBaseProxy.ExecuteScalarAsync("usp_Image_Save", parameters, CancellationToken.None);
        }
    }
}

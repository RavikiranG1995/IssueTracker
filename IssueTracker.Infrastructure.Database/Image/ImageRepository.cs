using IssueTracker.Domain.Entities.IssueImage;
using IssueTracker.Domain.Repositories;
using IssueTracker.Infrastructure.Database.Helpers;
using Microsoft.Data.SqlClient;
using System.Data;

namespace IssueTracker.Infrastructure.Database.Image
{
    public class ImageRepository : IImageRepository
    {
        private readonly IDataBaseWrapper _dataBaseProxy;
        public ImageRepository(IDataBaseWrapper dataBaseProxy)
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
            foreach (DataRow imageRow in filesDT.Rows)
            {
                var issueImage = new Domain.Entities.Issues.File
                {
                    ImagePath = Convert.IsDBNull(imageRow["ImagePath"]) ? null : (string)imageRow["ImagePath"],
                    ImageGuid = Convert.IsDBNull(imageRow["ImageGuid"]) ? null : (Guid)imageRow["ImageGuid"],
                };
                issueFiles.Add(issueImage);
            }
            return issueFiles;
        }

        public async Task SaveImage(int issueId, IFile file)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@IssueId",issueId),
                new SqlParameter("@ImageGuid",file.ImageGuid),
                new SqlParameter("@ImagePath",file.ImagePath),
            };
            var returnObject = await _dataBaseProxy.ExecuteScalarAsync("usp_Image_Save", parameters, CancellationToken.None);
        }
    }
}

using IssueTracker.Domain.Entities.IssueImage;
using IssueTracker.Domain.Entities.Issues;
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

        public async Task Delete(Guid imageGuid)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@ImageGuid",imageGuid)
            };
            var returnObject = await _dataBaseProxy.ExecuteScalarAsync("usp_Image_DeleteByGuid", parameters, CancellationToken.None);
        }
        public async Task<IList<IImage>> GetAllIssueImages(int issueId)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@IssueId",issueId)
            };
            using var issueDS = await _dataBaseProxy.GetDataSetAsync("usp_GetIssueImagesBy_IssueId", parameters, CancellationToken.None);
            var imagesDT = issueDS.Tables[0];
            var issueImages = new List<IImage>();
            foreach (DataRow imageRow in imagesDT.Rows)
            {
                var issueImage = new Domain.Entities.Issues.Image
                {
                    ImagePath = Convert.IsDBNull(imageRow["ImagePath"]) ? null : (string)imageRow["ImagePath"],
                    ImageGuid = Convert.IsDBNull(imageRow["ImageGuid"]) ? null : (Guid)imageRow["ImageGuid"],
                };
                issueImages.Add(issueImage);
            }
            return issueImages;
        }

        public async Task SaveImage(int issueId, IImage image)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@IssueId",issueId),
                new SqlParameter("@ImageGuid",image.ImageGuid),
                new SqlParameter("@ImagePath",image.ImagePath),
            };
            var returnObject = await _dataBaseProxy.ExecuteScalarAsync("usp_Image_Save", parameters, CancellationToken.None);
        }
    }
}

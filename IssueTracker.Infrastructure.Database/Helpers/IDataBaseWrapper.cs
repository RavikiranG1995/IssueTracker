using Microsoft.Data.SqlClient;
using System.Data;

namespace IssueTracker.Infrastructure.Database.Helpers
{
    public interface IDataBaseWrapper
    {
        Task<object> ExecuteScalarAsync(string procName, IEnumerable<SqlParameter> parameters, CancellationToken cancellationToken);
        int ExecuteNonQuery(string procName, IEnumerable<SqlParameter> parameters);
        Task<DataTable> GetDataTableAsync(string procName, IEnumerable<SqlParameter> parameters, string tableName, CancellationToken cancellationToken);
        Task<DataSet> GetDataSetAsync(string procName, IEnumerable<SqlParameter> parameters, CancellationToken cancellationToken);
    }
}

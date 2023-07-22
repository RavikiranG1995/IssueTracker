using Microsoft.Data.SqlClient;
using System.Data;

namespace IssueTracker.Infrastructure.Database.Helpers
{
    public class DataBaseWrapper : IDataBaseWrapper
    {
        private readonly string _connectionString;

        public DataBaseWrapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int ExecuteNonQuery(string procName, IEnumerable<SqlParameter> parameters)
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                return 0;
            }
            return Database.ExecuteNonQuery(procName, parameters, _connectionString);
        }

        public async Task<object> ExecuteScalarAsync(string procName, IEnumerable<SqlParameter> parameters, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                return null;
            }
            return await Database.ExecuteScalarAsync(procName, parameters, _connectionString, cancellationToken);
        }

        public async Task<DataTable> GetDataTableAsync(string procName, IEnumerable<SqlParameter> parameters, string tableName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                return null;
            }
            return await Database.GetDataTableAsync(procName, parameters, tableName, _connectionString, cancellationToken);
        }

        public async Task<DataSet> GetDataSetAsync(string procName, IEnumerable<SqlParameter> parameters, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                return null;
            }
            return await Database.GetDataSetAsync(procName, parameters, _connectionString, cancellationToken);
        }
    }
}

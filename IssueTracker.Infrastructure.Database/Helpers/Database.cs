using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Infrastructure.Database.Helpers
{
    public class Database
    {
        public static async Task<DataTable> GetDataTableAsync(string procName, IEnumerable<SqlParameter> parameters, string tableName, string connectionString, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                return null;
            }
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using SqlCommand command = conn.CreateCommand();
                command.CommandText = procName;
                command.CommandType = CommandType.StoredProcedure;
                if (parameters?.Any() ?? false)
                {
                    foreach (var param in parameters)
                    {
                        SqlParameter newParam = new SqlParameter(param.ParameterName, param.Value);
                        command.Parameters.Add(newParam);
                    }
                }
                await conn.OpenAsync(cancellationToken);
                dt.Load(await command.ExecuteReaderAsync(cancellationToken));
                conn.Close();
            }

            if (!string.IsNullOrEmpty(tableName))
            {
                dt.TableName = tableName;
            }

            return dt;
        }

        public static async Task<DataSet> GetDataSetAsync(string procName, IEnumerable<SqlParameter> parameters, string connectionString, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                return null;
            }
            DataSet ds = new DataSet();
            using SqlConnection conn = new SqlConnection(connectionString);
            using SqlCommand command = conn.CreateCommand();
            command.CommandText = procName;
            command.CommandType = CommandType.StoredProcedure;
            if (parameters?.Any() ?? false)
            {
                foreach (var param in parameters)
                {
                    SqlParameter sqlParameter = param;
                    if (sqlParameter.Value == null)
                    {
                        sqlParameter.Value = string.Empty;
                    }
                    SqlParameter newParam = new SqlParameter(param.ParameterName, param.Value);
                    command.Parameters.Add(newParam);
                }
            }
            try
            {
                await conn.OpenAsync(cancellationToken);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        public static int ExecuteNonQuery(string procName, IEnumerable<SqlParameter> parameters, string connectionString)
        {
            bool flag = parameters.Any(p => p.Direction == ParameterDirection.ReturnValue);
            using SqlConnection conn = new SqlConnection(connectionString);
            using SqlCommand command = conn.CreateCommand();
            command.CommandText = procName;
            command.CommandType = CommandType.StoredProcedure;
            if (flag)
            {
                command.Parameters.AddRange(parameters.ToArray());
            }
            else if (parameters.Any())
            {
                foreach (var param in parameters)
                {
                    SqlParameter sqlParameter = param;
                    if (sqlParameter.Value == null)
                    {
                        object obj = (sqlParameter.Value = string.Empty);
                    }

                    SqlParameter value = new SqlParameter(param.ParameterName, sqlParameter.Value);
                    command.Parameters.Add(value);
                }
            }

            conn.Open();
            if (flag)
            {
                command.ExecuteNonQuery();
                string parameterName = parameters.First(p => p.Direction == ParameterDirection.ReturnValue).ParameterName;
                return (int)command.Parameters[parameterName].Value;
            }

            return command.ExecuteNonQuery();
        }

        public static async Task<object> ExecuteScalarAsync(string procName, IEnumerable<SqlParameter> parameters, string connectionString, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                return null;
            }
            DataTable dt = new DataTable();

            using SqlConnection conn = new SqlConnection(connectionString);
            using SqlCommand command = conn.CreateCommand();
            command.CommandText = procName;
            command.CommandType = CommandType.StoredProcedure;
            if (parameters?.Any() ?? false)
            {
                foreach (var param in parameters)
                {
                    SqlParameter sqlParameter = param;
                    if (sqlParameter.Value == null)
                    {
                        sqlParameter.Value = string.Empty;
                    }
                    SqlParameter newParam = new SqlParameter(param.ParameterName, param.Value);
                    command.Parameters.Add(newParam);
                }
            }
            await conn.OpenAsync(cancellationToken);
            return await command.ExecuteScalarAsync(cancellationToken);
        }
    }
}

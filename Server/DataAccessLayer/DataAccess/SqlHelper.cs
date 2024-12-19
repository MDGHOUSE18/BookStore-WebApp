using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DataAccess
{
    public class SqlHelper
    {
        private readonly string _connectionString;

        // Constructor accepting IConfiguration
        public SqlHelper(IConfiguration configuration)
        {
            // Retrieve the connection string from the configuration
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Executes a stored procedure with parameters and returns a DataTable
        public async Task<DataTable> ExecuteStoredProcedureAsync(string storedProcedure, SqlParameter[] parameters)
        {
            Console.WriteLine(parameters);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    var dataTable = new DataTable();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        dataTable.Load(reader);
                    }
                    return dataTable;
                }
            }
        }

        // Executes a command with non-query results (e.g., Insert, Update, Delete)
        public async Task<int> ExecuteNonQueryAsync(string query, SqlParameter[] parameters)
        {
            Console.WriteLine(parameters);
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(query, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }

        // Executes a scalar query (returns a single value)
        public async Task<object> ExecuteScalarAsync(string query, SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(query, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    return await command.ExecuteScalarAsync();
                }
            }
        }
    }
}

using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace El_Cuervo_Gym_Web.Core.DataAccess
{
    public class Connection : IConnection
    {
        private readonly string _connectionString;

        public Connection(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection DbConnection => new NpgsqlConnection(_connectionString);

        public async Task<IEnumerable<T>> QueryAsync<T>(string query, object parameters = null)
        {
            using (var dbConnection = DbConnection)
            {
                return await dbConnection.QueryAsync<T>(query, parameters);
            }
        }

        public async Task<int> ExecuteAsync(string query, object parameters = null)
        {
            using (var dbConnection = DbConnection)
            {
                return await dbConnection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<T> ExecuteScalarAsync<T>(string query, object parameters = null)
        {
            using (var dbConnection = DbConnection)
            {
                return await dbConnection.ExecuteScalarAsync<T>(query, parameters);
            }
        }

        public async Task ExecuteSqlScriptAsync(string scriptPath)
        {
            var script = await File.ReadAllTextAsync(scriptPath);
            using (var dbConnection = DbConnection)
            {
                await dbConnection.ExecuteAsync(script);
            }
        }

        public async Task<T> QuerySingleAsync<T>(string query, object parameters = null)
        {
            using (var dbConnection = DbConnection)
            {
                return await dbConnection.QuerySingleAsync<T>(query, parameters);
            }               
        }
    }
}

using Dapper;
using Npgsql;
using System.Data;

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

        public async Task<T> QuerySingleAsync<T>(string query, object parameters = null)
        {
            using (var dbConnection = DbConnection)
            {
                return await dbConnection.QuerySingleAsync<T>(query, parameters);
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

        public async Task LogError(Exception ex, string contexto, string extraInfo = "")
        {
            var query = @"
                SELECT adm.InsertarError(
                    @Contexto,
                    @TipoError,
                    @MensajeException,
                    @StackTrace,
                    @InfoExtra
                )";

            var parameters = new
            {
                Contexto = contexto,
                TipoError = ex.GetType().Name,
                MensajeException = ex.Message,
                ex.StackTrace,
                InfoExtra = extraInfo
            };

            using (var dbConnection = DbConnection)
            {
                await dbConnection.ExecuteAsync(query, parameters);
            }
        }

        public async Task CorrerTablas()
        {
            await ExecuteSqlScriptAsync("Core/DataAccess/Scripts/0_ScriptTablas.txt");
            await ExecuteSqlScriptAsync("Core/DataAccess/Scripts/1_ScriptAdminFunctions.txt");
            await ExecuteSqlScriptAsync("Core/DataAccess/Scripts/2_ScriptParametroFunctions.txt");
            await ExecuteSqlScriptAsync("Core/DataAccess/Scripts/3_ScriptErroresFunctions.txt");
            await ExecuteSqlScriptAsync("Core/DataAccess/Scripts/4_ScriptSocioFunctions.txt");
            await ExecuteSqlScriptAsync("Core/DataAccess/Scripts/5_ScriptPagoFunctions.txt");
            await ExecuteSqlScriptAsync("Core/DataAccess/Scripts/6_ScriptIngresoFunctions.txt");
        }
    }
}
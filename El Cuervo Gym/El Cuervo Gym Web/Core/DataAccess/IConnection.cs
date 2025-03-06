using System.Collections.Generic;
using System.Threading.Tasks;

namespace El_Cuervo_Gym_Web.Core.DataAccess
{
    public interface IConnection
    {
        Task<IEnumerable<T>> QueryAsync<T>(string query, object parameters = null);
        Task<int> ExecuteAsync(string query, object parameters = null);
        Task<T> ExecuteScalarAsync<T>(string query, object parameters = null);
        Task<T> QuerySingleAsync<T>(string query, object parameters = null);
        Task ExecuteSqlScriptAsync(string scriptPath);
        Task LogError(Exception ex, string contexto, string extraInfo = "");
        Task CorrerTablas();
    }
}

using El_Cuervo_Gym_Web.Core.DataAccess;

namespace El_Cuervo_Gym_Web.Core.Utils.Logging
{
    public class CLogger : ICLogger
    {
        private readonly IConnection _connection;

        public CLogger(IConnection connection)
        {
            _connection = connection;
        }

        public async Task<string> LogError(Exception ex, string contexto, string extraInfo = "")
        {
            await _connection.LogError(ex, contexto, extraInfo);
            return "/Admin/Error";
        }

        public async Task<string> LogErrorSocio(Exception ex, string contexto, string extraInfo = "")
        {
            await _connection.LogError(ex, contexto, extraInfo);
            return "/Socio/Error";
        }
    }
}

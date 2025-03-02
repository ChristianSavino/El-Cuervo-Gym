using El_Cuervo_Gym_Web.Core.Admin.Domain;
using El_Cuervo_Gym_Web.Core.DataAccess;

namespace El_Cuervo_Gym_Web.Core.Admin.DataAccess
{
    public class AdminDataAccess : IAdminDataAccess
    {
        private readonly IConnection _connection;

        public AdminDataAccess(IConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<DatosAdminLogin>> ObtenerAdmin(string usuario, string password)
        {
            var query = "SELECT * FROM adm.LoginAdmin(@usuario, @password)";
            var parameters = new { usuario, password };

            return await _connection.QueryAsync<DatosAdminLogin>(query, parameters);
        }
    }
}

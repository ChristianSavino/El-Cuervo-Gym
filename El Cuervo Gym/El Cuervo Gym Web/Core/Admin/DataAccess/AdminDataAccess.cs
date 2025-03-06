using El_Cuervo_Gym_Web.Core.Admin.Domain;
using El_Cuervo_Gym_Web.Core.Admin.Domain.Requests;
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

        public async Task<int> InsertarAdmin(DatosAdmin admin)
        {
            var query = "SELECT adm.InsertarAdmin(@Usuario, @Password, @Estado, @IsMaster)";
            var parameters = new
            {
                admin.Usuario,
                admin.Password,
                admin.Estado,
                admin.IsMaster
            };

            return await _connection.QuerySingleAsync<int>(query, parameters);
        }

        public async Task<IEnumerable<DatosAdmin>> FiltrarAdmins(FiltroAdmin filtro)
        {
            var query = "SELECT * FROM adm.FiltrarAdmin(@Usuario, @Id, @IncluirDadosDeBaja)";
            var parameters = new
            {
                filtro.Usuario,
                Id = filtro.NumAdmin,
                filtro.IncluirDadosDeBaja
            };
            return await _connection.QueryAsync<DatosAdmin>(query, parameters);
        }

        public async Task<DatosAdmin> ObtenerAdminPorId(int adminId)
        {
            var query = "SELECT * FROM adm.ObtenerAdminPorId(@Id)";
            var parameters = new { Id = adminId };
            return await _connection.QuerySingleAsync<DatosAdmin>(query, parameters);
        }

        public async Task<bool> ActualizarAdmin(DatosAdmin admin)
        {
            var query = "SELECT adm.ActualizarAdmin(@Id, @Usuario, @Password, @Estado, @IsMaster)";
            var parameters = new
            {
                admin.Id,
                admin.Usuario,
                admin.Password,
                admin.Estado,
                admin.IsMaster
            };

            var result = await _connection.QuerySingleAsync<int>(query, parameters);
            return result > 0;
        }

        public async Task DarDeBajaAdmin(int adminId)
        {
            var query = "SELECT adm.DarDeBajaAdmin(@Id)";
            var parameters = new { Id = adminId };
            await _connection.ExecuteAsync(query, parameters);
        }
    }
}

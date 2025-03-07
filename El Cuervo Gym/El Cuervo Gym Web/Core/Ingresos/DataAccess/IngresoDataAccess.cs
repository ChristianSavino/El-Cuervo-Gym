using El_Cuervo_Gym_Web.Core.DataAccess;
using El_Cuervo_Gym_Web.Core.Ingresos.Domain;

namespace El_Cuervo_Gym_Web.Core.Ingresos.DataAccess
{
    public class IngresoDataAccess : IIngresoDataAccess
    {
        private readonly IConnection _connection;

        public IngresoDataAccess(IConnection connection)
        {
            _connection = connection;
        }

        public async Task BajaIngreso(int idIngreso)
        {
            var query = "SELECT soc.BajaIngreso(@IdIngreso);";
            var parameters = new
            {
                IdIngreso = idIngreso
            };

            var result = await _connection.ExecuteScalarAsync<bool>(query,parameters);
        }

        public async Task<int> InsertarIngreso(Ingreso ingreso)
        {
            var query = "SELECT soc.InsertarIngreso(@IdSocio, @FechaIngreso, @Estado);";
            var parameters = new
            {
                ingreso.IdSocio,
                ingreso.FechaIngreso,
                ingreso.Estado
            };

            return await _connection.ExecuteScalarAsync<int>(query, parameters);
        }

        public Task<IEnumerable<IngresoLista>> ObtenerIngresosEnElDia(DateTime fecha)
        {
            var query = "SELECT * FROM soc.IngresosDia(@Fecha);";
            var parameters = new
            {
                Fecha = fecha
            };

            return _connection.QueryAsync<IngresoLista>(query, parameters);
        }

        public async Task<IEnumerable<Ingreso>> ObtenerIngresosEnElDiaSocio(DateTime fecha, int idSocio)
        {
           var query = "SELECT * FROM soc.IngresosDiaSocio(@Fecha, @IdSocio);";
            var parameters = new
            {
                Fecha = fecha,
                IdSocio = idSocio
            };
            return await _connection.QueryAsync<Ingreso>(query, parameters);
        }
    }
}

using El_Cuervo_Gym_Web.Core.DataAccess;
using El_Cuervo_Gym_Web.Core.Socio.Domain;
using El_Cuervo_Gym_Web.Core.Socio.Domain.Request;

namespace El_Cuervo_Gym_Web.Core.Socio.DataAccess
{
    public class SocioDataAccess : ISocioDataAccess
    {
        private readonly IConnection _connection;

        public SocioDataAccess(IConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> InsertarSocio(DatosSocio socio)
        {
            var query = @"
                SELECT Soc.InsertarSocio(
                    @Nombre,
                    @Apellido,
                    @Documento,
                    @Telefono,
                    @ObraSocial,
                    @NumeroObraSocial,
                    @NumeroEmergencia,
                    @ContactoEmergencia,
                    @FechaSubscripcion,
                    @ProximoVencimientoCuota,
                    @Estado,
                    @IdAdmin
                )";

            var parameters = new
            {
                socio.Nombre,
                socio.Apellido,
                socio.Documento,
                socio.Telefono,
                socio.ObraSocial,
                socio.NumeroObraSocial,
                socio.NumeroEmergencia,
                socio.ContactoEmergencia,
                socio.FechaSubscripcion,
                socio.ProximoVencimientoCuota,
                socio.Estado,
                socio.IdAdmin
            };

            return await _connection.QuerySingleAsync<int>(query, parameters);
        }

        public async Task<IEnumerable<DatosSocio>> ObtenerSocios(FiltroSocio filtro)
        {
            var query = @"
                SELECT *
                FROM Soc.FiltrarSocios(
                    @Nombre,
                    @Documento,
                    @NumeroSocio,
                    @FechaInicio,
                    @FechaFin,
                    @CuotasVencidas,
                    @IncluirDadosDeBaja
                )";

            var parameters = new
            {
                filtro.Nombre,
                filtro.Documento,
                filtro.NumeroSocio,
                filtro.FechaInicio,
                filtro.FechaFin,
                filtro.CuotasVencidas,
                filtro.IncluirDadosDeBaja
            };

            return await _connection.QueryAsync<DatosSocio>(query, parameters);
        }

        public async Task<DatosSocio> ObtenerSocioPorId(int idSocio)
        {
            var query = @"
                SELECT *
                FROM Soc.ObtenerSocioPorId(@Id)";

            var parameters = new
            {
                Id = idSocio
            };

            return await _connection.QuerySingleAsync<DatosSocio>(query, parameters);
        }

        public async Task<DatosSocio> ObtenerSocioConPagosPorId(int idSocio)
        {
            var query = @"
                SELECT *
                FROM Soc.ObtenerSocioConPagosPorId(@Id)";

            var parameters = new
            {
                Id = idSocio
            };

            return await _connection.QuerySingleAsync<DatosSocio>(query, parameters);
        }

        public async Task<bool> ActualizarSocio(DatosSocio socio)
        {
            var query = @"
                SELECT Soc.ActualizarSocio(
                    @Id,
                    @Nombre,
                    @Apellido,
                    @Documento,
                    @Telefono,
                    @ObraSocial,
                    @NumeroObraSocial,
                    @NumeroEmergencia,
                    @ContactoEmergencia,
                    @FechaSubscripcion,
                    @ProximoVencimientoCuota,
                    @Estado,
                    @IdAdmin
                )";

            var parameters = new
            {
                socio.Id,
                socio.Nombre,
                socio.Apellido,
                socio.Documento,
                socio.Telefono,
                socio.ObraSocial,
                socio.NumeroObraSocial,
                socio.NumeroEmergencia,
                socio.ContactoEmergencia,
                socio.FechaSubscripcion,
                socio.ProximoVencimientoCuota,
                socio.Estado,
                socio.IdAdmin
            };

            var affectedRows = await _connection.ExecuteScalarAsync<int>(query, parameters);
            return affectedRows > 0;
        }

        public async Task<bool> DarDeBajaSocio(int socioId)
        {
            var query = "SELECT Soc.DarDeBajaSocio(@Id)";
            var parameters = new { Id = socioId };

            var affectedRows = await _connection.ExecuteScalarAsync<int>(query, parameters);
            return affectedRows > 0;
        }

        public async Task<bool> ActualizarProximaFechaVencimiento(int socioId, DateTime fechaProxima)
        {
            var query = "SELECT Soc.ActualizarProximaFechaVencimiento(@Id, @FechaProxima)";
            var parameters = new { Id = socioId, FechaProxima = fechaProxima };

            var affectedRows = await _connection.ExecuteScalarAsync<int>(query, parameters);
            return affectedRows > 0;
        }
    }
}
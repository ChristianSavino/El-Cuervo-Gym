using El_Cuervo_Gym_Web.Core.Cobranza.Domain;
using El_Cuervo_Gym_Web.Core.Cobranza.Domain.Request;
using El_Cuervo_Gym_Web.Core.DataAccess;

namespace El_Cuervo_Gym_Web.Core.Cobranza.DataAccess
{
    public class CobranzaDataAccess : ICobranzaDataAccess
    {
        private readonly IConnection _connection;

        public CobranzaDataAccess(IConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> InsertarCobranza(Pago cobranza)
        {
            var query = @"
                SELECT Soc.InsertarPago(
                    @IdSocio,
                    @FechaPago,
                    @FechaCuota,
                    @Monto,
                    @Estado,
                    @IdAdmin,
                    @MetodoPago,
                    @Comprobante
                )";

            var parameters = new
            {
                cobranza.IdSocio,
                cobranza.FechaPago,
                cobranza.FechaCuota,
                cobranza.Monto,
                cobranza.Estado,
                cobranza.IdAdmin,
                cobranza.MetodoPago,
                cobranza.Comprobante
            };

            return await _connection.QuerySingleAsync<int>(query, parameters);
        }

        public async Task<IEnumerable<PagoListado>> ObtenerCobranzasFiltro(FiltroCobranza filtro)
        {
            var query = @"
                SELECT * FROM Soc.FiltrarPagos(
                    @Nombre,
                    @Documento,
                    @NumeroSocio,
                    @FechaInicio,
                    @FechaFin,
                    @IncluirDadosDeBaja,
                    @MetodoPago
                )";

            var parameters = new
            {
                filtro.Nombre,
                filtro.Documento,
                filtro.NumeroSocio,
                filtro.FechaInicio,
                filtro.FechaFin,
                filtro.IncluirDadosDeBaja,
                filtro.MetodoPago
            };

            return await _connection.QueryAsync<PagoListado>(query, parameters);
        }

        public async Task<PagoListado> ObtenerCobranzaPorId(int id)
        {
            var query = @"
                SELECT * FROM Soc.ObtenerPagoPorId(@Id)";

            var parameters = new { Id = id };

            return await _connection.QuerySingleAsync<PagoListado>(query, parameters);
        }

        public async Task<bool> ExistePagosPosteriores(FiltroCobranza filtro)
        {
            var query = @"
        SELECT Soc.ExistePagosPosteriores(
            @Id,
            @FechaCuota
        )";

            var parameters = new
            {
                filtro.Id,
                filtro.FechaCuota
            };

            return await _connection.QuerySingleAsync<bool>(query, parameters);
        }
    }
}
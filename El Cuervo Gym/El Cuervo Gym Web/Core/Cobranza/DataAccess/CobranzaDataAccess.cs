using El_Cuervo_Gym_Web.Core.Cobranza.Domain;
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
    }
}











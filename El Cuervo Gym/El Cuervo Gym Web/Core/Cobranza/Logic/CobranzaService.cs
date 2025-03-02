using El_Cuervo_Gym_Web.Core.Cobranza.DataAccess;
using El_Cuervo_Gym_Web.Core.Cobranza.Domain;
using El_Cuervo_Gym_Web.Core.Socio.Domain;
using El_Cuervo_Gym_Web.Core.Utils;

namespace El_Cuervo_Gym_Web.Core.Cobranza.Logic
{
    public class CobranzaService : ICobranzaService
    {
        private readonly ICobranzaDataAccess _cobranzaDataAccess;

        public CobranzaService(ICobranzaDataAccess cobranzaDataAccess)
        {
            _cobranzaDataAccess = cobranzaDataAccess;
        }

        public async Task<int> InsertarCobranza(Pago cobranza)
        {
            return await _cobranzaDataAccess.InsertarCobranza(cobranza);
        }

        public async Task<int> InsertarCobranzaNuevoSocio(DatosSocio socio)
        {
            var pago = new Pago
            {
                IdSocio = socio.Id,
                FechaPago = DateTime.Now,
                FechaCuota = socio.FechaSubscripcion,
                Monto = 24000,
                Comprobante = "Pago de inscripción",
                Estado = Estado.Activo,
                IdAdmin = socio.IdAdmin,
                MetodoPago = TipoPago.Inicial
            };

            return await InsertarCobranza(pago);
        }
    }
}

using El_Cuervo_Gym_Web.Core.Cobranza.DataAccess;
using El_Cuervo_Gym_Web.Core.Cobranza.Domain;
using El_Cuervo_Gym_Web.Core.Cobranza.Domain.Request;
using El_Cuervo_Gym_Web.Core.Parametros.Logic;
using El_Cuervo_Gym_Web.Core.Socio.Domain;
using El_Cuervo_Gym_Web.Core.Utils;

namespace El_Cuervo_Gym_Web.Core.Cobranza.Logic
{
    public class CobranzaService : ICobranzaService
    {
        private readonly ICobranzaDataAccess _cobranzaDataAccess;
        private readonly IParametros _parametros;

        public CobranzaService(ICobranzaDataAccess cobranzaDataAccess, IParametros parametros)
        {
            _cobranzaDataAccess = cobranzaDataAccess;
            _parametros = parametros;
        }

        public async Task<bool> DarDeBajaComprobante(int idComprobante)
        {
            return await _cobranzaDataAccess.DarDeBajaComprobante(idComprobante);
        }

        public async Task<int> InsertarCobranza(Pago cobranza)
        {
            return await _cobranzaDataAccess.InsertarCobranza(cobranza);
        }

        public async Task<int> InsertarCobranzaNuevoSocio(DatosSocio socio)
        {
            var montoCuota = int.Parse(_parametros.ObtenerTodosLosParametros().First(x => x.Clave == Helper.ValorCuotaParamName).Valor);

            var pago = new Pago
            {
                IdSocio = socio.Id,
                FechaPago = DateTime.Now,
                FechaCuota = socio.FechaSubscripcion,
                Monto = montoCuota,
                Comprobante = "Pago de inscripción",
                Estado = Estado.Activo,
                IdAdmin = socio.IdAdmin,
                MetodoPago = TipoPago.Inicial
            };

            return await InsertarCobranza(pago);
        }

        public async Task<PagoListado> ObtenerCobranzaPorId(int idCobranza)
        {
            return await _cobranzaDataAccess.ObtenerCobranzaPorId(idCobranza);
        }

        public async Task<(PagoListado cobranza, bool existenPagosPosteriores)> ObtenerCobranzaPorIdValidada(int idCobranza)
        {
            var cobranza = await ObtenerCobranzaPorId(idCobranza);

            var filtroCobranza = new FiltroCobranza
            {
                FechaCuota = cobranza.FechaCuota,
                NumeroSocio = cobranza.IdSocio,
                Id = cobranza.Id
            };

            var existePagosPosteriores = await _cobranzaDataAccess.ExistePagosPosteriores(filtroCobranza);

            return (cobranza, existePagosPosteriores);
        }

        public async Task<IEnumerable<PagoListado>> ObtenerCobranzasFiltro(FiltroCobranza filtro)
        {
            return await _cobranzaDataAccess.ObtenerCobranzasFiltro(filtro);
        }
    }
}

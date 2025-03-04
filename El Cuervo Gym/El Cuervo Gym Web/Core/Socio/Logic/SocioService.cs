using El_Cuervo_Gym_Web.Core.Cobranza.Domain;
using El_Cuervo_Gym_Web.Core.Cobranza.Logic;
using El_Cuervo_Gym_Web.Core.Socio.DataAccess;
using El_Cuervo_Gym_Web.Core.Socio.Domain;
using El_Cuervo_Gym_Web.Core.Utils;
using Newtonsoft.Json;
using static El_Cuervo_Gym_Web.Pages.Admin.Socio.ListarSocioModel;

namespace El_Cuervo_Gym_Web.Core.Socio.Logic
{
    public class SocioService : ISocioService
    {
        private readonly ISocioDataAccess _socioDataAccess;
        private readonly ICobranzaService _cobranzaService;

        public SocioService(ISocioDataAccess socioDataAccess, ICobranzaService cobranzaService)
        {
            _socioDataAccess = socioDataAccess;
            _cobranzaService = cobranzaService;
        }

        public async Task<int> InsertarSocio(DatosSocio socio)
        {
            return await _socioDataAccess.InsertarSocio(socio);
        }

        public async Task<IEnumerable<DatosSocio>> ObtenerSocios(FiltroModel filtro)
        {
            try
            {
                return await _socioDataAccess.ObtenerSocios(filtro);
            }
            catch (Exception)
            {

                throw;
            }            
        }

        public async Task<DatosSocio> ObtenerSocioPorId(int idSocio)
        {
            return await _socioDataAccess.ObtenerSocioPorId(idSocio);
        }

        public async Task<DatosSocio> ObtenerSocioConPagosPorId(int idSocio)
        {
            var socio = await _socioDataAccess.ObtenerSocioConPagosPorId(idSocio);
            socio.UltimosPagos = JsonConvert.DeserializeObject<List<Pago>>(socio.PagosJson);
            socio.UltimosPagos = socio.UltimosPagos.OrderByDescending(p => p.FechaPago).ToList();

            return socio;
        }

        public async Task<bool> ActualizarSocio(DatosSocio socio)
        {
            return await _socioDataAccess.ActualizarSocio(socio);
        }

        public async Task<bool> DarDeBajaSocio(int socioId)
        {
            return await _socioDataAccess.DarDeBajaSocio(socioId);
        }

        public async Task<bool> ValidarSiSocioExiste(int documento)
        {
            var socios = await _socioDataAccess.ObtenerSocios(new FiltroModel() { Documento = documento.ToString()});
            return socios.Any();
        }

        public async Task<DateTime> CobrarSocio(int socioId, DateTime fechaCuota, Pago pago)
        {
            var fechaProxima = Helper.ObtenerProximoVencimientoDeCuota(fechaCuota);
            var result = await _cobranzaService.InsertarCobranza(pago);

            await _socioDataAccess.ActualizarProximaFechaVencimiento(socioId, fechaProxima);

            return fechaProxima;
        }
    }
}

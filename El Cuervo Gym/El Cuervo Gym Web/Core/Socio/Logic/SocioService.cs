using El_Cuervo_Gym_Web.Core.Cobranza.Domain;
using El_Cuervo_Gym_Web.Core.Socio.DataAccess;
using El_Cuervo_Gym_Web.Core.Socio.Domain;
using Newtonsoft.Json;
using static El_Cuervo_Gym_Web.Pages.Admin.Socio.ListarSocioModel;

namespace El_Cuervo_Gym_Web.Core.Socio.Logic
{
    public class SocioService : ISocioService
    {
        private readonly ISocioDataAccess _socioDataAccess;

        public SocioService(ISocioDataAccess socioDataAccess)
        {
            _socioDataAccess = socioDataAccess;
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
    }
}

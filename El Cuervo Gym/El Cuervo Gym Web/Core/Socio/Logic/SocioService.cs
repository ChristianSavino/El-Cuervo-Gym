using El_Cuervo_Gym_Web.Core.Cobranza.Domain;
using El_Cuervo_Gym_Web.Core.Socio.DataAccess;
using El_Cuervo_Gym_Web.Core.Socio.Domain;
using El_Cuervo_Gym_Web.Core.Socio.Domain.Request;
using Newtonsoft.Json;

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

        public async Task<IEnumerable<DatosSocio>> ObtenerSocios(FiltroSocio filtro)
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
            if (!string.IsNullOrEmpty(socio.PagosJson))
            {
                socio.UltimosPagos = JsonConvert.DeserializeObject<List<Pago>>(socio.PagosJson);
                socio.UltimosPagos = socio.UltimosPagos.OrderByDescending(p => p.FechaPago).ToList();
            }
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
            var socios = await _socioDataAccess.ObtenerSocios(new FiltroSocio() { Documento = documento.ToString() });
            return socios.Any();
        }

        public async Task<bool> ActualizarProximaFechaVencimiento(int socioId, DateTime fechaProxima)
        {
            return await _socioDataAccess.ActualizarProximaFechaVencimiento(socioId, fechaProxima);
        }

        public async Task<DatosSocio> LogearSocio(int documento, int nroSocio)
        {
            var socioExiste = await _socioDataAccess.LogearSocio(documento, nroSocio);
            if(socioExiste)
            {
                return await _socioDataAccess.ObtenerSocioConPagosPorId(nroSocio);
            }

            throw new Exception("Usuario incorrecto");
        }
    }
}

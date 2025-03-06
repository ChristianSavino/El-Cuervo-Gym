using El_Cuervo_Gym_Web.Core.Admin.DataAccess;
using El_Cuervo_Gym_Web.Core.Admin.Domain;
using El_Cuervo_Gym_Web.Core.Cobranza.Domain;
using El_Cuervo_Gym_Web.Core.Cobranza.Logic;
using El_Cuervo_Gym_Web.Core.Socio.Domain;
using El_Cuervo_Gym_Web.Core.Socio.Logic;
using El_Cuervo_Gym_Web.Core.Utils;

namespace El_Cuervo_Gym_Web.Core.Admin.Logic
{
    public class AdminService : IAdminService
    {
        private readonly IAdminDataAccess _dataAccess;
        private readonly ISocioService _socioService;
        private readonly ICobranzaService _cobranzaService;

        public AdminService(IAdminDataAccess dataAccess, ISocioService socioService, ICobranzaService cobranzaService)
        {
            _dataAccess = dataAccess;
            _socioService = socioService;
            _cobranzaService = cobranzaService;
        }

        public async Task<DatosAdminLogin> ObtenerAdmin(string usuario, string password)
        {
            var result = await _dataAccess.ObtenerAdmin(usuario, password);
            var admin = result.FirstOrDefault();
            if (admin == null)
            {
                throw new Exception("Usuario o contraseña incorrectos");
            }

            return admin;
        }

        public async Task<DatosSocio> InsertarSocio(DatosSocio socio)
        {
            var socioExistente = await _socioService.ValidarSiSocioExiste(socio.Documento);
            if(socioExistente)
            {
                throw new SocioYaExisteException("Ya existe un socio con el documento ingresado");
            }

            var proximaCuotaPago = Helper.ObtenerProximoVencimientoDeCuota(socio.FechaSubscripcion);

            socio.ProximoVencimientoCuota = proximaCuotaPago;

            socio.Id = await _socioService.InsertarSocio(socio);

            if (socio.FechaSubscripcion.ToString("MM/yyyy") == DateTime.Now.ToString("MM/yyyy"))
            {
                var pagoId = await _cobranzaService.InsertarCobranzaNuevoSocio(socio);
            }

            return socio;
        }

        public async Task<bool> ActualizarSocio(DatosSocio socio)
        {
            var proximaCuotaPago = Helper.ObtenerProximoVencimientoDeCuota(socio.FechaSubscripcion);
            socio.ProximoVencimientoCuota = proximaCuotaPago;

            return await _socioService.ActualizarSocio(socio);
        }

        public async Task<DateTime> CobrarSocio(int socioId, DateTime fechaCuota, Pago pago)
        {
            var fechaProxima = Helper.ObtenerProximoVencimientoDeCuota(fechaCuota);
            var result = await _cobranzaService.InsertarCobranza(pago);

            await _socioService.ActualizarProximaFechaVencimiento(socioId, fechaProxima);

            return fechaProxima;
        }

        public async Task DarDeBajaComprobante(int idComprobante)
        {
            var comprobante = await _cobranzaService.ObtenerCobranzaPorId(idComprobante);
            await _cobranzaService.DarDeBajaComprobante(idComprobante);
            await _socioService.ActualizarProximaFechaVencimiento(comprobante.IdSocio, comprobante.FechaCuota);
        }

        public async Task<int> InsertarAdmin(DatosAdmin admin)
        {
            return await _dataAccess.InsertarAdmin(admin);
        }
    }
}

using El_Cuervo_Gym_Web.Core.Admin.DataAccess;
using El_Cuervo_Gym_Web.Core.Admin.Domain;
using El_Cuervo_Gym_Web.Core.Admin.Domain.Requests;
using El_Cuervo_Gym_Web.Core.Cobranza.Domain;
using El_Cuervo_Gym_Web.Core.Cobranza.Logic;
using El_Cuervo_Gym_Web.Core.Socio.Domain;
using El_Cuervo_Gym_Web.Core.Socio.Logic;
using El_Cuervo_Gym_Web.Core.Utils;
using El_Cuervo_Gym_Web.Core.WhatsApp.Logic;

namespace El_Cuervo_Gym_Web.Core.Admin.Logic
{
    public class AdminService : IAdminService
    {
        private readonly IAdminDataAccess _dataAccess;
        private readonly ISocioService _socioService;
        private readonly ICobranzaService _cobranzaService;
        private readonly IWhatsAppService _whatsAppService;

        public AdminService(IAdminDataAccess dataAccess, ISocioService socioService, ICobranzaService cobranzaService, IWhatsAppService whatsAppService)
        {
            _dataAccess = dataAccess;
            _socioService = socioService;
            _cobranzaService = cobranzaService;
            _whatsAppService = whatsAppService;
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

            await _whatsAppService.EnviarMensajeAsync(socio.Telefono.ToString(), socio);

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

        public async Task<IEnumerable<DatosAdmin>> FiltrarAdmins(FiltroAdmin filtro)
        {
            return await _dataAccess.FiltrarAdmins(filtro);
        }

        public async Task<DatosAdmin> ObtenerAdminPorId(int adminId)
        {
            return await _dataAccess.ObtenerAdminPorId(adminId);
        }

        public async Task<bool> ActualizarAdmin(DatosAdmin admin)
        {
            return await _dataAccess.ActualizarAdmin(admin);
        }

        public async Task DarDeBajaAdmin(int adminId)
        {
            await _dataAccess.DarDeBajaAdmin(adminId);
        }

        public async Task<DateTime> ReIngresoSocio(int socioId, DateTime fechaCuota, Pago pago)
        {
            var socio = await _socioService.ObtenerSocioPorId(socioId);
            if(socio.ProximoVencimientoCuota.Date <= fechaCuota.Date)
            {
                throw new Exception("El socio ya ha sido reincorporado o se encuentra al día con las cuotas.");
            }

            return await CobrarSocio(socioId, fechaCuota, pago);
        }
    }
}

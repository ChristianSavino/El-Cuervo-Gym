using El_Cuervo_Gym_Web.Core.Admin.DataAccess;
using El_Cuervo_Gym_Web.Core.Admin.Domain;
using El_Cuervo_Gym_Web.Core.Cobranza.Logic;
using El_Cuervo_Gym_Web.Core.Socio.Domain;
using El_Cuervo_Gym_Web.Core.Socio.Logic;

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
            try
            {
                var result = await _dataAccess.ObtenerAdmin(usuario, password);
                var admin = result.FirstOrDefault();
                if (admin == null)
                {
                    throw new Exception("Usuario o contraseña incorrectos");
                }

                return admin;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<DatosSocio> InsertarSocio(DatosSocio socio)
        {
            try
            {
                var proximaCuotaPago = socio.FechaSubscripcion.AddMonths(1);
                if(proximaCuotaPago.Date < DateTime.Now.Date)
                {
                    proximaCuotaPago = new DateTime(DateTime.Now.Year, DateTime.Now.Month, socio.FechaSubscripcion.Day);
                    proximaCuotaPago = proximaCuotaPago.AddMonths(1);
                }

                socio.ProximoVencimientoCuota = proximaCuotaPago;

                socio.Id = await _socioService.InsertarSocio(socio);

                if (socio.FechaSubscripcion.ToString("MM/yyyy") == DateTime.Now.ToString("MM/yyyy"))
                {
                    var pagoId = await _cobranzaService.InsertarCobranzaNuevoSocio(socio);
                }

                return socio;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

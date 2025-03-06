using El_Cuervo_Gym_Web.Core.Admin.Domain;
using El_Cuervo_Gym_Web.Core.Cobranza.Domain;
using El_Cuervo_Gym_Web.Core.Socio.Domain;

namespace El_Cuervo_Gym_Web.Core.Admin.Logic
{
    public interface IAdminService
    {
        Task<DatosAdminLogin> ObtenerAdmin(string usuario, string password);
        Task<DatosSocio> InsertarSocio(DatosSocio socio);
        Task<bool> ActualizarSocio(DatosSocio socio);
        Task<DateTime> CobrarSocio(int socioId, DateTime fechaCuota, Pago pago);
        Task DarDeBajaComprobante(int idComprobante);
        Task<int> InsertarAdmin(DatosAdmin admin);
    }
}

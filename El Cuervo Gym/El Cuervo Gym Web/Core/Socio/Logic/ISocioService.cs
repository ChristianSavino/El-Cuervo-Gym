using El_Cuervo_Gym_Web.Core.Cobranza.Domain;
using El_Cuervo_Gym_Web.Core.Socio.Domain;
using static El_Cuervo_Gym_Web.Pages.Admin.Socio.ListarSocioModel;

namespace El_Cuervo_Gym_Web.Core.Socio.Logic
{
    public interface ISocioService
    {
        Task<int> InsertarSocio(DatosSocio socio);
        Task<IEnumerable<DatosSocio>> ObtenerSocios(FiltroModel filtro);
        Task<DatosSocio> ObtenerSocioPorId(int idSocio);
        Task<DatosSocio> ObtenerSocioConPagosPorId(int idSocio);
        Task<bool> ActualizarSocio(DatosSocio socio);
        Task<bool> DarDeBajaSocio(int socioId);
        Task<bool> ValidarSiSocioExiste(int documento);
        Task<DateTime> CobrarSocio(int socioId, DateTime fechaCuota, Pago pago);
    }
}

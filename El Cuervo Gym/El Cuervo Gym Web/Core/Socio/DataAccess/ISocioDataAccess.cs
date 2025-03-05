using El_Cuervo_Gym_Web.Core.Socio.Domain;
using El_Cuervo_Gym_Web.Core.Socio.Domain.Request;

namespace El_Cuervo_Gym_Web.Core.Socio.DataAccess
{
    public interface ISocioDataAccess
    {
        Task<int> InsertarSocio(DatosSocio socio);
        Task<IEnumerable<DatosSocio>> ObtenerSocios(FiltroSocio filtro);
        Task<DatosSocio> ObtenerSocioPorId(int idSocio);
        Task<DatosSocio> ObtenerSocioConPagosPorId(int idSocio);
        Task<bool> ActualizarSocio(DatosSocio socio);
        Task<bool> DarDeBajaSocio(int socioId);
        Task<bool> ActualizarProximaFechaVencimiento(int socioId, DateTime fechaProxima);
    }
}
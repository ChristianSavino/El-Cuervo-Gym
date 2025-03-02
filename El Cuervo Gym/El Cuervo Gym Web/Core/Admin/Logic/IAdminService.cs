using El_Cuervo_Gym_Web.Core.Admin.Domain;
using El_Cuervo_Gym_Web.Core.Socio.Domain;
using System.Threading.Tasks;

namespace El_Cuervo_Gym_Web.Core.Admin.Logic
{
    public interface IAdminService
    {
        Task<DatosAdminLogin> ObtenerAdmin(string usuario, string password);
        Task<DatosSocio> InsertarSocio(DatosSocio socio);
        Task<bool> ActualizarSocio(DatosSocio socio);
    }
}

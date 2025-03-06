using El_Cuervo_Gym_Web.Core.Admin.Domain;
using El_Cuervo_Gym_Web.Core.Admin.Domain.Requests;

namespace El_Cuervo_Gym_Web.Core.Admin.DataAccess
{
    public interface IAdminDataAccess
    {
        Task<IEnumerable<DatosAdminLogin>> ObtenerAdmin(string usuario, string password);
        Task<int> InsertarAdmin(DatosAdmin admin);
        Task<IEnumerable<DatosAdmin>> FiltrarAdmins(FiltroAdmin filtro);
        Task<DatosAdmin> ObtenerAdminPorId(int adminId);
        Task<bool> ActualizarAdmin(DatosAdmin admin);
        Task DarDeBajaAdmin(int adminId);
    }
}

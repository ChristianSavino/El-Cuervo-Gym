using El_Cuervo_Gym_Web.Core.Admin.Domain;

namespace El_Cuervo_Gym_Web.Core.Admin.DataAccess
{
    public interface IAdminDataAccess
    {
        Task<IEnumerable<DatosAdminLogin>> ObtenerAdmin(string usuario, string password);
        Task<int> InsertarAdmin(DatosAdmin admin);
    }
}

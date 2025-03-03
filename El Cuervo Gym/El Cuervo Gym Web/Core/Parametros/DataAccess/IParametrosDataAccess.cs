using El_Cuervo_Gym_Web.Core.Parametros.Domain;

namespace El_Cuervo_Gym_Web.Core.Parametros.DataAccess
{
    public interface IParametrosDataAccess
    {
        Task<IEnumerable<Parametro>> ObtenerParametros();
        Task ActualizarParametro(Parametro parametro);
    }
}

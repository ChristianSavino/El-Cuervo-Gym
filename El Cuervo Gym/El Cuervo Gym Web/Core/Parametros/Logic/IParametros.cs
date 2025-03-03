using El_Cuervo_Gym_Web.Core.Parametros.Domain;

namespace El_Cuervo_Gym_Web.Core.Parametros.Logic
{
    public interface IParametros
    {
        List<Parametro> ObtenerTodosLosParametros();
        Task ActualizarTodosLosParametros(IEnumerable<Parametro> parametros);

    }
}

using El_Cuervo_Gym_Web.Core.Parametros.DataAccess;
using El_Cuervo_Gym_Web.Core.Parametros.Domain;

namespace El_Cuervo_Gym_Web.Core.Parametros.Logic
{
    public class Parametros : IParametros
    {
        private readonly IParametrosDataAccess _parametrosDataAccess;
        private IEnumerable<Parametro> _parametros;

        public Parametros(IParametrosDataAccess parametrosDataAccess)
        {
            _parametrosDataAccess = parametrosDataAccess;
             CargarTodosLosParametros().Wait();
        }

        private async Task CargarTodosLosParametros()
        {
            _parametros = await _parametrosDataAccess.ObtenerParametros();
        }

        public List<Parametro> ObtenerTodosLosParametros()
        {
            return _parametros.ToList();
        }

        public async Task ActualizarTodosLosParametros(IEnumerable<Parametro> parametros)
        {
            foreach (var parametro in parametros)
            {
                await _parametrosDataAccess.ActualizarParametro(parametro);
            }

            _parametros = parametros;
        }
    }
}

using El_Cuervo_Gym_Web.Core.DataAccess;
using El_Cuervo_Gym_Web.Core.Parametros.Domain;

namespace El_Cuervo_Gym_Web.Core.Parametros.DataAccess
{
    public class ParametrosDataAccess : IParametrosDataAccess
    {
        private readonly IConnection _connection;

        public ParametrosDataAccess(IConnection connection)
        {
            _connection = connection;
        }

        public async Task ActualizarParametro(Parametro parametro)
        {
            var query = @"
                SELECT adm.ActualizarParametro(
                    @Clave,
                    @Valor,
                    @Descripcion
                )";

            var parameters = new
            {
                parametro.Clave,
                parametro.Valor,
                parametro.Descripcion
            };

            await _connection.ExecuteAsync(query, parameters);
        }

        public async Task<IEnumerable<Parametro>> ObtenerParametros()
        {
            var query = "SELECT Id, Clave, Valor, Descripcion, FechaModificacion FROM adm.Parametros";
            return await _connection.QueryAsync<Parametro>(query);
        }
    }
}

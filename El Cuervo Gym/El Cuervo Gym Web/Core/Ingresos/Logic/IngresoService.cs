using El_Cuervo_Gym_Web.Core.Ingresos.DataAccess;
using El_Cuervo_Gym_Web.Core.Ingresos.Domain;
using El_Cuervo_Gym_Web.Core.Ingresos.Domain.Request;

namespace El_Cuervo_Gym_Web.Core.Ingresos.Logic
{
    public class IngresoService : IIngresoService
    {
        private readonly IIngresoDataAccess _ingresoDataAccess;

        public IngresoService(IIngresoDataAccess ingresoDataAccess)
        {
            _ingresoDataAccess = ingresoDataAccess;
        }

        public async Task BajaIngreso(int idIngreso)
        {
            await _ingresoDataAccess.BajaIngreso(idIngreso);
        }

        public async Task<int> InsertarIngreso(Ingreso ingreso)
        {
            return await _ingresoDataAccess.InsertarIngreso(ingreso);
        }

        public async Task<IEnumerable<IngresoLista>> ObtenerIngresosEnElDia(DateTime fecha)
        {
            return await _ingresoDataAccess.ObtenerIngresosEnElDia(fecha);
        }

        public async Task<IEnumerable<Ingreso>> ObtenerIngresosEnElDiaSocio(DateTime fecha, int idSocio)
        {
            return await _ingresoDataAccess.ObtenerIngresosEnElDiaSocio(fecha, idSocio);
        }

        public async Task<IEnumerable<IngresoLista>> ObtenerIngresosFiltro(FiltroIngreso filtro)
        {
            return await _ingresoDataAccess.ObtenerIngresosFiltro(filtro);
        }
    }
}

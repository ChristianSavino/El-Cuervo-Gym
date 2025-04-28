using El_Cuervo_Gym_Web.Core.Ingresos.Domain;
using El_Cuervo_Gym_Web.Core.Ingresos.Domain.Request;

namespace El_Cuervo_Gym_Web.Core.Ingresos.Logic
{
    public interface IIngresoService
    {
        public Task<IEnumerable<IngresoLista>> ObtenerIngresosEnElDia(DateTime fecha);
        public Task<IEnumerable<Ingreso>> ObtenerIngresosEnElDiaSocio(DateTime fecha, int idSocio);
        public Task<IEnumerable<IngresoLista>> ObtenerIngresosFiltro(FiltroIngreso filtro);
        public Task<int> InsertarIngreso(Ingreso ingreso);
        public Task BajaIngreso(int idIngreso);
    }
}

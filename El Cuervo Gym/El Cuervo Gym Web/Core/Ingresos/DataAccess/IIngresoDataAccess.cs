using El_Cuervo_Gym_Web.Core.Ingresos.Domain;

namespace El_Cuervo_Gym_Web.Core.Ingresos.DataAccess
{
    public interface IIngresoDataAccess
    {
        public Task<IEnumerable<IngresoLista>> ObtenerIngresosEnElDia(DateTime fecha);
        public Task<IEnumerable<Ingreso>> ObtenerIngresosEnElDiaSocio(DateTime fecha, int idSocio);
        public Task<int> InsertarIngreso(Ingreso ingreso);
        public Task BajaIngreso(int idIngreso);
    }
}

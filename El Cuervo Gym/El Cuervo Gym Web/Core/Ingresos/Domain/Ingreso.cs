using El_Cuervo_Gym_Web.Core.Utils;

namespace El_Cuervo_Gym_Web.Core.Ingresos.Domain
{
    public class Ingreso
    {
        public int Id { get; set; }
        public int IdSocio { get; set; }
        public DateTime FechaIngreso { get; set; }
        public Estado Estado { get; set; }
    }
}
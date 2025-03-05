using El_Cuervo_Gym_Web.Core.Utils;

namespace El_Cuervo_Gym_Web.Core.Admin.Domain
{
    public class DatosAdmin
    {
        public int Id { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }
        public Estado Estado { get; set; }
        public bool IsMaster { get; set; }
    }
}

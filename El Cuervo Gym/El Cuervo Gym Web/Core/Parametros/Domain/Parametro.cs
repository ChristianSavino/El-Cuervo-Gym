namespace El_Cuervo_Gym_Web.Core.Parametros.Domain
{
    public class Parametro
    {
        public int Id { get; set; }
        public string Clave { get; set; }
        public string Valor { get; set; }
        public string Descripcion { get; set; }

        public DateTime FechaModificacion { get; set; }
    }
}

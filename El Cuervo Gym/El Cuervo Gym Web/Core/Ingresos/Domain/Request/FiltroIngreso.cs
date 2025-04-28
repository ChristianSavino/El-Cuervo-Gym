using El_Cuervo_Gym_Web.Core.Utils;

namespace El_Cuervo_Gym_Web.Core.Ingresos.Domain.Request
{
    public class FiltroIngreso
    {
        public string Nombre { get; set; }
        public int? Documento { get; set; }
        public int? NumeroSocio { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool IncluirDadosDeBaja { get; set; }
        public TipoIngreso? Tipo { get; set; }
        public int? Id { get; set; }
    }
}

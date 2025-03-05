using El_Cuervo_Gym_Web.Core.Utils;

namespace El_Cuervo_Gym_Web.Core.Cobranza.Domain.Request
{
    public class FiltroCobranza
    {
        public string Nombre { get; set; }
        public int? Documento { get; set; }
        public int? NumeroSocio { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool IncluirDadosDeBaja { get; set; }
        public TipoPago? MetodoPago { get; set; }
        public int? Id { get; set; }
        public DateTime? FechaCuota { get; set; }
    }
}

namespace El_Cuervo_Gym_Web.Core.Socio.Domain.Request
{
    public class FiltroSocio
    {
        public string Nombre { get; set; }
        public string Documento { get; set; }
        public string NumeroSocio { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool CuotasVencidas { get; set; }
        public bool IncluirDadosDeBaja { get; set; }
    }
}

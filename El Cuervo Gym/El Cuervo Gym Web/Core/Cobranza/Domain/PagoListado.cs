using El_Cuervo_Gym_Web.Core.Utils;

namespace El_Cuervo_Gym_Web.Core.Cobranza.Domain
{
    public class PagoListado
    {
        public int Id { get; set; }
        public int IdSocio { get; set; }
        public DateTime FechaPago { get; set; }
        public DateTime FechaCuota { get; set; }
        public int Monto { get; set; }
        public Estado Estado { get; set; }
        public int IdAdmin { get; set; }
        public TipoPago MetodoPago { get; set; }
        public string Comprobante { get; set; }
        public string Nombre { get; set; }
        public int Documento { get; set; }
    }
}

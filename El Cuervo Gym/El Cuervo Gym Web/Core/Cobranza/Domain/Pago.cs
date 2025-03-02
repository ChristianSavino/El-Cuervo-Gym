using El_Cuervo_Gym_Web.Core.Utils;

namespace El_Cuervo_Gym_Web.Core.Cobranza.Domain
{
    public class Pago
    {
        public int Id { get; set; }
        public int IdSocio { get; set; }
        public DateTime FechaPago { get; set; }
        public DateTime FechaCuota { get; set; }
        public decimal Monto { get; set; }
        public Estado Estado { get; set; }
        public int IdAdmin { get; set; }
        public TipoPago MetodoPago { get; set; }
        public string Comprobante { get; set; }
    }
}

using El_Cuervo_Gym_Web.Core.Cobranza.Domain;
using El_Cuervo_Gym_Web.Core.Utils;

namespace El_Cuervo_Gym_Web.Core.Socio.Domain
{
    public class DatosSocio
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Documento { get; set; }
        public int Telefono { get; set; }
        public string ObraSocial { get; set; }
        public string NumeroObraSocial { get; set; }
        public int NumeroEmergencia { get; set; }
        public string ContactoEmergencia { get; set; }
        public DateTime FechaSubscripcion { get; set; }
        public DateTime ProximoVencimientoCuota { get; set; }
        public List<Pago> UltimosPagos { get; set; }
        public Estado Estado { get; set; }
        public int IdAdmin { get; set; }
        public string PagosJson { get; set; }
    }
}
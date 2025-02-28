using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace El_Cuervo_Gym_Web.Pages.Admin.Socio
{
    public class DetalleSocioModel : PageModel
    {
        public class Pago
        {
            public DateTime FechaPago { get; set; }
            public decimal Monto { get; set; }
        }

        public class SocioModel
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public string Documento { get; set; }
            public string Telefono { get; set; }
            public string ObraSocial { get; set; }
            public string NumeroObraSocial { get; set; }
            public string NumeroEmergencia { get; set; }
            public string ContactoEmergencia { get; set; }
            public DateTime FechaSubscripcion { get; set; }
            public DateTime ProximoVencimientoCuota { get; set; }
            public string Estado { get; set; }
            public List<Pago> UltimosPagos { get; set; }
        }

        public SocioModel Socio { get; set; }

        public void OnGet(int socioId)
        {
            // Aquí puedes obtener los datos del socio desde una base de datos o cualquier otra fuente de datos
            Socio = new SocioModel
            {
                Id = socioId,
                Nombre = "Juan",
                Apellido = "Pérez",
                Documento = "12345678",
                Telefono = "555-1234",
                ObraSocial = "OSDE",
                NumeroObraSocial = "987654321",
                NumeroEmergencia = "555-5678",
                ContactoEmergencia = "María López",
                FechaSubscripcion = new DateTime(2020, 1, 15),
                ProximoVencimientoCuota = new DateTime(2023, 12, 15),
                Estado = "Activo",
                UltimosPagos = new List<Pago>
                {
                    new Pago { FechaPago = new DateTime(2023, 11, 15), Monto = 1000 },
                    new Pago { FechaPago = new DateTime(2023, 10, 15), Monto = 1000 },
                    new Pago { FechaPago = new DateTime(2023, 9, 15), Monto = 1000 }
                }
            };
        }
    }
}
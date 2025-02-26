using El_Cuervo_Gym_Web.Core.Admin.Domain;
using El_Cuervo_Gym_Web.Core.Cobranza.Domain;
using El_Cuervo_Gym_Web.Core.Utils;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace El_Cuervo_Gym_Web.Pages.Admin
{
    public class DetalleSocioModel : PageModel
    {
        public DatosSocio Socio { get; set; }

        public void OnGet(int socioId)
        {
            if (!Helper.IsSessionAdmin(HttpContext))
            {
                RedirectToPage("/Admin/Login");
            }

            Socio = new DatosSocio
            {
                Nombre = "Christian",
                Apellido = "Savino",
                Documento = 12345678,
                Id = 1,
                Telefono = 46603478,
                ObraSocial = "OSDE",
                NumeroObraSocial = "987654321",
                NumeroEmergencia = 46603479,
                ContactoEmergencia = "Antonella Savino",
                FechaSubscripcion = new DateTime(2020, 1, 15),
                ProximoVencimientoCuota = new DateTime(2023, 12, 15),
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


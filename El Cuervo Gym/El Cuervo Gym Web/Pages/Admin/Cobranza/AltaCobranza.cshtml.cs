using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace El_Cuervo_Gym_Web.Pages.Admin.Cobranza
{
    public class AltaCobranzaModel : PageModel
    {
        public class CobranzaModel
        {
            public string Nombre { get; set; }
            public string Documento { get; set; }
            public string NumeroSocio { get; set; }
            [DataType(DataType.Date)]
            public DateTime FechaPago { get; set; }
            public decimal Monto { get; set; }
        }

        [BindProperty]
        public CobranzaModel Cobranza { get; set; }

        public void OnGet(int socioId)
        {
            // Aquí puedes obtener los datos del socio y el valor de la cuota desde una base de datos o cualquier otra fuente de datos
            Cobranza = new CobranzaModel
            {
                Nombre = "Juan Pérez",
                Documento = "12345678",
                NumeroSocio = "12345",
                FechaPago = DateTime.Now,
                Monto = 1000 // Valor de la cuota
            };
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Aquí puedes agregar la lógica para registrar la nueva cobranza en la base de datos

            // Calcular la fecha de la siguiente cuota (por ejemplo, un mes después de la fecha de pago actual)
            var siguienteCuota = Cobranza.FechaPago.AddMonths(1);

            // Redirigir a la página de confirmación de cobranza
            return RedirectToPage("/Admin/Cobranza/Responses/CobranzaCorrecta", new { siguienteCuota = siguienteCuota.ToString("yyyy-MM-dd") });
        }
    }
}





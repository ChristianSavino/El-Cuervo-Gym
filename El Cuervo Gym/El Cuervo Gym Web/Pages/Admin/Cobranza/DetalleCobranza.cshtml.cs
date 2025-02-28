using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace El_Cuervo_Gym_Web.Pages.Admin.Cobranza
{
    public class DetalleCobranzaModel : PageModel
    {
        public class CobranzaModel
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public string Documento { get; set; }
            public string NumeroSocio { get; set; }
            public DateTime FechaPago { get; set; }
            public decimal Monto { get; set; }
        }

        public CobranzaModel Cobranza { get; set; }

        public void OnGet(int cobranzaId)
        {
            // Aquí puedes obtener los datos de la cobranza desde una base de datos o cualquier otra fuente de datos
            Cobranza = new CobranzaModel
            {
                Id = cobranzaId,
                Nombre = "Juan Pérez",
                Documento = "12345678",
                NumeroSocio = "12345",
                FechaPago = new DateTime(2023, 11, 15),
                Monto = 1000
            };
        }
    }
}



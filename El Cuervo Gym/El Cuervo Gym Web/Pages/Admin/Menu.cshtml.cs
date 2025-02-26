using El_Cuervo_Gym_Web.Core.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace El_Cuervo_Gym_Web.Pages.Admin
{
    public class MenuModel : PageModel
    {
        public class SocioIngreso
        {
            public string NombreCompleto { get; set; }
            public string NumeroSocio { get; set; }
            public string HorarioIngreso { get; set; }
            public int Id { get; set; }
        }

        public List<SocioIngreso> Socios { get; set; }

        public IActionResult OnGet()
        {
            if (!Helper.IsSessionAdmin(HttpContext))
            {
                return RedirectToPage("/Admin/Login");
            }

            Socios = new List<SocioIngreso>
            {
                new SocioIngreso { NombreCompleto = "Juan Pérez", NumeroSocio = "12345", HorarioIngreso = "08:00 AM", Id = 1 },
                new SocioIngreso { NombreCompleto = "María López", NumeroSocio = "67890", HorarioIngreso = "09:30 AM", Id = 2 },
                new SocioIngreso { NombreCompleto = "Kerubin", NumeroSocio = "181197", HorarioIngreso = "08:43 AM", Id = 3 }
            };

            return Page();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace El_Cuervo_Gym_Web.Pages.Admin.Socio
{
    public class DarDeBajaSocioModel : PageModel
    {
        public IActionResult OnGet(int socioId)
        {
            // Aquí puedes agregar la lógica para dar de baja al socio en la base de datos

            // Redirigir a la página de confirmación de baja
            return Page();
        }
    }
}
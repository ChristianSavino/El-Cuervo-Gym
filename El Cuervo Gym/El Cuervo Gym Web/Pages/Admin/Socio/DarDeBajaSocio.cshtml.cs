using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace El_Cuervo_Gym_Web.Pages.Admin.Socio
{
    public class DarDeBajaSocioModel : PageModel
    {
        public IActionResult OnGet(int socioId)
        {
            // Aqu� puedes agregar la l�gica para dar de baja al socio en la base de datos

            // Redirigir a la p�gina de confirmaci�n de baja
            return Page();
        }
    }
}
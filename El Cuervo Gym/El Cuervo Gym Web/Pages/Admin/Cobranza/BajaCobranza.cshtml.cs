using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace El_Cuervo_Gym_Web.Pages.Admin.Cobranza
{
    public class BajaCobranzaModel : PageModel
    {
        public IActionResult OnGet(int cobranzaId)
        {
            // Aqu� puedes agregar la l�gica para dar de baja la cobranza en la base de datos

            // Redirigir a la p�gina de confirmaci�n de baja
            return Page();
        }
    }
}

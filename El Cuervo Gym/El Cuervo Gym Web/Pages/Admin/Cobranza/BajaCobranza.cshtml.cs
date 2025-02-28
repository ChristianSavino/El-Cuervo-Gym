using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace El_Cuervo_Gym_Web.Pages.Admin.Cobranza
{
    public class BajaCobranzaModel : PageModel
    {
        public IActionResult OnGet(int cobranzaId)
        {
            // Aquí puedes agregar la lógica para dar de baja la cobranza en la base de datos

            // Redirigir a la página de confirmación de baja
            return Page();
        }
    }
}

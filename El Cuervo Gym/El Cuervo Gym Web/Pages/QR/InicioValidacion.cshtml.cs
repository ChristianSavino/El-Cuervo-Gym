using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace El_Cuervo_Gym_Web.Pages.QR
{
    public class InicioValidacionModel : PageModel
    {
        public IActionResult OnGet()
        {
            return RedirectToPage("/Socio/Login?qr=True");
        }
    }
}

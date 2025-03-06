using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace El_Cuervo_Gym_Web.Pages.Socio
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            HttpContext.Session.Remove("Socio");
            HttpContext.Session.Remove("NombreSocio");
            return RedirectToPage("/Socio/Login");
        }
    }
}

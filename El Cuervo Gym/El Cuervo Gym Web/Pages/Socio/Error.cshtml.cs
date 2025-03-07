using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace El_Cuervo_Gym_Web.Pages.Socio
{
    public class ErrorModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Accion { get; set; }

        [BindProperty(SupportsGet = true)]
        public string MensajeError { get; set; }

        public void OnGet(string accion, string mensajeError)
        {
            Accion = accion;
            MensajeError = mensajeError;
        }
    }
}

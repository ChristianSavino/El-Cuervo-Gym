using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace El_Cuervo_Gym_Web.Pages.Admin.Cobranza.Responses
{
    public class CobranzaCorrectaModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public DateTime SiguienteCuota { get; set; }

        public void OnGet(DateTime siguienteCuota)
        {
            SiguienteCuota = siguienteCuota;
        }
    }
}
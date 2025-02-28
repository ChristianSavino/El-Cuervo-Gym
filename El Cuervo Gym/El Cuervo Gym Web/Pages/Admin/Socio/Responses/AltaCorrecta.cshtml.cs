using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace El_Cuervo_Gym_Web.Pages.Admin.Socio.Responses
{
    public class AltaCorrectaModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string NombreCompleto { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Documento { get; set; }

        [BindProperty(SupportsGet = true)]
        public int NumeroSocio { get; set; }

        public void OnGet(string nombreCompleto, int documento, int numeroSocio)
        {
            NombreCompleto = nombreCompleto;
            Documento = documento;
            NumeroSocio = numeroSocio;
        }
    }
}
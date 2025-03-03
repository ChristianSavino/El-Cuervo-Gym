using El_Cuervo_Gym_Web.Core.Socio.Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace El_Cuervo_Gym_Web.Pages.Admin.Socio
{
    public class DarDeBajaSocioModel : PageModel
    {
        private ISocioService _socioService;

        public DarDeBajaSocioModel(ISocioService socioService)
        {
            _socioService = socioService;
        }

        public async Task<IActionResult> OnGet(int socioId)
        {
            var result = await _socioService.DarDeBajaSocio(socioId);

            return Page();
        }
    }
}
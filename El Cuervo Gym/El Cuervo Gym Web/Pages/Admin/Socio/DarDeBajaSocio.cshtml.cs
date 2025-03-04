using El_Cuervo_Gym_Web.Core.Socio.Logic;
using El_Cuervo_Gym_Web.Core.Utils.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace El_Cuervo_Gym_Web.Pages.Admin.Socio
{
    public class DarDeBajaSocioModel : PageModel
    {
        private ISocioService _socioService;
        private ICLogger _logger;

        public DarDeBajaSocioModel(ISocioService socioService, ICLogger logger)
        {
            _socioService = socioService;
            _logger = logger;
        }

        public async Task<IActionResult> OnGet(int socioId)
        {
            try
            {
                await _socioService.DarDeBajaSocio(socioId);
            }
            catch (Exception ex)
            {
                var contexto = "Baja Socio";
                RedirectToPage(await _logger.LogError(ex, contexto, string.Empty), new { accion = contexto, mensajeError = ex.Message });
            }

            return Page();
        }
    }
}
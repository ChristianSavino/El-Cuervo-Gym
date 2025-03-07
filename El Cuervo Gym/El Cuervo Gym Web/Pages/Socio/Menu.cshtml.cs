using El_Cuervo_Gym_Web.Core.Socio.Domain;
using El_Cuervo_Gym_Web.Core.Socio.Logic;
using El_Cuervo_Gym_Web.Core.Utils.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace El_Cuervo_Gym_Web.Pages.Socio
{
    public class MenuModel : PageModel
    {
        private readonly ISocioService _socioService;
        private readonly ICLogger _logger;

        public MenuModel(ISocioService socioService, ICLogger logger)
        {
            _socioService = socioService;
            _logger = logger;
        }

        public DatosSocio Socio { get; set; }

        public async Task<IActionResult> OnGet()
        {
            try
            {
                var socioId = int.Parse(HttpContext.Session.GetString("Socio"));
                Socio = await _socioService.ObtenerSocioConPagosPorId(socioId);
            }
            catch (Exception ex)
            {
                var contexto = "Detalle Membresía";
                return RedirectToPage(await _logger.LogErrorSocio(ex, contexto, string.Empty), new { accion = contexto, mensajeError = ex.Message });
            }

            return Page();
        }
    }
}

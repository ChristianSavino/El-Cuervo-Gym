using El_Cuervo_Gym_Web.Core.Ingresos.Logic;
using El_Cuervo_Gym_Web.Core.Utils.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace El_Cuervo_Gym_Web.Pages.Admin.Ingreso
{
    public class BajaIngresoModel : PageModel
    {
        private IIngresoService _ingresoService;
        private ICLogger _logger;

        public BajaIngresoModel(IIngresoService ingresoService, ICLogger logger)
        {
            _ingresoService = ingresoService;
            _logger = logger;
        }

        public async Task<IActionResult> OnGet(int ingresoId)
        {
			try
			{
                await _ingresoService.BajaIngreso(ingresoId);
            }
			catch (Exception ex)
			{
                var contexto = "Baja Ingreso";
                return RedirectToPage(await _logger.LogError(ex, contexto, string.Empty), new { accion = contexto, mensajeError = ex.Message });
            }

			return Page();
        }
    }
}

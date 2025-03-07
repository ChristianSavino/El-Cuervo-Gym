using El_Cuervo_Gym_Web.Core.Ingresos.Domain;
using El_Cuervo_Gym_Web.Core.Ingresos.Logic;
using El_Cuervo_Gym_Web.Core.Utils.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace El_Cuervo_Gym_Web.Pages.Admin
{
    public class MenuModel : PageModel
    {
        private readonly ICLogger _logger;
        private readonly IIngresoService _ingresoService;

        public MenuModel(ICLogger logger, IIngresoService ingresoService)
        {
            _logger = logger;
            _ingresoService = ingresoService;
        }

        public IEnumerable<IngresoLista> Ingresos { get; set; }

        public async Task<IActionResult> OnGet()
        {
            try
            {
                Ingresos = await _ingresoService.ObtenerIngresosEnElDia(DateTime.Now);
            }
            catch (Exception ex)
            {
                var contexto = "Carga Lista Ingresos Menu";
                return RedirectToPage(await _logger.LogError(ex, contexto, string.Empty), new { accion = contexto, mensajeError = ex.Message });
            }

            return Page();
        }
    }
}

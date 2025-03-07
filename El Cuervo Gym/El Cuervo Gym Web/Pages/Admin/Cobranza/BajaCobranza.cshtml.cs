using El_Cuervo_Gym_Web.Core.Admin.Logic;
using El_Cuervo_Gym_Web.Core.Utils.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace El_Cuervo_Gym_Web.Pages.Admin.Cobranza
{
    public class BajaCobranzaModel : PageModel
    {
        private ICLogger _logger;
        private IAdminService _adminService;

        public BajaCobranzaModel(ICLogger logger, IAdminService adminService)
        {
            _logger = logger;
            _adminService = adminService;
        }

        public async Task<IActionResult> OnGet(int cobranzaId)
        {
            try
            {
                 await _adminService.DarDeBajaComprobante(cobranzaId);
            }
            catch (Exception ex)
            {
                var contexto = "Baja de Cobranza";
                return RedirectToPage(await _logger.LogError(ex, contexto, string.Empty), new { accion = contexto, mensajeError = ex.Message });
            }

            return Page();
        }
    }
}

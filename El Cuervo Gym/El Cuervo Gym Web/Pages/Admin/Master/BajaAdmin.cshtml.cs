using El_Cuervo_Gym_Web.Core.Admin.Logic;
using El_Cuervo_Gym_Web.Core.Utils.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace El_Cuervo_Gym_Web.Pages.Admin.Master
{
    public class BajaAdminModel : PageModel
    {
        private readonly IAdminService _adminService;
        private readonly ICLogger _logger;

        public BajaAdminModel(IAdminService adminService, ICLogger logger)
        {
            _adminService = adminService;
            _logger = logger;
        }

        public async Task<IActionResult> OnGet(int adminId)
        {
            try
            {
                await _adminService.DarDeBajaAdmin(adminId);
            }
            catch (Exception ex)
            {
                var contexto = "Baja de Admin";
                return RedirectToPage(await _logger.LogError(ex, contexto, string.Empty), new { accion = contexto, mensajeError = ex.Message });
            }

            return Page();
        }
    }
}

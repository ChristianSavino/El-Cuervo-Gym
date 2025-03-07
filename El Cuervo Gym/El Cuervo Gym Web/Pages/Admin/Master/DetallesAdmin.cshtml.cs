using El_Cuervo_Gym_Web.Core.Admin.Domain;
using El_Cuervo_Gym_Web.Core.Admin.Logic;
using El_Cuervo_Gym_Web.Core.Utils;
using El_Cuervo_Gym_Web.Core.Utils.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace El_Cuervo_Gym_Web.Pages.Admin.Master
{
    public class DetallesAdminModel : PageModel
    {
        private readonly ICLogger _logger;
        private readonly IAdminService _adminService;

        public DetallesAdminModel(ICLogger logger, IAdminService adminService)
        {
            _logger = logger;
            _adminService = adminService;
        }

        public DatosAdmin Admin { get; set; }
        public bool DadoBaja { get; set; }

        public async Task<IActionResult> OnGet(int adminId)
        {
            try
            {
                Admin = await _adminService.ObtenerAdminPorId(adminId);
                DadoBaja = Admin.Estado == Estado.Baja;
            }
            catch (Exception ex)
            {
                var contexto = "Detalle Admin";
                return RedirectToPage(await _logger.LogError(ex, contexto, string.Empty), new { accion = contexto, mensajeError = ex.Message });
            }

            return Page();
        }
    }
}

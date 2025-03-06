using El_Cuervo_Gym_Web.Core.Admin.Domain;
using El_Cuervo_Gym_Web.Core.Admin.Logic;
using El_Cuervo_Gym_Web.Core.Utils;
using El_Cuervo_Gym_Web.Core.Utils.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace El_Cuervo_Gym_Web.Pages.Admin.Master
{
    public class ModificarAdminModel : PageModel
    {
        private readonly ICLogger _logger;
        private readonly IAdminService _adminService;

        public ModificarAdminModel(ICLogger logger, IAdminService adminService)
        {
            _logger = logger;
            _adminService = adminService;
        }

        public class AdminModel
        {
            public int Id { get; set; }
            [Required(ErrorMessage = "El campo Usuario es obligatorio.")]
            public string Usuario { get; set; }
            [Required(ErrorMessage = "El campo Contraseña es obligatorio.")]
            public string Password { get; set; }
            public bool IsMaster { get; set; }
            public Estado Estado { get; set; }
        }

        [BindProperty]
        public AdminModel Admin { get; set; }
        public bool OperacionExitosa { get; set; }
        public string ErrorMessage { get; set; }

        public async Task OnGet(int adminId)
        {
            try
            {
                var admin = await _adminService.ObtenerAdminPorId(adminId);
                Admin = new AdminModel
                {
                    Id = admin.Id,
                    Usuario = admin.Usuario,
                    Password = admin.Password,
                    IsMaster = admin.IsMaster,
                    Estado = admin.Estado
                };
            }
            catch (Exception ex)
            {
                var contexto = "Modificar Admin";
                RedirectToPage(await _logger.LogError(ex, contexto, string.Empty), new { accion = contexto, mensajeError = ex.Message });
            }
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                var admin = new DatosAdmin()
                {
                    Id = Admin.Id,
                    Usuario = Admin.Usuario,
                    Password = Admin.Password,
                    IsMaster = Admin.IsMaster,
                    Estado = Admin.Estado
                };
                
                await _adminService.ActualizarAdmin(admin);
            }
            catch (Exception ex)
            {
                var contexto = "Modificar Admin";
                RedirectToPage(await _logger.LogError(ex, contexto, string.Empty), new { accion = contexto, mensajeError = ex.Message });
            }

            OperacionExitosa = true;
            return Page();
        }
    }
}

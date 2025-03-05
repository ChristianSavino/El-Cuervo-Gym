using El_Cuervo_Gym_Web.Core.Admin.Domain;
using El_Cuervo_Gym_Web.Core.Admin.Logic;
using El_Cuervo_Gym_Web.Core.Utils.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace El_Cuervo_Gym_Web.Pages.Admin.Master
{
    public class AltaAdminModel : PageModel
    {
        private ICLogger _logger;
        private IAdminService _adminService;

        public AltaAdminModel(ICLogger logger, IAdminService adminService)
        {
            _logger = logger;
            _adminService = adminService;
        }

        public class AltaAdmin
        {
            [Required(ErrorMessage = "El campo Usuario es obligatorio.")]
            public string Usuario { get; set; }

            [Required(ErrorMessage = "El campo Contraseña es obligatorio.")]
            public string Password { get; set; }

            [Required(ErrorMessage = "Es Usuario Maestro?")]
            public bool IsMaster { get; set; }      
        }

        [BindProperty]
        public AltaAdmin NuevoAdmin { get; set; }
        public string ErrorMessage { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }
            
            try
            {
                var admin = new DatosAdmin
                {
                    Usuario = NuevoAdmin.Usuario,
                    Password = NuevoAdmin.Password,
                    IsMaster = NuevoAdmin.IsMaster
                };

                //await _adminService.InsertarAdmin(admin);
                return RedirectToPage("/Admin/Master/ListarAdmin");
            }
            catch (Exception ex)
            {
                var contexto = "Alta Admin";
                return RedirectToPage(await _logger.LogError(ex, contexto, string.Empty), new { accion = contexto, mensajeError = ex.Message });
            }
        }
    }
}

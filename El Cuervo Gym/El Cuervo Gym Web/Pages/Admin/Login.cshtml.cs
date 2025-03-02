using El_Cuervo_Gym_Web.Core.Admin.Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace El_Cuervo_Gym_Web.Pages.Admin
{
    public class LoginAdminModel : PageModel
    {
        private readonly IAdminService _adminService;
        public LoginAdminModel(IAdminService adminService)
        {
            _adminService = adminService;
        }


        [BindProperty]
        public AdminInputModel Input { get; set; }

        public string ErrorMessage { get; set; }

        public class AdminInputModel
        {
            [Required(ErrorMessage = "El campo Usuario es obligatorio.")]
            public string Username { get; set; }

            [Required(ErrorMessage = "El campo Contraseña es obligatorio.")]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var admin = await _adminService.ObtenerAdmin(Input.Username, Input.Password);
                if (admin != null)
                {
                    var adminJson = JsonSerializer.Serialize(admin);
                    HttpContext.Session.SetString("NombreUsuario",admin.Usuario);
                    HttpContext.Session.SetString("Admin", adminJson);
                    return RedirectToPage("/Admin/Menu");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }

            return Page();
        }
    }
}


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace El_Cuervo_Gym_Web.Pages.Admin
{
    public class LoginAdminModel : PageModel
    {
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

            if (Input.Username == "admin" || Input.Password == "1234")
            {
                HttpContext.Session.SetString("Admin", Input.Username);
                return RedirectToPage("/Admin/Menu");
            }

            ErrorMessage = "Usuario o contraseña incorrecta.";
            return Page();
        }
    }
}


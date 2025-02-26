using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace El_Cuervo_Gym_Web.Pages.User
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; }

        [FromQuery(Name = "qr")]
        public bool IsQr { get; set; }

        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "El campo Documento es obligatorio.")]
            public string ID { get; set; }

            [Required(ErrorMessage = "El campo N° Socio es obligatorio.")]
            public string Socio { get; set; }
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Input.ID != "1" || Input.Socio != "1234")
            {
                ErrorMessage = "Usuario o contraseña incorrecta.";
                return Page();
            }

            if (IsQr)
            {

            }
            
            return RedirectToPage("/Index");
        }
    }
}


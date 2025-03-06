using El_Cuervo_Gym_Web.Core.Socio.Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace El_Cuervo_Gym_Web.Pages.User
{
    public class LoginModel : PageModel
    {
        private readonly ISocioService _socioService;

        public LoginModel(ISocioService socioService)
        {
            _socioService = socioService;
        }

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

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var socio = await _socioService.LogearSocio(int.Parse(Input.ID), int.Parse(Input.Socio));
                if(socio != null)
                {
                    HttpContext.Session.SetString("NombreSocio", socio.Nombre);
                    HttpContext.Session.SetString("Socio", socio.Id.ToString());
                }

                if (IsQr)
                {
                    return RedirectToPage("/Socio/ValidacionQr");
                }
                else
                {
                    return RedirectToPage("/Socio/Menu");
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


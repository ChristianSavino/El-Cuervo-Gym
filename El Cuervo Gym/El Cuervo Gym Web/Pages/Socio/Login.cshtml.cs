using El_Cuervo_Gym_Web.Core.Socio.Logic;
using El_Cuervo_Gym_Web.Core.WhatsApp.Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace El_Cuervo_Gym_Web.Pages.User
{
    public class LoginModel : PageModel
    {
        private readonly ISocioService _socioService;
        private readonly IWhatsAppService _whatsAppService;

        public LoginModel(ISocioService socioService, IWhatsAppService whatsAppService)
        {
            _socioService = socioService;
            _whatsAppService = whatsAppService;
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

        public async Task<IActionResult> OnPostEnviarWhatsappAsync(int documentoRecuperar)
        {
            try
            {
                var socio = await _socioService.ObtenerSocioPorIdDocumento(documentoRecuperar);
                if (socio == null)
                {
                    TempData["ErrorWhatsapp"] = "No se encontró un socio con ese documento.";
                    return RedirectToPage();
                }

                await _whatsAppService.EnviarMensajeAsync(socio.Telefono.ToString(), socio);

                TempData["MensajeWhatsapp"] = "Te enviamos tu número de socio por WhatsApp.";
            }
            catch (Exception ex)
            {
                TempData["ErrorWhatsapp"] = ex.Message;
            }

            return RedirectToPage();
        }
    }
}


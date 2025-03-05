using El_Cuervo_Gym_Web.Core.Admin.Domain;
using El_Cuervo_Gym_Web.Core.Admin.Logic;
using El_Cuervo_Gym_Web.Core.Socio.Domain;
using El_Cuervo_Gym_Web.Core.Socio.Logic;
using El_Cuervo_Gym_Web.Core.Utils;
using El_Cuervo_Gym_Web.Core.Utils.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace El_Cuervo_Gym_Web.Pages.Admin.Socio
{
    public class ModificarSocioModel : PageModel
    {
        private readonly IAdminService _adminService;
        private readonly ISocioService _socioService;
        private readonly ICLogger _logger;

        public ModificarSocioModel(IAdminService adminService, ISocioService socioService, ICLogger logger)
        {
            _socioService = socioService;
            _adminService = adminService;
            _logger = logger;
        }   

        public class SocioModel
        {
            public int Id { get; set; }
            
            [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
            public string Nombre { get; set; }

            [Required(ErrorMessage = "El campo Apellido es obligatorio.")]
            public string Apellido { get; set; }

            [Required(ErrorMessage = "El campo Documento es obligatorio.")]
            public int Documento { get; set; }

            [Required(ErrorMessage = "El campo Teléfono es obligatorio.")]
            public int Telefono { get; set; }

            [Required(ErrorMessage = "El campo Obra Social es obligatorio.")]
            public string ObraSocial { get; set; }

            [Required(ErrorMessage = "El campo Número de Obra Social es obligatorio.")]
            public string NumeroObraSocial { get; set; }

            [Required(ErrorMessage = "El campo Número de Emergencia es obligatorio.")]
            public int NumeroEmergencia { get; set; }

            [Required(ErrorMessage = "El campo Contacto de Emergencia es obligatorio.")]
            public string ContactoEmergencia { get; set; }

            [Required(ErrorMessage = "El campo Fecha de Subscripción es obligatorio.")]
            [DataType(DataType.Date)]
            public DateTime FechaSubscripcion { get; set; }

            [Required(ErrorMessage = "El campo Estado es obligatorio.")]
            public Estado Estado { get; set; }
        }

        [BindProperty]
        public SocioModel Socio { get; set; }

        public bool OperacionExitosa { get; set; }

        public async Task OnGet(int socioId)
        {
            try
            {
                var socio = await _socioService.ObtenerSocioPorId(socioId);

                Socio = new SocioModel
                {
                    Id = socio.Id,
                    Nombre = socio.Nombre,
                    Apellido = socio.Apellido,
                    Documento = socio.Documento,
                    Telefono = socio.Telefono,
                    ObraSocial = socio.ObraSocial,
                    NumeroObraSocial = socio.NumeroObraSocial,
                    NumeroEmergencia = socio.NumeroEmergencia,
                    ContactoEmergencia = socio.ContactoEmergencia,
                    FechaSubscripcion = socio.FechaSubscripcion,
                    Estado = socio.Estado
                };
            }
            catch (Exception ex)
            {
                var contexto = "Modificar Socio";
                RedirectToPage(await _logger.LogError(ex, contexto, string.Empty), new { accion = contexto, mensajeError = ex.Message });
            }
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var admin = JsonConvert.DeserializeObject<DatosAdminLogin>(HttpContext.Session.GetString("Admin"));

                var socio = new DatosSocio
                {
                    Id = Socio.Id,
                    Nombre = Socio.Nombre,
                    Apellido = Socio.Apellido,
                    Documento = Socio.Documento,
                    Telefono = Socio.Telefono,
                    ObraSocial = Socio.ObraSocial,
                    NumeroObraSocial = Socio.NumeroObraSocial,
                    NumeroEmergencia = Socio.NumeroEmergencia,
                    ContactoEmergencia = Socio.ContactoEmergencia,
                    FechaSubscripcion = Socio.FechaSubscripcion,
                    Estado = Socio.Estado,
                    IdAdmin = admin.Id
                };

                OperacionExitosa = await _adminService.ActualizarSocio(socio);
            }
            catch (Exception ex)
            {
                var contexto = "Modificar Socio";
                RedirectToPage(await _logger.LogError(ex, contexto, string.Empty), new { accion = contexto, mensajeError = ex.Message });
            }

            return Page();
        }
    }
}
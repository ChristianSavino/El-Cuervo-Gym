using El_Cuervo_Gym_Web.Core.Admin.Domain;
using El_Cuervo_Gym_Web.Core.Admin.Logic;
using El_Cuervo_Gym_Web.Core.Socio.Domain;
using El_Cuervo_Gym_Web.Core.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace El_Cuervo_Gym_Web.Pages.Admin.Socio
{
    public class AltaSocioModel : PageModel
    {
        private readonly IAdminService _adminService;

        public AltaSocioModel(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public class AltaSocio
        {
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
        }

        [BindProperty]
        public AltaSocio NuevoSocio { get; set; }

        public void OnGet()
        {
            NuevoSocio = new AltaSocio
            {
                FechaSubscripcion = DateTime.Now
            };
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

                var datosSocio = new DatosSocio
                {
                    Nombre = NuevoSocio.Nombre,
                    Apellido = NuevoSocio.Apellido,
                    Documento = NuevoSocio.Documento,
                    Telefono = NuevoSocio.Telefono,
                    ObraSocial = NuevoSocio.ObraSocial,
                    NumeroObraSocial = NuevoSocio.NumeroObraSocial,
                    NumeroEmergencia = NuevoSocio.NumeroEmergencia,
                    ContactoEmergencia = NuevoSocio.ContactoEmergencia,
                    FechaSubscripcion = NuevoSocio.FechaSubscripcion,
                    Estado = Estado.Activo,
                    IdAdmin = admin.Id
                };

                var socio = await _adminService.InsertarSocio(datosSocio);

                return RedirectToPage("/Admin/Socio/Responses/AltaCorrecta", new { nombreCompleto = $"{socio.Nombre} {socio.Apellido}", documento = socio.Documento, numeroSocio = socio.Id });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
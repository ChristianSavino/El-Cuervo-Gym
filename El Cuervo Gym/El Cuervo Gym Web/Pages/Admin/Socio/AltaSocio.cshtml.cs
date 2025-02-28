using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace El_Cuervo_Gym_Web.Pages.Admin.Socio
{
    public class AltaSocioModel : PageModel
    {
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

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Aquí puedes agregar la lógica para generar el número de socio y guardar el nuevo socio en la base de datos
            // Ejemplo de generación de número de socio
            var numeroSocioGenerado = 1;

            // Guardar el nuevo socio en la base de datos (lógica no implementada)

            // Redirigir a la página de confirmación
            return RedirectToPage("/Admin/Socio/Responses/AltaCorrecta", new { nombreCompleto = $"{NuevoSocio.Nombre} {NuevoSocio.Apellido}", documento = NuevoSocio.Documento, numeroSocio = numeroSocioGenerado });
        }
    }
}





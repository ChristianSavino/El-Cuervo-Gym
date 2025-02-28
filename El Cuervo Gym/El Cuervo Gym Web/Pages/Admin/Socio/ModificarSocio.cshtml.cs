using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace El_Cuervo_Gym_Web.Pages.Admin.Socio
{
    public class ModificarSocioModel : PageModel
    {
        public class SocioModel
        {
            [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
            public string Nombre { get; set; }

            [Required(ErrorMessage = "El campo Apellido es obligatorio.")]
            public string Apellido { get; set; }

            [Required(ErrorMessage = "El campo Documento es obligatorio.")]
            public int Documento { get; set; }

            [Required(ErrorMessage = "El campo Tel�fono es obligatorio.")]
            public int Telefono { get; set; }

            [Required(ErrorMessage = "El campo Obra Social es obligatorio.")]
            public string ObraSocial { get; set; }

            [Required(ErrorMessage = "El campo N�mero de Obra Social es obligatorio.")]
            public string NumeroObraSocial { get; set; }

            [Required(ErrorMessage = "El campo N�mero de Emergencia es obligatorio.")]
            public int NumeroEmergencia { get; set; }

            [Required(ErrorMessage = "El campo Contacto de Emergencia es obligatorio.")]
            public string ContactoEmergencia { get; set; }

            [Required(ErrorMessage = "El campo Fecha de Subscripci�n es obligatorio.")]
            [DataType(DataType.Date)]
            public DateTime FechaSubscripcion { get; set; }

            [Required(ErrorMessage = "El campo Estado es obligatorio.")]
            public string Estado { get; set; }
        }

        [BindProperty]
        public SocioModel Socio { get; set; }

        public bool OperacionExitosa { get; set; }

        public void OnGet(int socioId)
        {
            // Aqu� puedes obtener los datos del socio desde una base de datos o cualquier otra fuente de datos
            // Ejemplo de datos de socio
            Socio = new SocioModel
            {
                Nombre = "Juan",
                Apellido = "P�rez",
                Documento = 12345678,
                Telefono = 5551234,
                ObraSocial = "OSDE",
                NumeroObraSocial = "987654321",
                NumeroEmergencia = 5555678,
                ContactoEmergencia = "Mar�a L�pez",
                FechaSubscripcion = new DateTime(2020, 1, 15),
                Estado = "Activo"
            };
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Aqu� puedes agregar la l�gica para guardar los cambios del socio en la base de datos

            OperacionExitosa = true;
            return Page();
        }
    }
}
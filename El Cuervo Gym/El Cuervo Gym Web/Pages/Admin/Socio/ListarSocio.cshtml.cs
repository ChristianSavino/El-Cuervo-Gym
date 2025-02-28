using El_Cuervo_Gym_Web.Core.Admin.Domain;
using El_Cuervo_Gym_Web.Core.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace El_Cuervo_Gym_Web.Pages.Admin.Socio
{
    public class ListarSocioModel : PageModel
    {
        public class FiltroModel
        {
            public string Nombre { get; set; }
            public string Documento { get; set; }
            public string NumeroSocio { get; set; }
            public DateTime? FechaInicio { get; set; }
            public DateTime? FechaFin { get; set; }
            public bool CuotasVencidas { get; set; }
            public bool IncluirDadosDeBaja { get; set; }
        }

        [BindProperty(SupportsGet = true)]
        public FiltroModel Filtro { get; set; }
        public List<DatosSocio> SociosAtrasados { get; set; }

        public void OnGet()
        {
            if (!Helper.IsSessionAdmin(HttpContext))
            {
                RedirectToPage("/Admin/Login");
            }

            var todosLosSocios = new List<DatosSocio>
            {
                new DatosSocio { Id = 1, Nombre = "Juan Pérez", Documento = 12345678, FechaSubscripcion = new DateTime(2020, 1, 15), ProximoVencimientoCuota = new DateTime(2023, 11, 15) },
                new DatosSocio { Id = 2, Nombre = "María López", Documento = 87654321, FechaSubscripcion = new DateTime(2021, 5, 20), ProximoVencimientoCuota = new DateTime(2023, 10, 15) },
                new DatosSocio { Id = 3, Nombre = "Carlos García", Documento = 11223344, FechaSubscripcion = new DateTime(2019, 3, 10), ProximoVencimientoCuota = new DateTime(2023, 9, 15) }
            };

            // Filtrar socios atrasados
            SociosAtrasados = todosLosSocios
                .Where(s => !Filtro.CuotasVencidas || s.ProximoVencimientoCuota < DateTime.Now)
                .Where(s => !Filtro.IncluirDadosDeBaja || s.Estado != Estado.Baja)
                .Where(s => string.IsNullOrEmpty(Filtro.Nombre) || s.Nombre.Contains(Filtro.Nombre, StringComparison.OrdinalIgnoreCase))
                .Where(s => string.IsNullOrEmpty(Filtro.Documento) || s.Documento.ToString() == Filtro.Documento)
                .Where(s => string.IsNullOrEmpty(Filtro.NumeroSocio) || s.Id.ToString() == Filtro.NumeroSocio)
                .Where(s => !Filtro.FechaInicio.HasValue || s.FechaSubscripcion >= Filtro.FechaInicio.Value)
                .Where(s => !Filtro.FechaFin.HasValue || s.FechaSubscripcion <= Filtro.FechaFin.Value)
                .ToList();
        }
    }
}

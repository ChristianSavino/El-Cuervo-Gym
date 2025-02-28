using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace El_Cuervo_Gym_Web.Pages.Admin.Cobranza
{
    public class ListarCobranzasModel : PageModel
    {
        public class FiltroModel
        {
            public string Nombre { get; set; }
            public string Documento { get; set; }
            public string NumeroSocio { get; set; }
            public DateTime? FechaInicio { get; set; }
            public DateTime? FechaFin { get; set; }
            public bool IncluirDadosDeBaja { get; set; }
        }

        public class CobranzaModel
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public string Documento { get; set; }
            public string NumeroSocio { get; set; }
            public DateTime FechaPago { get; set; }
            public decimal Monto { get; set; }
            public bool DadoDeBaja { get; set; }
        }

        [BindProperty(SupportsGet = true)]
        public FiltroModel Filtro { get; set; }
        public List<CobranzaModel> Cobranzas { get; set; }

        public void OnGet()
        {
            // Aquí puedes obtener los datos de las cobranzas desde una base de datos o cualquier otra fuente de datos
            var todasLasCobranzas = new List<CobranzaModel>
            {
                new CobranzaModel { Id = 1, Nombre = "Juan Pérez", Documento = "12345678", NumeroSocio = "12345", FechaPago = new DateTime(2023, 11, 15), Monto = 1000, DadoDeBaja = false },
                new CobranzaModel { Id = 2, Nombre = "María López", Documento = "87654321", NumeroSocio = "67890", FechaPago = new DateTime(2023, 10, 15), Monto = 1000, DadoDeBaja = false },
                new CobranzaModel { Id = 3, Nombre = "Carlos García", Documento = "11223344", NumeroSocio = "11223", FechaPago = new DateTime(2023, 9, 15), Monto = 1000, DadoDeBaja = true }
            };

            // Filtrar cobranzas
            Cobranzas = todasLasCobranzas
                .Where(c => string.IsNullOrEmpty(Filtro.Nombre) || c.Nombre.Contains(Filtro.Nombre, StringComparison.OrdinalIgnoreCase))
                .Where(c => string.IsNullOrEmpty(Filtro.Documento) || c.Documento.Contains(Filtro.Documento, StringComparison.OrdinalIgnoreCase))
                .Where(c => string.IsNullOrEmpty(Filtro.NumeroSocio) || c.NumeroSocio.Contains(Filtro.NumeroSocio, StringComparison.OrdinalIgnoreCase))
                .Where(c => !Filtro.FechaInicio.HasValue || c.FechaPago >= Filtro.FechaInicio.Value)
                .Where(c => !Filtro.FechaFin.HasValue || c.FechaPago <= Filtro.FechaFin.Value)
                .Where(c => Filtro.IncluirDadosDeBaja || !c.DadoDeBaja)
                .ToList();
        }
    }
}
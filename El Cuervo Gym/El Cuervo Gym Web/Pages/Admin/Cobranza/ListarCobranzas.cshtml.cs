using El_Cuervo_Gym_Web.Core.Cobranza.Domain;
using El_Cuervo_Gym_Web.Core.Cobranza.Logic;
using El_Cuervo_Gym_Web.Core.Utils.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace El_Cuervo_Gym_Web.Pages.Admin.Cobranza
{
    public class ListarCobranzasModel : PageModel
    {
        private readonly ICobranzaService _cobranzaService;
        private readonly ICLogger _logger;

        public ListarCobranzasModel(ICobranzaService cobranzaService, ICLogger logger)
        {
            _cobranzaService = cobranzaService;
            _logger = logger;
        }

        public class FiltroModel
        {
            public string Nombre { get; set; }
            public int? Documento { get; set; }
            public int? NumeroSocio { get; set; }
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
        public List<PagoListado> Cobranzas { get; set; }

        public async Task OnGet()
        {
            try
            {
                if (CheckIfFiltroIsEmpty())
                {
                    Filtro = new FiltroModel()
                    {
                        IncluirDadosDeBaja = false,
                        FechaInicio = DateTime.Now.AddDays(-7),
                        FechaFin = DateTime.Now.AddDays(1)
                    };
                }
                else
                {
                    Filtro.FechaFin = Filtro.FechaFin?.AddDays(1);
                }

                var cobranzas = await _cobranzaService.ObtenerCobranzasFiltro(Filtro);
                Cobranzas = cobranzas.ToList();

                Filtro.FechaFin = Filtro.FechaFin?.AddDays(-1);
            }
            catch (Exception ex)
            {
                var contexto = "Listado de Cobranzas";
                RedirectToPage(await _logger.LogError(ex, contexto, string.Empty), new { accion = contexto, mensajeError = ex.Message });
            }
        }

        private bool CheckIfFiltroIsEmpty()
        {
            return Filtro.Nombre == null && Filtro.Documento == null && Filtro.NumeroSocio == null && Filtro.FechaInicio == null && Filtro.FechaFin == null;
        }
    }
}
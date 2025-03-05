using El_Cuervo_Gym_Web.Core.Socio.Domain;
using El_Cuervo_Gym_Web.Core.Socio.Domain.Request;
using El_Cuervo_Gym_Web.Core.Socio.Logic;
using El_Cuervo_Gym_Web.Core.Utils.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace El_Cuervo_Gym_Web.Pages.Admin.Socio
{
    public class ListarSocioModel : PageModel
    {
        private readonly ISocioService _socioService;
        private readonly ICLogger _logger;

        public ListarSocioModel(ISocioService socioService, ICLogger logger)
        {
            _socioService = socioService;
            _logger = logger;
        }

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
        public List<DatosSocio> Socios { get; set; }

        public async Task OnGet()
        {
            try
            {
                if (!CheckIfFiltroIsEmpty())
                {
                    var filtroSocios = new FiltroSocio()
                    {
                        Nombre = Filtro.Nombre,
                        Documento = Filtro.Documento,
                        NumeroSocio = Filtro.NumeroSocio,
                        FechaInicio = Filtro.FechaInicio,
                        FechaFin = Filtro.FechaFin?.AddDays(1),
                        CuotasVencidas = Filtro.CuotasVencidas,
                        IncluirDadosDeBaja = Filtro.IncluirDadosDeBaja
                    };
                    
                    var socios = await _socioService.ObtenerSocios(filtroSocios);
                    Socios = socios.ToList();
                }
            }
            catch (Exception ex)
            {
                var contexto = "Listado Socios";
                RedirectToPage(await _logger.LogError(ex, contexto, string.Empty), new { accion = contexto, mensajeError = ex.Message });
            }
        }

        private bool CheckIfFiltroIsEmpty()
        {
            return Filtro.Nombre == null && Filtro.Documento == null && Filtro.NumeroSocio == null && Filtro.FechaInicio == null && Filtro.FechaFin == null;
        }
    }
}

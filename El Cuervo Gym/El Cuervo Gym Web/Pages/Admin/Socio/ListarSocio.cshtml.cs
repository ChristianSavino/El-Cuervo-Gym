using El_Cuervo_Gym_Web.Core.Socio.Domain;
using El_Cuervo_Gym_Web.Core.Socio.Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace El_Cuervo_Gym_Web.Pages.Admin.Socio
{
    public class ListarSocioModel : PageModel
    {
        private readonly ISocioService _socioService;

        public ListarSocioModel(ISocioService socioService)
        {
            _socioService = socioService;
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
                if (CheckIfFiltroIsEmpty())
                {
                    Filtro = new FiltroModel()
                    {
                        CuotasVencidas = false,
                        IncluirDadosDeBaja = false,
                        FechaInicio = DateTime.Now.AddDays(-7),
                        FechaFin = DateTime.Now.AddDays(1)
                    };
                }
                else
                {
                    Filtro.FechaFin = Filtro.FechaFin?.AddDays(1);
                }

                var socios = await _socioService.ObtenerSocios(Filtro);
                Socios = socios.ToList();

                Filtro.FechaFin = Filtro.FechaFin?.AddDays(-1);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool CheckIfFiltroIsEmpty()
        {
            return Filtro.Nombre == null && Filtro.Documento == null && Filtro.NumeroSocio == null && Filtro.FechaInicio == null && Filtro.FechaFin == null;
        }
    }
}

using El_Cuervo_Gym_Web.Core.Admin.Domain;
using El_Cuervo_Gym_Web.Core.Ingresos.Domain;
using El_Cuervo_Gym_Web.Core.Ingresos.Domain.Request;
using El_Cuervo_Gym_Web.Core.Ingresos.Logic;
using El_Cuervo_Gym_Web.Core.Utils;
using El_Cuervo_Gym_Web.Core.Utils.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace El_Cuervo_Gym_Web.Pages.Admin.Ingreso
{
    public class ListarIngresosModel : PageModel
    {
        private readonly ICLogger _logger;
        private readonly IIngresoService _ingresoService;

        public ListarIngresosModel(ICLogger logger, IIngresoService ingresoService)
        {
            _logger = logger;
            _ingresoService = ingresoService;
        }

        public class FiltroModel
        {
            public string Nombre { get; set; }
            public int? Documento { get; set; }
            public int? NumeroSocio { get; set; }
            public DateTime? FechaInicio { get; set; }
            public DateTime? FechaFin { get; set; }
            public bool IncluirDadosDeBaja { get; set; }
            public TipoIngreso? Tipo { get; set; }
        }

        [BindProperty(SupportsGet = true)]
        public FiltroModel Filtro { get; set; }
        public bool IsAdminMaster { get; set; }
        public IEnumerable<IngresoLista> Ingresos { get; set; }


        public async Task<IActionResult> OnGet()
        {
            var admin = HttpContext.Session.GetString("Admin");
            if (admin != null)
            {
                var adminJson = JsonConvert.DeserializeObject<DatosAdminLogin>(admin);
                IsAdminMaster = adminJson.IsMaster;
            }

            try
            {
                if (CheckIfFiltroIsEmpty())
                {
                    Filtro = new FiltroModel()
                    {
                        IncluirDadosDeBaja = false,
                        FechaInicio = DateTime.Now.Date,
                        FechaFin = DateTime.Now.Date,
                        Tipo = null
                    };
                }

                var filtroIngreso = new FiltroIngreso()
                {
                    Nombre = Filtro.Nombre,
                    Documento = Filtro.Documento,
                    NumeroSocio = Filtro.NumeroSocio,
                    FechaInicio = Filtro.FechaInicio,
                    FechaFin = Filtro.FechaFin?.AddDays(1),
                    IncluirDadosDeBaja = Filtro.IncluirDadosDeBaja,
                    Tipo = Filtro.Tipo
                };

                Ingresos = await _ingresoService.ObtenerIngresosFiltro(filtroIngreso);
            }
            catch (Exception ex)
            {
                var contexto = "Listado de Ingresos";
                return RedirectToPage(await _logger.LogError(ex, contexto, string.Empty), new { accion = contexto, mensajeError = ex.Message });
            }

            return Page();
        }

        private bool CheckIfFiltroIsEmpty()
        {
            return Filtro.Nombre == null && Filtro.Documento == null && Filtro.NumeroSocio == null && Filtro.FechaInicio == null && Filtro.FechaFin == null && Filtro.Tipo == null;
        }
    }
}

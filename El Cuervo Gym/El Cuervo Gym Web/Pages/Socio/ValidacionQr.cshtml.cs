using El_Cuervo_Gym_Web.Core.Ingresos.Domain;
using El_Cuervo_Gym_Web.Core.Ingresos.Logic;
using El_Cuervo_Gym_Web.Core.Parametros.Logic;
using El_Cuervo_Gym_Web.Core.Socio.Domain;
using El_Cuervo_Gym_Web.Core.Socio.Logic;
using El_Cuervo_Gym_Web.Core.Utils;
using El_Cuervo_Gym_Web.Core.Utils.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace El_Cuervo_Gym_Web.Pages.Socio
{
    public class ValidacionQrModel : PageModel
    {
        private readonly ISocioService _socioService;
        private readonly IParametros _parametros;
        private readonly ICLogger _logger;
        private readonly IIngresoService _ingresoService;

        public ValidacionQrModel(ISocioService socioService, IParametros parametros, ICLogger logger, IIngresoService ingresoService)
        {
            _socioService = socioService;
            _parametros = parametros;
            _logger = logger;
            _ingresoService = ingresoService;
        }

        public bool Alerta { get; set; }
        public bool Prohibir { get; set; }
        public int DiasAtraso { get; set; }
        public int IngresosPrevios { get; set; }
        public DatosSocio Socio { get; set; }

        public async Task<IActionResult> OnGet()
        {
            try
            {
                var parametroMaxDias = int.Parse(_parametros.ObtenerTodosLosParametros().First(p => p.Clave == Helper.DiasMaxPermitidoParamName).Valor);
                var socioId = int.Parse(HttpContext.Session.GetString("Socio"));

                Socio = await _socioService.ObtenerSocioConPagosPorId(socioId);

                DiasAtraso = (DateTime.Now - Socio.ProximoVencimientoCuota).Days;
                if (DiasAtraso < 0)
                {
                    DiasAtraso = 0;
                }

                var ingresos = await _ingresoService.ObtenerIngresosEnElDiaSocio(DateTime.Now, socioId);
                IngresosPrevios = ingresos.Count();

                var tipoIngreso = TipoIngreso.Normal;

                if(DiasAtraso > 0 || IngresosPrevios > 0)
                {
                    Alerta = true;
                    tipoIngreso = TipoIngreso.Alerta;
                }
                
                if (DiasAtraso > parametroMaxDias || IngresosPrevios > 1)
                {
                    Prohibir = true;
                    tipoIngreso = TipoIngreso.Prohibir;
                }

                var ingreso = new Ingreso
                {
                    FechaIngreso = DateTime.Now,
                    IdSocio = socioId,
                    Estado = Estado.Activo,
                    Tipo = tipoIngreso
                };

                ingreso.Id = await _ingresoService.InsertarIngreso(ingreso);
                if (ingreso.Id == 0)
                {
                    throw new Exception("No se pudo registrar el ingreso");
                }
            }
            catch (Exception ex)
            {
                var contexto = "Resultados QR";
                return RedirectToPage(await _logger.LogErrorSocio(ex, contexto, string.Empty), new { accion = contexto, mensajeError = ex.Message });
            }

            return Page();
        }
    }
}

using El_Cuervo_Gym_Web.Core.Parametros.Logic;
using El_Cuervo_Gym_Web.Core.Socio.Domain;
using El_Cuervo_Gym_Web.Core.Socio.Logic;
using El_Cuervo_Gym_Web.Core.Utils;
using El_Cuervo_Gym_Web.Core.Utils.Logging;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace El_Cuervo_Gym_Web.Pages.Socio
{
    public class ValidacionQrModel : PageModel
    {
        private readonly ISocioService _socioService;
        private readonly IParametros _parametros;
        private readonly ICLogger _logger;

        public ValidacionQrModel(ISocioService socioService, IParametros parametros, ICLogger logger)
        {
            _socioService = socioService;
            _parametros = parametros;
            _logger = logger;
        }

        public bool Alerta { get; set; }
        public bool Prohibir { get; set; }
        public int DiasAtraso { get; set; }
        public int IngresosPrevios { get; set; }
        public DatosSocio Socio { get; set; }

        public async Task OnGet()
        {
            try
            {
                var parametroMaxDias = int.Parse(_parametros.ObtenerTodosLosParametros().First(p => p.Clave == Helper.DiasMaxPermitidoParamName).Valor);
                var socioId = int.Parse(HttpContext.Session.GetString("Socio"));

                Socio = await _socioService.ObtenerSocioConPagosPorId(socioId);

                DiasAtraso = (DateTime.Now - Socio.ProximoVencimientoCuota).Days;
                if(DiasAtraso < 0)
                {
                    DiasAtraso = 0;
                }

                IngresosPrevios = 0;

                Alerta = DiasAtraso > 0 || IngresosPrevios > 0;
                Prohibir = DiasAtraso > parametroMaxDias || IngresosPrevios > 1;
            }
            catch (Exception ex)
            {
                var contexto = "Resultados QR";
                RedirectToPage(await _logger.LogError(ex, contexto, string.Empty), new { accion = contexto, mensajeError = ex.Message });
            }
        }
    }
}

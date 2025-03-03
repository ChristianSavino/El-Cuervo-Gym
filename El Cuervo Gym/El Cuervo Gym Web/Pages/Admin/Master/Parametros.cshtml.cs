using El_Cuervo_Gym_Web.Core.Parametros.Logic;
using El_Cuervo_Gym_Web.Core.Utils;
using El_Cuervo_Gym_Web.Core.Utils.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace El_Cuervo_Gym_Web.Pages.Admin.Master
{
    public class ParametrosModel : PageModel
    {
        private readonly IParametros _parametros;
        private readonly ICLogger _logger;

        public ParametrosModel(IParametros parametros, ICLogger logger)
        {
            _parametros = parametros;
            _logger = logger;
        }

        [BindProperty]
        public int DiasMaximosAtraso { get; set; }

        [BindProperty]
        public decimal ValorCuotaGeneral { get; set; }

        public void OnGet()
        {
            var parametros = _parametros.ObtenerTodosLosParametros();

            DiasMaximosAtraso = int.Parse(parametros.First(x => x.Clave == Helper.DiasMaxPermitidoParamName).Valor);
            ValorCuotaGeneral = int.Parse(parametros.First(x => x.Clave == Helper.ValorCuotaParamName).Valor);
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var parametros = _parametros.ObtenerTodosLosParametros();

                parametros.First(x => x.Clave == Helper.DiasMaxPermitidoParamName).Valor = DiasMaximosAtraso.ToString();
                parametros.First(x => x.Clave == Helper.ValorCuotaParamName).Valor = ValorCuotaGeneral.ToString();

                await _parametros.ActualizarTodosLosParametros(parametros);

                TempData["SuccessMessage"] = "Parámetros modificados correctamente.";
            }
            catch (Exception ex)
            {
                TempData["SuccessMessage"] = null;
                var contexto = "Parámetros";
                return RedirectToPage(await _logger.LogError(ex, contexto, string.Empty), new { accion = contexto, mensajeError = ex.Message });
            }

            return RedirectToPage("/Admin/Master/Parametros");
        }
    }
}

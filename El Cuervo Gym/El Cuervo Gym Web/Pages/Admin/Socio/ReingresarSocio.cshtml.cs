using El_Cuervo_Gym_Web.Core.Admin.Domain;
using El_Cuervo_Gym_Web.Core.Admin.Logic;
using El_Cuervo_Gym_Web.Core.Cobranza.Domain;
using El_Cuervo_Gym_Web.Core.Parametros.Logic;
using El_Cuervo_Gym_Web.Core.Utils;
using El_Cuervo_Gym_Web.Core.Utils.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace El_Cuervo_Gym_Web.Pages.Admin.Socio
{
    public class ReingresarSocioModel : PageModel
    {
        private readonly ICLogger _logger;
        private readonly IAdminService _adminService;
        private readonly IParametros _parametros;

        public string NombreCompleto { get; set; }
        public int Documento { get; set; }
        public int NumeroSocio { get; set; }
        public DateTime ProximaFechaPago { get; set; }

        public ReingresarSocioModel(ICLogger logger, IAdminService adminService, IParametros parametros)
        {
            _logger = logger;
            _adminService = adminService;
            _parametros = parametros;
        }

        public async Task<IActionResult> OnGet(string nombreCompleto, int documento, int numeroSocio)
        {
            var admin = HttpContext.Session.GetString("Admin");
            var adminJson = (DatosAdminLogin)null;
            if (admin != null)
            {
                adminJson = JsonConvert.DeserializeObject<DatosAdminLogin>(admin);
            }

            try
            {
                var valorCuota = _parametros.ObtenerTodosLosParametros().FirstOrDefault(x => x.Clave == Helper.ValorCuotaParamName);

                await _adminService.ReIngresoSocio(numeroSocio, DateTime.Now, new Pago
                {
                    FechaPago = DateTime.Now,
                    Monto = valorCuota != null ? int.Parse(valorCuota.Valor) : 0,
                    MetodoPago = TipoPago.ReIngreso,
                    Estado = Estado.Activo,
                    IdSocio = numeroSocio,
                    IdAdmin = adminJson.Id,
                    FechaCuota = DateTime.Now.Date,
                    Comprobante = "Re-Ingreso de socio"
                });
            }
            catch (Exception ex)
            {
                var contexto = "Re-Ingreso de Socio";
                return RedirectToPage(await _logger.LogError(ex, contexto, string.Empty), new { accion = contexto, mensajeError = ex.Message });
            }

            NombreCompleto = nombreCompleto;
            Documento = documento;
            NumeroSocio = numeroSocio;
            ProximaFechaPago = DateTime.Now.AddMonths(1).Date;

            return Page();
        }
    }
}

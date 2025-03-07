using El_Cuervo_Gym_Web.Core.Admin.Domain;
using El_Cuervo_Gym_Web.Core.Admin.Logic;
using El_Cuervo_Gym_Web.Core.Cobranza.Domain;
using El_Cuervo_Gym_Web.Core.Parametros.Logic;
using El_Cuervo_Gym_Web.Core.Socio.Logic;
using El_Cuervo_Gym_Web.Core.Utils;
using El_Cuervo_Gym_Web.Core.Utils.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace El_Cuervo_Gym_Web.Pages.Admin.Cobranza
{
    public class AltaCobranzaModel : PageModel
    {
        private readonly IAdminService _adminService;
        private readonly ISocioService _socioService;
        private readonly IParametros _parametros;
        private readonly ICLogger _logger;

        public AltaCobranzaModel(IAdminService adminService, IParametros parametros, ICLogger logger, ISocioService socioService)
        {
            _adminService = adminService;
            _socioService = socioService;
            _parametros = parametros;
            _logger = logger;
        }

        public class CobranzaModel
        {
            public string Nombre { get; set; }
            public string Documento { get; set; }
            public string NumeroSocio { get; set; }
            [DataType(DataType.DateTime)]
            public DateTime FechaPago { get; set; }
            public DateTime FechaCuota { get; set; }
            public int Monto { get; set; }
            public string Comprobante { get; set; }
            public TipoPago TipoPago { get; set; }
        }

        [BindProperty]
        public CobranzaModel Cobranza { get; set; }

        public async Task<IActionResult> OnGet(int socioId)
        {
            try
            {
                var socio = await _socioService.ObtenerSocioPorId(socioId);
                var valorCuota = _parametros.ObtenerTodosLosParametros().FirstOrDefault(x => x.Clave == Helper.ValorCuotaParamName);

                var now = DateTime.Now;
                var fechaPago = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);

                Cobranza = new CobranzaModel()
                {
                    Nombre = $"{socio.Nombre} {socio.Apellido}",
                    Documento = socio.Documento.ToString(),
                    NumeroSocio = socio.Id.ToString(),
                    FechaPago = fechaPago,
                    FechaCuota = socio.ProximoVencimientoCuota,
                    Monto = valorCuota != null ? int.Parse(valorCuota.Valor) : 0
                };

            }
            catch (Exception ex)
            {
                var contexto = "Alta Cobranza";
                return RedirectToPage(await _logger.LogError(ex, contexto, string.Empty), new { accion = contexto, mensajeError = ex.Message });
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var proximaCuota = DateTime.Now;

            try
            {
                var admin = JsonConvert.DeserializeObject<DatosAdminLogin>(HttpContext.Session.GetString("Admin"));

                var socioId = int.Parse(Cobranza.NumeroSocio);
                proximaCuota = await _adminService.CobrarSocio(socioId, Cobranza.FechaCuota, new Pago()
                {
                    FechaPago = Cobranza.FechaPago,
                    Monto = Cobranza.Monto,
                    FechaCuota = Cobranza.FechaCuota,
                    Comprobante = Cobranza.Comprobante,
                    Estado = Estado.Activo,
                    IdAdmin = admin.Id,
                    IdSocio = socioId,
                    MetodoPago = Cobranza.TipoPago
                });
            }
            catch (Exception ex)
            {
                var contexto = "Alta Cobranza";
                return RedirectToPage(await _logger.LogError(ex, contexto, string.Empty), new { accion = contexto, mensajeError = ex.Message });
            }

            return RedirectToPage("/Admin/Cobranza/Responses/CobranzaCorrecta", new { siguienteCuota = proximaCuota.ToString("yyyy-MM-dd") });
        }
    }
}
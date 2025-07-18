using El_Cuervo_Gym_Web.Core.Cobranza.Domain;
using El_Cuervo_Gym_Web.Core.Ingresos.Domain.Request;
using El_Cuervo_Gym_Web.Core.Ingresos.Logic;
using El_Cuervo_Gym_Web.Core.Socio.Logic;
using El_Cuervo_Gym_Web.Core.Utils;
using El_Cuervo_Gym_Web.Core.Utils.Logging;
using El_Cuervo_Gym_Web.Core.WhatsApp.Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace El_Cuervo_Gym_Web.Pages.Admin.Socio
{
    public class DetalleSocioModel : PageModel
    {
        private readonly ISocioService _socioService;
        private readonly IIngresoService _ingresoService;
        private readonly IWhatsAppService _whatsAppService;
        private readonly ICLogger _logger;

        public DetalleSocioModel(ISocioService socioService, IIngresoService ingresoService, IWhatsAppService whatsAppService, ICLogger logger)
        {
            _socioService = socioService;
            _ingresoService = ingresoService;
            _whatsAppService = whatsAppService;
            _logger = logger;
        }

        public class SocioModel
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public string Documento { get; set; }
            public string Telefono { get; set; }
            public string ObraSocial { get; set; }
            public string NumeroObraSocial { get; set; }
            public string NumeroEmergencia { get; set; }
            public string ContactoEmergencia { get; set; }
            public DateTime FechaSubscripcion { get; set; }
            public DateTime ProximoVencimientoCuota { get; set; }
            public string Estado { get; set; }
            public IEnumerable<Pago> UltimosPagos { get; set; }
        }

        public SocioModel Socio { get; set; }
        public bool DadoBaja { get; set; }
        public bool AptoReIngreso { get; set; }

        public async Task<IActionResult> OnGet(int socioId)
        {
            try
            {
                var socio = await _socioService.ObtenerSocioConPagosPorId(socioId);

                Socio = new SocioModel
                {
                    Id = socio.Id,
                    Nombre = socio.Nombre,
                    Apellido = socio.Apellido,
                    Documento = socio.Documento.ToString(),
                    Telefono = socio.Telefono.ToString(),
                    ObraSocial = socio.ObraSocial,
                    NumeroObraSocial = socio.NumeroObraSocial,
                    NumeroEmergencia = socio.NumeroEmergencia.ToString(),
                    ContactoEmergencia = socio.ContactoEmergencia,
                    FechaSubscripcion = socio.FechaSubscripcion,
                    ProximoVencimientoCuota = socio.ProximoVencimientoCuota,
                    Estado = socio.Estado.ToString(),
                    UltimosPagos = socio.UltimosPagos?.OrderByDescending(p => p.FechaPago).Take(5)
                };

                DadoBaja = socio.Estado == Estado.Baja;

                var fechaAptoReIngreso = socio.ProximoVencimientoCuota.AddMonths(1).Date;

                if (fechaAptoReIngreso < DateTime.Now)
                {
                    var filtroIngreso = new FiltroIngreso
                    {
                        FechaInicio = socio.ProximoVencimientoCuota.Date,
                        FechaFin = fechaAptoReIngreso,
                        IncluirDadosDeBaja = false,
                        NumeroSocio = socioId
                    };

                    var ingresos = await _ingresoService.ObtenerIngresosFiltro(filtroIngreso);
                    AptoReIngreso = ingresos.Count() == 0;
                }

            }
            catch (Exception ex)
            {
                var contexto = "Detalle Socio";
                return RedirectToPage(await _logger.LogError(ex, contexto, string.Empty), new { accion = contexto, mensajeError = ex.Message });
            }

            return Page();
        }

        public async Task<IActionResult> OnPostEnviarWhatsappAsync(int socioId)
        {
            var socio = await _socioService.ObtenerSocioPorId(socioId);

            await _whatsAppService.EnviarMensajeAsync(socio.Telefono.ToString(), socio);

            TempData["MensajeWhatsapp"] = "Mensaje enviado correctamente.";
            return RedirectToPage(new { socioId });
        }
    }
}
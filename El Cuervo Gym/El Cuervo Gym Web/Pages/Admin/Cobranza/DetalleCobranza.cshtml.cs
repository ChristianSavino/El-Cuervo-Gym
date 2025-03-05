using El_Cuervo_Gym_Web.Core.Cobranza.Logic;
using El_Cuervo_Gym_Web.Core.Utils;
using El_Cuervo_Gym_Web.Core.Utils.Logging;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace El_Cuervo_Gym_Web.Pages.Admin.Cobranza
{
    public class DetalleCobranzaModel : PageModel
    {
        private readonly ICobranzaService _cobranzaService;
        private readonly ICLogger _logger;

        public DetalleCobranzaModel(ICobranzaService cobranzaService, ICLogger logger)
        {
            _cobranzaService = cobranzaService;
            _logger = logger;
        }

        public class CobranzaModel
        {
            public int Id { get; set; }
            public int IdSocio { get; set; }
            public DateTime FechaPago { get; set; }
            public DateTime FechaCuota { get; set; }
            public decimal Monto { get; set; }
            public Estado Estado { get; set; }
            public TipoPago MetodoPago { get; set; }
            public string Comprobante { get; set; }
            public string Nombre { get; set; }
            public int Documento { get; set; }
        }

        public CobranzaModel Cobranza { get; set; }
        public bool ExistenPagosPosteriores { get; set; }


        public async Task OnGet(int cobranzaId)
        {
            try
            {
                var result = await _cobranzaService.ObtenerCobranzaPorIdValidada(cobranzaId);

                Cobranza = new CobranzaModel
                {
                    Id = result.cobranza.Id,
                    Nombre = result.cobranza.Nombre,
                    Documento = result.cobranza.Documento,
                    IdSocio = result.cobranza.IdSocio,
                    FechaPago = result.cobranza.FechaPago,
                    FechaCuota = result.cobranza.FechaCuota,
                    Monto = result.cobranza.Monto,
                    MetodoPago = result.cobranza.MetodoPago,
                    Comprobante = result.cobranza.Comprobante,
                    Estado = result.cobranza.Estado
                };

                ExistenPagosPosteriores = result.existenPagosPosteriores;
            }
            catch (Exception ex)
            {
                var contexto = "Detalle de Cobranzas";
                RedirectToPage(await _logger.LogError(ex, contexto, string.Empty), new { accion = contexto, mensajeError = ex.Message });
            }         
        }
    }
}



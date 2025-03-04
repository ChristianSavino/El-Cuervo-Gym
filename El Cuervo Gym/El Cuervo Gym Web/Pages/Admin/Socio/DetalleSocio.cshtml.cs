using El_Cuervo_Gym_Web.Core.Socio.Logic;
using El_Cuervo_Gym_Web.Core.Utils.Logging;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace El_Cuervo_Gym_Web.Pages.Admin.Socio
{
    public class DetalleSocioModel : PageModel
    {
        private readonly ISocioService _socioService;
        private readonly ICLogger _logger;

        public DetalleSocioModel(ISocioService socioService, ICLogger logger)
        {
            _socioService = socioService;
            _logger = logger;
        }

        public class Pago
        {
            public DateTime FechaPago { get; set; }
            public decimal Monto { get; set; }
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
            public List<Pago> UltimosPagos { get; set; }
        }

        public SocioModel Socio { get; set; }

        public async Task OnGet(int socioId)
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
                    UltimosPagos = socio.UltimosPagos.Select(p => new Pago
                    {
                        FechaPago = p.FechaPago,
                        Monto = p.Monto
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                var contexto = "Detalle Socio";
                RedirectToPage(await _logger.LogError(ex, contexto, string.Empty), new { accion = contexto, mensajeError = ex.Message });
            }
        }
    }
}
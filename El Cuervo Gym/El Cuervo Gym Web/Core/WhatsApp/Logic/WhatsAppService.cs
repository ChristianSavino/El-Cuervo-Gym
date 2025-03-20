using El_Cuervo_Gym_Web.Core.Socio.Domain;
using El_Cuervo_Gym_Web.Core.WhatsApp.Domain;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace El_Cuervo_Gym_Web.Core.WhatsApp.Logic
{
    public class WhatsAppService : IWhatsAppService
    {
        private readonly HttpClient _httpClient;
        private readonly WhatsAppSettings _settings;
        private readonly ILogger _logger;

        public WhatsAppService(HttpClient httpClient, IOptions<WhatsAppSettings> settings, ILogger<WhatsAppService> logger)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task EnviarMensajeAsync(string numero, DatosSocio socio)
        {
            if (!_settings.Habilitado)
            {
                return;
            }
            
            var numeroFormateado = FormatearNumeroWhatsApp(numero);

            var url = $"https://graph.facebook.com/v22.0/{_settings.PhoneNumberId}/messages";

            var message = new
            {
                messaging_product = "whatsapp",
                to = numeroFormateado,
                type = "template",
                template = new
                {
                    name = "mensaje_bienvenida",
                    language = new { code = "es" },
                    components = new[]
                     {
                        new
                        {
                            type = "body",
                            parameters = new[]
                            {
                                new { type = "text", text = socio.Nombre },
                                new { type = "text", text = socio.Apellido },
                                new { type = "text", text = socio.Id.ToString() },
                                new { type = "text", text = socio.ProximoVencimientoCuota.ToString("dd/MM/yyyy") }
                            }
                        }
                    }
                }
            };

            var json = JsonConvert.SerializeObject(message);
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            request.Headers.Add("Authorization", $"Bearer {_settings.AccessToken}");

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
               _logger.LogError($"Error al enviar mensaje de WhatsApp a {numeroFormateado}");
            }
        }

        public string FormatearNumeroWhatsApp(string numero)
        {
            numero = numero.Trim().Replace(" ", "").Replace("-", "");

            if (numero.StartsWith("15") && numero.Length == 10)
            {
                numero = "9" + numero;
            }

            if (numero.StartsWith("0"))
            {
                numero = numero.Substring(1);
            }

            return "54" + numero;
        }
    }
}

using El_Cuervo_Gym_Web.Core.Socio.Domain;

namespace El_Cuervo_Gym_Web.Core.WhatsApp.Logic
{
    public interface IWhatsAppService
    {
        Task EnviarMensajeAsync(string numero, DatosSocio socio);
    }
}

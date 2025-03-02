using El_Cuervo_Gym_Web.Core.Cobranza.Domain;
using El_Cuervo_Gym_Web.Core.Socio.Domain;

namespace El_Cuervo_Gym_Web.Core.Cobranza.Logic
{
    public interface ICobranzaService
    {
        Task<int> InsertarCobranzaNuevoSocio(DatosSocio socio);
        Task<int> InsertarCobranza(Pago cobranza);
    }
}

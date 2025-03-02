using El_Cuervo_Gym_Web.Core.Cobranza.Domain;

namespace El_Cuervo_Gym_Web.Core.Cobranza.DataAccess
{
    public interface ICobranzaDataAccess
    {
        Task<int> InsertarCobranza(Pago cobranza);
    }
}

using El_Cuervo_Gym_Web.Core.Cobranza.Domain;
using El_Cuervo_Gym_Web.Core.Cobranza.Domain.Request;

namespace El_Cuervo_Gym_Web.Core.Cobranza.DataAccess
{
    public interface ICobranzaDataAccess
    {
        Task<int> InsertarCobranza(Pago cobranza);
        Task<IEnumerable<PagoListado>> ObtenerCobranzasFiltro(FiltroCobranza filtro);
        Task<PagoListado> ObtenerCobranzaPorId(int id);
        Task<bool> ExistePagosPosteriores(FiltroCobranza filtro);
        Task<bool> DarDeBajaComprobante(int idComprobante);
    }
}

using El_Cuervo_Gym_Web.Core.Cobranza.Domain;
using El_Cuervo_Gym_Web.Core.Cobranza.Domain.Request;
using El_Cuervo_Gym_Web.Core.Socio.Domain;

namespace El_Cuervo_Gym_Web.Core.Cobranza.Logic
{
    public interface ICobranzaService
    {
        Task<int> InsertarCobranzaNuevoSocio(DatosSocio socio);
        Task<int> InsertarCobranza(Pago cobranza);
        Task<IEnumerable<PagoListado>> ObtenerCobranzasFiltro(FiltroCobranza filtro);
        Task<PagoListado> ObtenerCobranzaPorId(int idCobranza);
        Task<(PagoListado cobranza, bool existenPagosPosteriores)> ObtenerCobranzaPorIdValidada(int idCobranza);
        Task<bool> DarDeBajaComprobante(int idComprobante);
    }
}

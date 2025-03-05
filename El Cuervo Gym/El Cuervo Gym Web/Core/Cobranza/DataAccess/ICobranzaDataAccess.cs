using El_Cuervo_Gym_Web.Core.Cobranza.Domain;
using static El_Cuervo_Gym_Web.Pages.Admin.Cobranza.ListarCobranzasModel;

namespace El_Cuervo_Gym_Web.Core.Cobranza.DataAccess
{
    public interface ICobranzaDataAccess
    {
        Task<int> InsertarCobranza(Pago cobranza);
        Task<IEnumerable<PagoListado>> ObtenerCobranzasFiltro(FiltroModel filtro);
    }
}

namespace El_Cuervo_Gym_Web.Core.Utils.Logging
{
    public interface ICLogger
    {
        Task<string> LogError(Exception ex, string contexto, string extraInfo = "");
        Task<string> LogErrorSocio(Exception ex, string contexto, string extraInfo = "");
    }
}

namespace El_Cuervo_Gym_Web.Core.Utils
{
    public static class Helper
    {
        public static bool IsSessionAdmin(HttpContext httpContext)
        {
            return httpContext.Session.GetString("Admin") != null;
        }
    }
}

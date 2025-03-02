using El_Cuervo_Gym_Web.Core.Admin.DataAccess;
using El_Cuervo_Gym_Web.Core.Admin.Logic;
using El_Cuervo_Gym_Web.Core.Cobranza.DataAccess;
using El_Cuervo_Gym_Web.Core.Cobranza.Logic;
using El_Cuervo_Gym_Web.Core.DataAccess;
using El_Cuervo_Gym_Web.Core.Socio.DataAccess;
using El_Cuervo_Gym_Web.Core.Socio.Logic;

namespace El_Cuervo_Gym_Web.Configuration
{
    public static class Bootstrapper
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IConnection, Connection>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IAdminDataAccess, AdminDataAccess>();
            services.AddScoped<ISocioService, SocioService>();
            services.AddScoped<ISocioDataAccess, SocioDataAccess>();
            services.AddScoped<ICobranzaService, CobranzaService>();
            services.AddScoped<ICobranzaDataAccess, CobranzaDataAccess>();

            services.AddRazorPages();

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
        }
    }
}

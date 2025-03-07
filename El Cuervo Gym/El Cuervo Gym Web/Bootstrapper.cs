using El_Cuervo_Gym_Web.Core.Admin.DataAccess;
using El_Cuervo_Gym_Web.Core.Admin.Logic;
using El_Cuervo_Gym_Web.Core.Cobranza.DataAccess;
using El_Cuervo_Gym_Web.Core.Cobranza.Logic;
using El_Cuervo_Gym_Web.Core.DataAccess;
using El_Cuervo_Gym_Web.Core.Ingresos.DataAccess;
using El_Cuervo_Gym_Web.Core.Ingresos.Logic;
using El_Cuervo_Gym_Web.Core.Parametros.DataAccess;
using El_Cuervo_Gym_Web.Core.Parametros.Logic;
using El_Cuervo_Gym_Web.Core.Socio.DataAccess;
using El_Cuervo_Gym_Web.Core.Socio.Logic;
using El_Cuervo_Gym_Web.Core.Utils.Logging;

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
            services.AddScoped<ICLogger, CLogger>();
            services.AddScoped<IIngresoDataAccess, IngresoDataAccess>();
            services.AddScoped<IIngresoService, IngresoService>();

            services.AddScoped<IParametrosDataAccess, ParametrosDataAccess>();
            services.AddScoped<IParametros, Parametros>();


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

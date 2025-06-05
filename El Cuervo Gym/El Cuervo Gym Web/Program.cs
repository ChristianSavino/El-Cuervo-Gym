using El_Cuervo_Gym_Web.Configuration;
using El_Cuervo_Gym_Web.Core.DataAccess;

var builder = WebApplication.CreateBuilder(args);

var ports = builder.Configuration["Hosting:Ports"];
var path = Directory.GetCurrentDirectory();

var certPath = Path.Combine(path, "Certs", builder.Configuration["Cert:CertName"]);
var password = builder.Configuration["Cert:CertPassword"];

var portHttp = int.Parse(ports.Split(";")[0]);
var portHttps = int.Parse(ports.Split(";")[1]);

Bootstrapper.ConfigureServices(builder.Services, builder.Configuration);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(portHttp);
    options.ListenAnyIP(portHttps, listenOptions =>
    {
        listenOptions.UseHttps(certPath, password);
    });
});

var app = builder.Build();
app.UseCookiePolicy();
app.UseSession();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var connection = scope.ServiceProvider.GetRequiredService<IConnection>();
    await connection.CorrerTablas();
}

app.Run();
using El_Cuervo_Gym_Web.Configuration;
using El_Cuervo_Gym_Web.Core.DataAccess;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

var urls = builder.Configuration["Hosting:Urls"];
var path = Directory.GetCurrentDirectory();
Console.WriteLine(path);
var certPath = Path.Combine(path, "Certs", "localhost.pfx");
var certPassword = "@Keru181197";

var cert = new X509Certificate2(certPath, certPassword);
var https = urls.Split(";")[1];
var port = int.Parse(https.Split(":")[2]);

if (!string.IsNullOrEmpty(urls))
{
    builder.WebHost.UseUrls(urls);
}

Bootstrapper.ConfigureServices(builder.Services, builder.Configuration);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(port, listenOptions =>
    {
        listenOptions.UseHttps(cert);
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
app.UseCors("AllowGithubPages");
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var connection = scope.ServiceProvider.GetRequiredService<IConnection>();
    await connection.CorrerTablas();
}

app.Run();
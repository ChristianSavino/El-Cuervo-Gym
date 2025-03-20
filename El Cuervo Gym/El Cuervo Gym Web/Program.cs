using El_Cuervo_Gym_Web.Configuration;
using El_Cuervo_Gym_Web.Core.DataAccess;

var builder = WebApplication.CreateBuilder(args);

var urls = builder.Configuration["Hosting:Urls"];

if (!string.IsNullOrEmpty(urls))
{
    builder.WebHost.UseUrls(urls);
}

Bootstrapper.ConfigureServices(builder.Services, builder.Configuration);

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

using (var scope = app.Services.CreateScope())
{
    var connection = scope.ServiceProvider.GetRequiredService<IConnection>();
    await connection.CorrerTablas();
}

app.Run();
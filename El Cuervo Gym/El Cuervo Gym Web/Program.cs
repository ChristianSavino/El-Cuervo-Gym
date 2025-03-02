using El_Cuervo_Gym_Web.Configuration;
using El_Cuervo_Gym_Web.Core.DataAccess;

var builder = WebApplication.CreateBuilder(args);

Bootstrapper.ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

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

app.UseSession();

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var connection = scope.ServiceProvider.GetRequiredService<IConnection>();
    await connection.ExecuteSqlScriptAsync("Core/DataAccess/Script.sql");
}

app.Run();
using LeadTheBoard.Application.Utilities.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextConfig(builder.Configuration);

builder.Services.AddApplicationDependencyConfig();

builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(12); // Oturum s�resi 12 saat
        options.LoginPath = "/Account/Login"; // Giri� yap�lmas� gereken yol
        options.AccessDeniedPath = "/Account/AccessDenied"; // Eri�im reddedildi�i zaman y�nlendirilecek yol
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin")); // Sadece "Admin" rol�ne sahip kullan�c�lar eri�ebilir
    options.AddPolicy("ManagerOnly", policy => policy.RequireRole("Manager")); // Sadece "Manager" rol�ne sahip kullan�c�lar eri�ebilir
    options.AddPolicy("OperatorOnly", policy => policy.RequireRole("Operator")); // Sadece "Operator" rol�ne sahip kullan�c�lar eri�ebilir
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error-development");
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await app.Services.SeedDataAsync();

app.Run();

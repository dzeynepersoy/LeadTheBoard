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
        options.ExpireTimeSpan = TimeSpan.FromHours(12); // Oturum süresi 12 saat
        options.LoginPath = "/Account/Login"; // Giriþ yapýlmasý gereken yol
        options.AccessDeniedPath = "/Account/AccessDenied"; // Eriþim reddedildiði zaman yönlendirilecek yol
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin")); // Sadece "Admin" rolüne sahip kullanýcýlar eriþebilir
    options.AddPolicy("ManagerOnly", policy => policy.RequireRole("Manager")); // Sadece "Manager" rolüne sahip kullanýcýlar eriþebilir
    options.AddPolicy("OperatorOnly", policy => policy.RequireRole("Operator")); // Sadece "Operator" rolüne sahip kullanýcýlar eriþebilir
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

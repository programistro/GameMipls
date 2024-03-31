using GameMipls.Net.Data;
using GameMipls.Net.Models;
using GameMipls.Net.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<AccountService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

// builder.Services.AddAuthentication(options =>
// {
//     options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//     options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
// }).AddCookie("LoginScheme", options =>
// {
//     // ��������� ��� ����� ����� ��������������
//     options.Cookie.Name = "LoginScheme";
//     options.Cookie.HttpOnly = true; // Только HTTP
//     options.ExpireTimeSpan = TimeSpan.FromMinutes(120); // Время жизни cookie
//     options.LoginPath = "/Home/Reg"; // Путь к странице входа
//     // ...
// });
// builder.Services.AddAuthentication()
//     .AddCookie("Coockie", options =>
//     {
//         options.LoginPath = "/Home/Reg";
//         // options.LogoutPath = "/Account/Logout";
//         // options.AccessDeniedPath = "/Account/AccessDenied";
//         options.ExpireTimeSpan = TimeSpan.FromDays(60);
//         options.SlidingExpiration = true;
//         // Другие настройки куки
//     });

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie("Cookie", options =>
    {
        options.LoginPath = "/Home/Reg";
        // options.LogoutPath = "/Account/Logout";
        // options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromDays(60);
        options.SlidingExpiration = true;
    });

builder.Services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<AppDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
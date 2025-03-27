using Dip.Repository;
using Dip.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.AspNetCore.Authentication.Cookies;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Dip.Pages;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<DiplomaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ÏîëüçîâàòåëèRepository>();
builder.Services.AddScoped<ÐåñòîðàíRepository>();
builder.Services.AddScoped<ÌåíþRepository>();
builder.Services.AddScoped<ÇàêàçûRepository>();
builder.Services.AddScoped<ÊîðçèíàRepository>();
builder.Services.AddSingleton<CloudinaryService>();
builder.Services.AddScoped<ÐîëèRepository>();
builder.Services.AddScoped<ÎöåíêèRepository>();
builder.Services.AddScoped<ÑîñòàâÊîðçèíûRepository>();
builder.Services.AddScoped<ÑêèäêèRepository>();
builder.Services.AddHostedService<DiscountCleanupService>();

builder.Services.AddRazorPages();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";  
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
    });

builder.Services.AddAuthorization();

CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

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

app.MapGet("/", async context =>
{
    if (context.User.Identity.IsAuthenticated)
    {
        context.Response.Redirect("/Index");
    }
    else
    {
        context.Response.Redirect("/Account/Login");
    }
    await Task.CompletedTask;
});

app.MapRazorPages();
app.Run();
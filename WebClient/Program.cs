﻿using ClassLibrary.Configuration;
using ClassLibrary.Exceptions;
using ClassLibrary.Repository;
using ClassLibrary.Proxy;
using ClassLibrary.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using WebClient.Proxy;
using WebClient.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure ApiSettings from configuration
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

// Configure HttpClient for NotificationService with BaseAddress from ApiSettings
builder.Services.AddHttpClient<WebClient.Services.NotificationService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"]);
});

// Register scoped services
builder.Services.AddScoped<ILogRepository, LoggerProxy>();
builder.Services.AddScoped<ILogInRepository, LogInProxy>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ILoggerService, LoggerService>();

// Configure cookie-based authentication
builder.Services.AddScoped<IUserRepository, UserProxy>();

builder.Services.AddScoped<IDoctorRepository, DoctorsProxy>();
builder.Services.AddScoped<IDoctorService, DoctorService>();

builder.Services.AddScoped<IPatientRepository, PatientProxy>();
builder.Services.AddScoped<IPatientService, PatientService>();  

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Route for doctor pages
app.MapControllerRoute(
    name: "doctor",
    pattern: "{controller=Doctor}/{action=Profile}/{id?}");

app.MapControllerRoute(
    name: "patient",
    pattern: "{controller=Patient}/{action=Profile}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

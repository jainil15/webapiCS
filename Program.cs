using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using webapi.Data;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using webapi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Newtonsoft.Json;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



//mssql db context
builder.Services.AddDbContext<AlbumDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AlbumDbContext2")));

//google drive
builder.Services.AddSingleton(sp => DriverServiceConfigurations.GetDriveService());

builder.Services.AddHttpClient("GoogleDriveClient");
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/api/login";
        // for 401 unauth
        options.ExpireTimeSpan = TimeSpan.FromDays(100);
        options.Cookie.MaxAge = options.ExpireTimeSpan;
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = 401;
            context.Response.WriteAsJsonAsync(new { error = "Unauthorized" });
            return Task.CompletedTask;
        };

    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// test this
app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHttpsRedirection();

// correct order ->
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();

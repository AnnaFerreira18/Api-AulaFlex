using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Repository;
using Infrastructure.Transaction;
using Microsoft.EntityFrameworkCore;
using Domain.Repositories;
using System.Text;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;


var builder = WebApplication.CreateBuilder(args);
var key = Encoding.ASCII.GetBytes("36B7247D-535D-4D46-8AA1-EBC5A01A696B");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAula, AulaRepository>();
builder.Services.AddScoped<IHorario, HorarioRepository>();
builder.Services.AddScoped<IColaborador, ColaboradorRepository>();
builder.Services.AddScoped<IInscricao, InscricaoRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("CorsPolicy", builder => builder.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader());
//});

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });


builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = "644409398312-dc8lc7ohd37uks7dov7kkr3pfdgueuqn.apps.googleusercontent.com";
        options.ClientSecret = "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";
        // options.CallbackPath = "/login-google";
    })
    .AddFacebook(options =>
    {
        options.AppId = "1042478310977057";
        options.AppSecret = "9d53869d661f8cf15521f5b245ed31b7";
        //options.CallbackPath = "/login-facebook";
    });

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = FacebookDefaults.AuthenticationScheme;
//})
//.AddCookie();


builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseCors("AllowAll");

//app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();

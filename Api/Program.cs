using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Repository;
using Infrastructure.Transaction;
using Microsoft.EntityFrameworkCore;
using Domain.Repositories;
using System.Text;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.IdentityModel.Tokens;                                                                                             

//using Autofac;
//using Autofac.Integration.WebApi;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Serialization;
//using Microsoft.Owin.Security.OAuth;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.Owin.Security.Jwt;
//using Microsoft.Owin.Security;


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
        options.ClientId = "SEU_CLIENT_ID_GOOGLE";
        options.ClientSecret = "SEU_CLIENT_SECRET_GOOGLE";
    })
    .AddFacebook(options =>
    {
        options.AppId = "SEU_APP_ID_FACEBOOK";
        options.AppSecret = "SEU_APP_SECRET_FACEBOOK";
    });


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

app.UseAuthorization();

app.MapControllers();

app.Run();


//OAuthAuthorizationServerOptions oAuthServerOptions = new OAuthAuthorizationServerOptions()
//{
//    AllowInsecureHttp = true,//Permite conexão não seguras http
//    TokenEndpointPath = new PathString("/api/v1/security/token"),//Local de onde o token vai ser disponibilizado
//    AccessTokenExpireTimeSpan = TimeSpan.FromHours(2),
//};

//// Geraçaõ do Token
//app.UseOAuthAuthorizationServer(oAuthServerOptions);
//app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Repository;
using Infrastructure.Transaction;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// Registrar repositórios (se necessário)
builder.Services.AddScoped<IAula, AulaRepository>();
builder.Services.AddScoped<IHorario, HorarioRepository>();
builder.Services.AddScoped<IColaborador, ColaboradorRepository>();
builder.Services.AddScoped<IInscricao, InscricaoRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

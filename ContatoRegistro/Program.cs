using ContatoRegistro.Aplication.DTOs;
using ContatoRegistro.Aplication.Service.Interfaces;
using ContatoRegistro.Aplication.Service.UseCases;
using ContatoRegistro.Aplication.Validation;
using ContatoRegistro.Doninio.RepositorioGateway;
using ContatoRegistro.Infra.Persistencia.Repository;
using ContatoRegistro.WEB.Presenters;
using ContatoRegistro.WEB.Presenters.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// WEB = Controller Clean Arquteture (MVC nesse caso do CRUD) + Presenter + Swegger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IContatoPresenter, ContatoPresenter>();

//// Infra = Data (Dapper + Postgres) + Repository
//builder.Services.AddScoped<IDbConnection>(sp =>
//{
//    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//    return new NpgsqlConnection(connectionString);
//});

// Aplication = Service + DTO + Validation + useCases (services)
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped<IValidator<CriarContatoDTO>, CriarContatoDtoValidator>();
builder.Services.AddScoped<IValidator<AtualizarContatoDTO>, AtualizarContatoDtoValidator>();
builder.Services.AddScoped<IContatoServico, ContatoServico>();

// Dominio = Entidades + ObjetosValor + RepositorioGateway (interfaces)
builder.Services.AddScoped<IContatoRepository, ContatoRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();

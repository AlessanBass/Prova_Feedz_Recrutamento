using Prova;
using Prova.Repositories;
using Prova.Services;
using SharpKml.Dom;
using System.Runtime.ConstrainedExecution;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
    options.Filters.Add<CustomExceptionFilter>()
);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configuration = builder.Configuration;
var filePath = configuration["FilePath"];

// Configurar AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddSingleton(_ => new PlacemarkRepository(filePath));

builder.Services.AddScoped<PlacemarkService>();

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

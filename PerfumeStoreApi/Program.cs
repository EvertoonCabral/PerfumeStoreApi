using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using PerfumeStoreApi.Context;
using AutoMapper;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add services to the container.

// Carrega a connection string antes de usar
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registra o AppDbContext ANTES do builder.Build()
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseLazyLoadingProxies().UseSqlServer(connectionString);
});

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions
        .ReferenceHandler = ReferenceHandler.IgnoreCycles);


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
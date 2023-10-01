using System;
using System.Net.Http;
using Broker.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//HTTP Client Projekter
#if DEBUG //Development
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:5001") });
#else //Production
    builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:5002") });
#endif

builder.Services.AddScoped<IProjektService, ProjektService>();
builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 443;
    options.HttpsPort = 7103;
});

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

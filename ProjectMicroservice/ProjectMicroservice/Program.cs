using System.Text.Json.Serialization;
using ProjectMicroservice.Data;
using ProjectMicroservice.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register the MongoDbContext
builder.Services.AddSingleton<MongoDbContext>(sp =>
    new MongoDbContext(
        builder.Configuration.GetConnectionString("MongoDb"),
        "test_db"
    )
);
builder.Services.AddMvc()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Registering the IProjectService and IBacklogService with their concrete implementations.
builder.Services.AddScoped<IProjectService, ProjectService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder
            .AllowAnyOrigin() // Specify the allowed origins
            .AllowAnyMethod() // Allow any HTTP method
            .AllowAnyHeader(); // Allow any HTTP headers
    });
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
app.UseCors();

app.Run();

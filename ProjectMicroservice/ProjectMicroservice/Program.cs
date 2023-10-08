using ProjectMicroservice.Data;
using ProjectMicroservice.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

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

// Registering the IProjectService and IBacklogService with their concrete implementations.
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IBacklogService, BacklogService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

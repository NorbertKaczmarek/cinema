using cinema.context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policyBuilder =>
    {
        policyBuilder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
var connectionString = configuration.GetConnectionString("MySql")!;
builder.Services.AddDbContext<CinemaDbContext>
    (options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

// Migrate database
app.Services.CreateScope().ServiceProvider.GetRequiredService<CinemaDbContext>().UpdateDatabase();

// Seed database
app.Services.CreateScope().ServiceProvider.GetRequiredService<CinemaDbContext>().SeedDatabase();

app.MapGet("/api/", () => "Hello World!");
app.MapGet("/api/2/", () => "Hello World! 2");
app.MapGet("/api/3/", () => "Hello World! 3");

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { } // used by Unit Testing

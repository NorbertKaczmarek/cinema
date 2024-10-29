using cinema.context;
using cinema.context.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var authenticationOptions = configuration.GetSection(nameof(cinema.api.AuthenticationOptions)).Get<cinema.api.AuthenticationOptions>()!;
builder.Services.AddSingleton(authenticationOptions);

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

// Authorization, Authentication (JWT)
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => {
    options.IncludeErrorDetails = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = authenticationOptions.ValidIssuer,
        ValidAudience = authenticationOptions.ValidAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationOptions.IssuerSigningKey))
    };
});

// Add services to the container.

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Seeder
builder.Services.AddScoped<Seeder>();

// Database
var connectionString = configuration.GetConnectionString("MySql")!;
builder.Services.AddDbContext<CinemaDbContext>
    (options => options.UseMySql(connectionString, ServerVersion.Parse("8.0.40-mysql")));

var app = builder.Build();
var scope = app.Services.CreateScope();

// Migrate database
scope.ServiceProvider.GetRequiredService<CinemaDbContext>().UpdateDatabase();

// Seed database
scope.ServiceProvider.GetRequiredService<Seeder>().SeedDatabase();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { } // used for Testing

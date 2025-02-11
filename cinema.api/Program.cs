using cinema.api.Helpers;
using cinema.api.Helpers.EmailSender;
using cinema.context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var authenticationOptions = configuration.GetSection(nameof(AuthenticationOptions)).Get<AuthenticationOptions>()!;
builder.Services.AddSingleton(authenticationOptions);

var emailOptions = configuration.GetSection(nameof(cinema.api.EmailOptions)).Get<cinema.api.EmailOptions>()!;
builder.Services.AddSingleton(emailOptions);

var seedingOptions = configuration.GetSection(nameof(SeedingOptions)).Get<SeedingOptions>()!;

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
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.IncludeErrorDetails = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = authenticationOptions.ValidIssuer,
        ValidAudience = authenticationOptions.ValidAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationOptions.IssuerSigningKey)),
        RoleClaimType = "Role"
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireRole("Admin"));
    options.AddPolicy("UserPolicy", policy =>
        policy.RequireRole("User"));
});

// Interceptors
builder.Services.AddSingleton<UpdateAuditableEntitiesInterceptor>();

// Add services to the container.
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.DescribeAllParametersInCamelCase();
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
        $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Cinema API",
        Description = "An ASP.NET Core Web API for managing a small cienema.",
    });

    options.SwaggerDoc("Admin", new OpenApiInfo { Title = "Admin API", Version = "v1" });
    options.SwaggerDoc("User", new OpenApiInfo { Title = "User API", Version = "v1" });
});

// EmailSender
builder.Services.AddTransient<IEmailSender, EmailSender>();

// Seeder
builder.Services.AddScoped<Seeder>();

// Database
var connectionString = configuration.GetConnectionString("MySql")!;
builder.Services.AddDbContext<CinemaDbContext>(
    (sp, options) =>
    {
        var auditableInterceptor = sp.GetService<UpdateAuditableEntitiesInterceptor>();
        options
            .UseMySql(connectionString, ServerVersion.Parse("8.0.40-mysql"))
            .AddInterceptors(auditableInterceptor!);
    });

// Automapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();
var scope = app.Services.CreateScope();

// Migrate database
scope.ServiceProvider.GetRequiredService<CinemaDbContext>().UpdateDatabase();

// Seed database
var seeder = scope.ServiceProvider.GetRequiredService<Seeder>();
if (seedingOptions.SeedUsersAndSeats == "true") seeder.SeedUsersAndSeats();
if (seedingOptions.SeedDatabase == "true") seeder.SeedDatabase();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/Admin/swagger.json", "Admin API");
    c.SwaggerEndpoint("/swagger/User/swagger.json", "User API");
});

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { } // used for Testing

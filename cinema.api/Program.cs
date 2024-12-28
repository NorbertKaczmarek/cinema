using cinema.api.Helpers;
using cinema.api.Helpers.EmailSender;
using cinema.context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var authenticationOptions = configuration.GetSection(nameof(cinema.api.AuthenticationOptions)).Get<cinema.api.AuthenticationOptions>()!;
builder.Services.AddSingleton(authenticationOptions);

var emailOptions = configuration.GetSection(nameof(cinema.api.EmailOptions)).Get<cinema.api.EmailOptions>()!;
builder.Services.AddSingleton(emailOptions);

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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationOptions.IssuerSigningKey))
    };
});

// Interceptors
builder.Services.AddSingleton<UpdateAuditableEntitiesInterceptor>();

// Add services to the container.
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers();

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
scope.ServiceProvider.GetRequiredService<Seeder>().SeedDatabase();

app.UseSwagger();
//app.UseSwaggerUI();
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

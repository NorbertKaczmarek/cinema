using cinema.api.Models;
using cinema.context;
using cinema.context.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace cinema.tests;

public class AuthControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly IServiceScope _scope;
    private readonly IServiceProvider _scopedServices;
    private readonly CinemaDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AuthControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
        _scope = factory.Services.CreateScope();
        _scopedServices = _scope.ServiceProvider;
        _context = _scopedServices.GetRequiredService<CinemaDbContext>();
        _passwordHasher = _scopedServices.GetRequiredService<IPasswordHasher<User>>();
    }

    private async Task<Guid> seedUser(UserCreateDto dto)
    {
        User newUser = new User
        {
            IsAdmin = dto.IsAdmin,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            PasswordHash = ""
        };
        var hashedPasword = _passwordHasher.HashPassword(newUser, dto.Password);
        newUser.PasswordHash = hashedPasword;

        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();

        return newUser.Id;
    }

    private async Task<string> getValidToken(bool isAdmin = false)
    {
        string email = "admin@test.com";
        string password = "AdminPassword123";

        User newUser = new User
        {
            IsAdmin = isAdmin,
            Email = email,
            FirstName = "Admin",
            LastName = "Test",
            PasswordHash = ""
        };
        var hashedPasword = _passwordHasher.HashPassword(newUser, password);
        newUser.PasswordHash = hashedPasword;

        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();

        var loginDto = new UserLoginDto
        {
            Email = email,
            Password = password
        };
        var loginContent = HttpContentHelper.ToJsonHttpContent(loginDto);

        var response = await _client.PostAsync("/api/Auth/login", loginContent);
        var token = await response.Content.ReadAsStringAsync();

        return token;
    }

    [Theory]
    [InlineData(false, "validuser@example.com", "ValidPassword123!", "Test", "User")]
    public async Task Login_WithValidCredentials_ReturnsToken(bool isAdmin, string email, string firstName, string LastName, string password)
    {
        // Arrange
        var userCreateDto = new UserCreateDto
        {
            IsAdmin = isAdmin,
            Email = email,
            FirstName = firstName,
            LastName = LastName,
            Password = password,
            ConfirmPassword = password,
        };
        var userId = await seedUser(userCreateDto);

        var loginDto = new UserLoginDto
        {
            Email = email,
            Password = password
        };
        var loginContent = HttpContentHelper.ToJsonHttpContent(loginDto);

        // Act
        var response = await _client.PostAsync("/api/Auth/login", loginContent);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var token = await response.Content.ReadAsStringAsync();
        token.Should().NotBeNullOrEmpty();
    }

    [Theory]
    [InlineData(false, "test4@example.com", "Password123!", "Bob", "Brown")]
    public async Task Login_WithInvalidCredentials_ReturnsBadRequest(bool isAdmin, string email, string firstName, string LastName, string password)
    {
        // Arrange
        var userCreateDto = new UserCreateDto
        {
            IsAdmin = isAdmin,
            Email = email,
            FirstName = firstName,
            LastName = LastName,
            Password = password,
            ConfirmPassword = password,
        };
        var userId = await seedUser(userCreateDto);
        var content = HttpContentHelper.ToJsonHttpContent(userCreateDto);

        var loginDto = new UserLoginDto
        {
            Email = email,
            Password = "WrongPassword!"
        };
        var loginContent = HttpContentHelper.ToJsonHttpContent(loginDto);

        // Act
        var response = await _client.PostAsync("/api/Auth/login", loginContent);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }
}

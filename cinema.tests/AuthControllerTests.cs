using cinema.api.Helpers;
using cinema.api.Models;
using cinema.api.Models.Admin;
using cinema.context;
using cinema.context.Entities;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace cinema.tests;

public class AuthControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly IServiceScope _scope;
    private readonly IServiceProvider _scopedServices;
    private readonly CinemaDbContext _context;
    private readonly string _endpoint = "/api/auth";

    public AuthControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
        _scope = factory.Services.CreateScope();
        _scopedServices = _scope.ServiceProvider;
        _context = _scopedServices.GetRequiredService<CinemaDbContext>();
    }

    private async Task<(Guid, string)> seedUser(UserCreateDto dto)
    {
        string password = "test1";
        (var saltText, var saltedHashedPassword) = SalterAndHasher.getSaltAndSaltedHashedPassword(password);

        User newUser = new User
        {
            IsAdmin = false,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Salt = saltText,
            SaltedHashedPassword = saltedHashedPassword,
        };

        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();

        return (newUser.Id, password);
    }

    private async Task<string> getValidToken(bool isAdmin = false)
    {
        string email = "admin@test.com";
        string password = "AdminPassword123";

        (var saltText, var saltedHashedPassword) = SalterAndHasher.getSaltAndSaltedHashedPassword(password);

        User newUser = new User
        {
            IsAdmin = isAdmin,
            Email = email,
            FirstName = "Admin",
            LastName = "Test",
            Salt = saltText,
            SaltedHashedPassword = saltedHashedPassword,
        };

        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();

        var loginDto = new UserLoginDto
        {
            Email = email,
            Password = password
        };
        var loginContent = HttpContentHelper.ToJsonHttpContent(loginDto);

        var response = await _client.PostAsync($"{_endpoint}/login", loginContent);
        var token = await response.Content.ReadAsStringAsync();

        return token;
    }

    [Theory]
    [InlineData("validuser@example.com", "ValidPassword123!", "Test")]
    public async Task Login_WithValidCredentials_ReturnsToken(string email, string firstName, string LastName)
    {
        // Arrange
        var userCreateDto = new UserCreateDto
        {
            Email = email,
            FirstName = firstName,
            LastName = LastName
        };
        var (userId, password) = await seedUser(userCreateDto);

        var loginDto = new UserLoginDto
        {
            Email = email,
            Password = password
        };
        var loginContent = HttpContentHelper.ToJsonHttpContent(loginDto);

        // Act
        var response = await _client.PostAsync($"{_endpoint}/login", loginContent);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var token = await response.Content.ReadAsStringAsync();
        token.Should().NotBeNullOrEmpty();
    }

    [Theory]
    [InlineData("test4@example.com", "Password123!", "Bob")]
    public async Task Login_WithInvalidCredentials_ReturnsBadRequest(string email, string firstName, string LastName)
    {
        // Arrange
        var userCreateDto = new UserCreateDto
        {
            Email = email,
            FirstName = firstName,
            LastName = LastName
        };
        var (userId, password2) = await seedUser(userCreateDto);
        var content = HttpContentHelper.ToJsonHttpContent(userCreateDto);

        var loginDto = new UserLoginDto
        {
            Email = email,
            Password = "WrongPassword!"
        };
        var loginContent = HttpContentHelper.ToJsonHttpContent(loginDto);

        // Act
        var response = await _client.PostAsync($"{_endpoint}/login", loginContent);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }
}

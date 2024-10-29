using cinema.context;
using cinema.context.Entities;
using FluentAssertions;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Microsoft.EntityFrameworkCore;
using cinema.api.Models;
using Microsoft.AspNetCore.Identity;

namespace cinema.tests;

public class UsersControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly IServiceScope _scope;
    private readonly IServiceProvider _scopedServices;
    private readonly CinemaDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly string _endpoint = "/api/admin/users";

    public UsersControllerTests(CustomWebApplicationFactory<Program> factory)
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

    [Theory]
    [InlineData("/api/admin/users")]
    public async Task Get_Users_ReturnsUsers(string url)
    {
        var response = await _client.GetAsync(url);

        response.EnsureSuccessStatusCode();
    }
}

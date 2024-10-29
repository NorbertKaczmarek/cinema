using cinema.context;
using cinema.context.Entities;
using Microsoft.Extensions.DependencyInjection;
using cinema.api.Models;
using Microsoft.AspNetCore.Identity;
using cinema.api.Helpers;

namespace cinema.tests;

public class UsersControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly IServiceScope _scope;
    private readonly IServiceProvider _scopedServices;
    private readonly CinemaDbContext _context;
    //private readonly string _endpoint = "/api/admin/users";

    public UsersControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
        _scope = factory.Services.CreateScope();
        _scopedServices = _scope.ServiceProvider;
        _context = _scopedServices.GetRequiredService<CinemaDbContext>();
    }

    private async Task<Guid> seedUser(UserCreateDto dto)
    {
        (var saltText, var saltedHashedPassword) = SalterAndHasher.getSaltAndSaltedHashedPassword(dto.Password);

        User newUser = new User
        {
            IsAdmin = dto.IsAdmin,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Salt = saltText,
            SaltedHashedPassword = saltedHashedPassword,
        };
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

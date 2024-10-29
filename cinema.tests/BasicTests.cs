using Microsoft.AspNetCore.Mvc.Testing;

namespace cinema.tests;

public class BasicTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public BasicTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Theory]
    [InlineData("/swagger")]
    [InlineData("/api/admin/categories")]
    [InlineData("/api/admin/movies")]
    [InlineData("/api/admin/orders")]
    [InlineData("/api/admin/screenings")]
    [InlineData("/api/admin/seats")]
    public async Task Get_Endpoints_ReturnsSuccessStatusCode(string url)
    {
        var response = await _client.GetAsync(url);

        response.EnsureSuccessStatusCode();
    }
}

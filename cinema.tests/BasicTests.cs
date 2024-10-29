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
    [InlineData("/api/categories")]
    [InlineData("/api/movies")]
    [InlineData("/api/orders")]
    [InlineData("/api/screenings")]
    [InlineData("/api/seats")]
    public async Task Get_Endpoints_ReturnsSuccessStatusCode(string url)
    {
        var response = await _client.GetAsync(url);

        response.EnsureSuccessStatusCode();
    }
}

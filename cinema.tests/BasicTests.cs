using Microsoft.AspNetCore.Mvc.Testing;

namespace cinema.tests;

public class BasicTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public BasicTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Theory]
    [InlineData("/swagger")]
    [InlineData("/api/categories")]
    [InlineData("/api/Movies")]
    [InlineData("/api/Orders")]
    [InlineData("/api/Screenings")]
    [InlineData("/api/Seats")]
    public async Task Get_Endpoints_ReturnsSuccessStatusCode(string url)
    {
        var response = await _client.GetAsync(url);

        response.EnsureSuccessStatusCode();
    }
}

using Microsoft.AspNetCore.Mvc.Testing;

namespace cinema.tests;

public class CategoriesControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public CategoriesControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public void Test1()
    {

    }
}

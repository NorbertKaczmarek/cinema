using cinema.context;
using cinema.context.Entities;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace cinema.tests;

public class CategoriesControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly IServiceScope _scope;
    private readonly IServiceProvider _scopedServices;
    private readonly CinemaDbContext _context;
    private readonly string _endpoint = "/api/admin/categories";

    public CategoriesControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
        _scope = factory.Services.CreateScope();
        _scopedServices = _scope.ServiceProvider;
        _context = _scopedServices.GetRequiredService<CinemaDbContext>();
    }

    private async Task<Guid> seedCategory(Category category)
    {
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
        return category.Id;
    }

    [Theory]
    [InlineData("/api/admin/categories")]
    public async Task Get_Categories_ReturnsCategories(string url)
    {
        var response = await _client.GetAsync(url);

        response.EnsureSuccessStatusCode();
    }

    [Theory]
    [InlineData("test1")]
    [InlineData("test2")]
    [InlineData("test3")]
    public async Task GetCategory_WithValidId_ReturnsOK(string name)
    {
        // Arrange
        var category = new Category() { Name = name };
        var categoryId = await seedCategory(category);

        // Act
        var response = await _client.GetAsync($"{_endpoint}/{categoryId}");
        var responseBody = await response.Content.ReadAsStringAsync();
        var retrievedCategory = await _context.Categories.FirstOrDefaultAsync(x => x.Id == categoryId);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        retrievedCategory.Should().NotBeNull();
        retrievedCategory!.Id.Should().Be(categoryId);
        retrievedCategory.Name.Should().Be(name);
    }

    [Theory]
    [InlineData("test21")]
    [InlineData("test22")]
    [InlineData("test23")]
    public async Task CreateCategory_WithValidModel_ReturnsCreatedStatus(string categoryName)
    {
        // Arrange
        var content = HttpContentHelper.ToJsonHttpContent(categoryName);

        // Act
        var response = await _client.PostAsync(_endpoint, content);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
    }

    [Theory]
    [InlineData("test31")]
    [InlineData("test32")]
    [InlineData("test33")]
    public async Task DeleteCategory_WithValidModel_ReturnsCreatedStatus(string categoryName)
    {
        // Arrange
        var category = new Category() { Name = categoryName };
        var categoryId = await seedCategory(category);

        // Act
        var response = await _client.DeleteAsync($"{_endpoint}/{categoryId}");
        var deletedCategory = await _context.Categories.FirstOrDefaultAsync(x => x.Id == categoryId);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        deletedCategory.Should().BeNull();
    }

    [Theory]
    [InlineData("test41")]
    [InlineData("test42")]
    [InlineData("test43")]
    public async Task CreateAndDeleteCategory_WithValidModel_ReturnsCreatedStatus(string categoryName)
    {
        // Arrange
        var content = HttpContentHelper.ToJsonHttpContent(categoryName);

        // Act
        var responseCreated = await _client.PostAsync(_endpoint, content);
        var location = responseCreated.Headers.Location;
        var responseDeleted = await _client.DeleteAsync(location);
        var deletedCategory = await _context.Categories.FirstOrDefaultAsync(x => x.Name == categoryName);

        // Asert
        responseCreated.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        location.Should().NotBeNull();
        responseDeleted.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        deletedCategory.Should().BeNull();
    }

    [Fact]
    public async Task CreateExistingCategory_WithValidModel_ReturnsBadRequest()
    {
        // Arrange
        var categoryName = "test51";
        var category = new Category() { Name = categoryName };
        var categoryId = await seedCategory(category);
        var content = HttpContentHelper.ToJsonHttpContent(categoryName);

        // Act
        var responseCreated = await _client.PostAsync(_endpoint, content);

        // Asert
        responseCreated.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }
}

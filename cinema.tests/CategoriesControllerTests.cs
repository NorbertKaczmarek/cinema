using cinema.context;
using cinema.context.Entities;
using FluentAssertions;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace cinema.tests;

public class CategoriesControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly IServiceScope _scope;
    private readonly IServiceProvider _scopedServices;
    private readonly CinemaDbContext _context;

    public CategoriesControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _scope = _factory.Services.CreateScope();
        _scopedServices = _scope.ServiceProvider;
        _context = _scopedServices.GetRequiredService<CinemaDbContext>();
    }

    private static HttpContent serializeObject(object obj)
    {
        return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
    }

    [Theory]
    [InlineData("/api/Categories")]
    public async Task Get_Categories_ReturnsCategories(string url)
    {
        var response = await _client.GetAsync(url);

        response.EnsureSuccessStatusCode();
    }

    private async Task<Guid> seedCategory(Category category)
    {
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
        return category.Id;
    }

    [Theory]
    [InlineData("test1")]
    [InlineData("test2")]
    [InlineData("test3")]
    public async Task GetCategory_WithValidId_ReturnsOK(string name)
    {
        // arange
        var category = new Category() { Name = name };
        var categoryId = await seedCategory(category);

        // act
        var response = await _client.GetAsync("/api/Categories/" + categoryId);
        var responseBody = await response.Content.ReadAsStringAsync();
        var retrievedCategory = await _context.Categories.FirstOrDefaultAsync(x => x.Id == categoryId);

        // asert
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
        // arange
        HttpContent content = serializeObject(categoryName);

        // act
        var response = await _client.PostAsync("/api/Categories", content);

        // asert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
    }

    [Theory]
    [InlineData("test31")]
    [InlineData("test32")]
    [InlineData("test33")]
    public async Task DeleteCategory_WithValidModel_ReturnsCreatedStatus(string categoryName)
    {
        // arange
        var category = new Category() { Name = categoryName };
        var categoryId = await seedCategory(category);

        // act
        var response = await _client.DeleteAsync("/api/Categories/" + categoryId);
        var deletedCategory = await _context.Categories.FirstOrDefaultAsync(x => x.Id == categoryId);

        // asert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        deletedCategory.Should().BeNull();
    }

    [Theory]
    [InlineData("test41")]
    [InlineData("test42")]
    [InlineData("test43")]
    public async Task CreateAndDeleteCategory_WithValidModel_ReturnsCreatedStatus(string categoryName)
    {
        // arange
        HttpContent content = serializeObject(categoryName);

        // act
        var responseCreated = await _client.PostAsync("/api/Categories", content);
        var location = responseCreated.Headers.Location;
        var responseDeleted = await _client.DeleteAsync(location);
        var deletedCategory = await _context.Categories.FirstOrDefaultAsync(x => x.Name == categoryName);

        // asert
        responseCreated.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        location.Should().NotBeNull();
        responseDeleted.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        deletedCategory.Should().BeNull();
    }

    //[Theory]
    //public async Task CreateCategory_WithValidModel_ReturnsCreatedStatus()
    //{
    //    // arange

    //    // act

    //    // asert
    //}
}

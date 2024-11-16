using cinema.context;
using cinema.context.Entities;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using cinema.api.Models;

namespace cinema.tests;

public class MoviesControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly IServiceScope _scope;
    private readonly IServiceProvider _scopedServices;
    private readonly CinemaDbContext _context;
    private readonly string _endpoint = "/api/admin/movies";

    public MoviesControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
        _scope = factory.Services.CreateScope();
        _scopedServices = _scope.ServiceProvider;
        _context = _scopedServices.GetRequiredService<CinemaDbContext>();
    }

    private async Task<Category> seedCategory(string categoryName)
    {
        var category = new Category() { Name = categoryName };
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
        return category;
    }

    private async Task<Guid> seedMovie(Movie movie)
    {
        await _context.Movies.AddAsync(movie);
        await _context.SaveChangesAsync();
        return movie.Id;
    }

    [Theory]
    [InlineData("/api/admin/movies")]
    public async Task Get_Categories_ReturnsCategories(string url)
    {
        var response = await _client.GetAsync(url);

        response.EnsureSuccessStatusCode();
    }

    [Theory]
    [InlineData("Tytul11", 1, 30, "posterUrl", "Director11", "Cast11", "Description11", 4.6, "Category11")]
    [InlineData("Tytul12", 2, 30, "posterUrl", "Director12", "Cast12", "Description12", 8.1, "Category12")]
    public async Task GetMovie_WithValidId_ReturnsOK(
        string title, 
        int durationHours,
        int durationMinutes,
        string posterUrl, 
        string director, 
        string cast, 
        string description, 
        double rating, 
        string categoryName)
    {
        // Arrange
        var category = await seedCategory(categoryName);
        var movie = new Movie()
        {
            Title = title,
            DurationMinutes = durationHours*60 + durationMinutes,
            PosterUrl = posterUrl,
            Director = director,
            Cast = cast,
            Description = description,
            Rating = rating,
            Category = category
        };
        var movieId = await seedMovie(movie);

        // Act
        var response = await _client.GetAsync($"{_endpoint}/{movieId}");
        var responseBody = await response.Content.ReadAsStringAsync();
        var retrievedMovie = await _context.Movies.FirstOrDefaultAsync(x => x.Id == movieId);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        retrievedMovie.Should().NotBeNull();
        retrievedMovie!.Id.Should().Be(movieId);
        retrievedMovie.Title.Should().Be(title);
        retrievedMovie.Description.Should().Be(description);
        retrievedMovie.Rating.Should().Be(rating);
    }

    [Theory]
    [InlineData("Tytul21", 1, 30, "posterUrl", "Director21", "Cast21", "Description21", 4.6, "Category21")]
    [InlineData("Tytul22", 2, 30, "posterUrl", "Director22", "Cast22", "Description22", 8.1, "Category22")]
    public async Task CreateMovie_WithValidModel_ReturnsCreatedStatus(
        string title,
        int durationHours,
        int durationMinutes,
        string posterUrl,
        string director,
        string cast,
        string description,
        double rating,
        string categoryName)
    {
        // Arrange
        var category = await seedCategory(categoryName);
        var movie = new MovieCreateDto()
        {
            Title = title,
            DurationMinutes = durationHours * 60 + durationMinutes,
            PosterUrl = posterUrl,
            Director = director,
            Cast = cast,
            Description = description,
            Rating = rating,
            CategoryName = categoryName
        };
        var content = HttpContentHelper.ToJsonHttpContent(movie);

        // Act
        var response = await _client.PostAsync(_endpoint, content);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
    }

    [Theory]
    [InlineData("Tytul31", 1, 30, "posterUrl", "Director31", "Cast31", "Description31", 4.6, "Category31")]
    [InlineData("Tytul32", 2, 30, "posterUrl", "Director32", "Cast32", "Description32", 8.1, "Category32")]
    public async Task DeleteMovie_WithValidModel_ReturnsCreatedStatus(
        string title,
        int durationHours,
        int durationMinutes,
        string posterUrl,
        string director,
        string cast,
        string description,
        double rating,
        string categoryName)
    {
        // Arrange
        var category = await seedCategory(categoryName);
        var movie = new Movie()
        {
            Title = title,
            DurationMinutes = durationHours * 60 + durationMinutes,
            PosterUrl = posterUrl,
            Director = director,
            Cast = cast,
            Description = description,
            Rating = rating,
            Category = category
        };
        var movieId = await seedMovie(movie);

        // Act
        var response = await _client.DeleteAsync($"{_endpoint}/{movieId}");
        var deletedMovie = await _context.Movies.FirstOrDefaultAsync(x => x.Id == movieId);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        deletedMovie.Should().BeNull();
    }

    [Theory]
    [InlineData("Tytul41", 1, 30, "posterUrl", "Director41", "Cast41", "Description41", 4.6, "Category41")]
    [InlineData("Tytul42", 2, 30, "posterUrl", "Director42", "Cast42", "Description42", 8.1, "Category42")]
    public async Task CreateAndDeleteMovie_WithValidModel_ReturnsCreatedStatus(
        string title,
        int durationHours,
        int durationMinutes,
        string posterUrl,
        string director,
        string cast,
        string description,
        double rating,
        string categoryName)
    {
        // Arrange
        var category = await seedCategory(categoryName);
        var movie = new MovieCreateDto()
        {
            Title = title,
            DurationMinutes = durationHours * 60 + durationMinutes,
            PosterUrl = posterUrl,
            Director = director,
            Cast = cast,
            Description = description,
            Rating = rating,
            CategoryName = categoryName
        };
        var content = HttpContentHelper.ToJsonHttpContent(movie);

        // Act
        var responseCreated = await _client.PostAsync(_endpoint, content);
        var location = responseCreated.Headers.Location;
        var responseDeleted = await _client.DeleteAsync(location);
        var deletedMovie = await _context.Movies.FirstOrDefaultAsync(x => x.Title == title);

        // Asert
        responseCreated.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        location.Should().NotBeNull();
        responseDeleted.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        deletedMovie.Should().BeNull();
    }
}

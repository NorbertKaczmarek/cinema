using cinema.context.Entities;
using cinema.context;
using Microsoft.EntityFrameworkCore;
using cinema.api.Controllers.Admin;
using cinema.api.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace cinema.tests.Controllers;

public class MoviesControllerTests
{
    private CinemaDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<CinemaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new CinemaDbContext(options);

        context.Categories.AddRange(new List<Category>
            {
                new Category { Id = Guid.NewGuid(), Name = "Action" },
                new Category { Id = Guid.NewGuid(), Name = "Drama" },
                new Category { Id = Guid.NewGuid(), Name = "Comedy" }
            });
        context.SaveChanges();

        context.Movies.AddRange(new List<Movie>
        {
            new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Movie 1",
                DurationMinutes = 111,
                PosterUrl = "https://movie1.jpg",
                TrailerUrl = "https://movie1.jpg",
                BackgroundUrl = "https://movie1.jpg",
                Director = "Director 1",
                Cast = "Cast 1",
                Description = "Description 1",
                Rating = 1.1,
                Category = context.Categories.FirstOrDefault(x => x.Name == "Dramat")
            },
            new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Movie 2",
                DurationMinutes = 222,
                PosterUrl = "https://movie2.jpg",
                TrailerUrl = "https://movie2.jpg",
                BackgroundUrl = "https://movie2.jpg",
                Director = "Director 2",
                Cast = "Cast 2",
                Description = "Description 2",
                Rating = 2.2,
                Category = context.Categories.FirstOrDefault(x => x.Name == "Action")
            },
            new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Movie 3",
                DurationMinutes = 333,
                PosterUrl = "https://movie3.jpg",
                TrailerUrl = "https://movie3.jpg",
                BackgroundUrl = "https://movie1.jpg",
                Director = "Director 3",
                Cast = "Cast 3",
                Description = "Description 3",
                Rating = 3.3,
                Category = context.Categories.FirstOrDefault(x => x.Name == "Action")
            },
        });
        context.SaveChanges();

        return context;
    }

    [Fact]
    public void Get_ReturnsAllSeats()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new MoviesController(context);

        // Act
        var result = controller.Get(new PageQuery());

        // Assert
        result.Should().NotBeNull();
        result.Content.Count().Should().Be(3);
        result.TotalElements.Should().Be(3);
    }

    [Fact]
    public void Get_WithValidId_ReturnsSeat()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new MoviesController(context);
        var movieId = context.Movies.First(m => m.Title == "Movie 1").Id;

        // Act
        var result = controller.Get(movieId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeOfType<Movie>();
        var movie = okResult.Value as Movie;
        movie!.Title.Should().Be("Movie 1");
        movie!.Rating.Should().Be(1.1);
    }

    [Fact]
    public void Get_WithInvalidId_ReturnsNull()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new MoviesController(context);

        // Act
        var result = controller.Get(Guid.NewGuid());

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var badRequestResult = result as NotFoundObjectResult;
        badRequestResult!.Value.Should().Be("Movie with that id was not found.");
    }

    [Theory]
    [InlineData("New Movie 1", 120, "https://newmovie1.jpg", "https://newmovie1-trailer.jpg", "https://newmovie1-bg.jpg", "Director 1", "Cast 1", "Description 1", 4.5)]
    [InlineData("New Movie 2", 150, "https://newmovie2.jpg", "https://newmovie2-trailer.jpg", "https://newmovie2-bg.jpg", "Director 2", "Cast 2", "Description 2", 3.8)]
    [InlineData("New Movie 3", 90, "https://newmovie3.jpg", "https://newmovie3-trailer.jpg", "https://newmovie3-bg.jpg", "Director 3", "Cast 3", "Description 3", 2.5)]
    public void Post_CreateMovieWithValidModel_ReturnsCreatedStatus(
        string title,
        int durationMinutes,
        string posterUrl,
        string trailerUrl,
        string backgroundUrl,
        string director,
        string cast,
        string description,
        double rating)
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new MoviesController(context);
        var newMovieDto = new MovieCreateDto
        {
            Title = title,
            DurationMinutes = durationMinutes,
            PosterUrl = posterUrl,
            TrailerUrl = trailerUrl,
            BackgroundUrl = backgroundUrl,
            Director = director,
            Cast = cast,
            Description = description,
            Rating = rating,
            CategoryId = context.Categories.First().Id
        };

        // Act
        var result = controller.Post(newMovieDto);

        // Assert
        result.Should().BeOfType<CreatedResult>();
        context.Movies.Count().Should().Be(4);
    }

    [Fact]
    public void Put_EditWithValidModel_UpdatesExistingMovie()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new MoviesController(context);
        var existingMovie = context.Movies.First();
        var updatedMovieDto = new MovieCreateDto
        {
            Title = "Updated Movie",
            DurationMinutes = 130,
            PosterUrl = "https://updatedmovie.jpg",
            TrailerUrl = "https://updatedmovie-trailer.jpg",
            BackgroundUrl = "https://updatedmovie-bg.jpg",
            Director = "Updated Director",
            Cast = "Updated Cast",
            Description = "Updated Description",
            Rating = 5.0,
            CategoryId = (Guid)existingMovie.CategoryId!
        };

        // Act
        var result = controller.Put(existingMovie.Id, updatedMovieDto);

        // Assert
        result.Should().BeOfType<CreatedResult>();
        var updatedMovie = context.Movies.First(m => m.Id == existingMovie.Id);
        updatedMovie.Title.Should().Be("Updated Movie");
        updatedMovie.Rating.Should().Be(5.0);
    }

    [Fact]
    public void Put_EditWithInvalidModel_ReturnsNotFound()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new MoviesController(context);
        var updatedMovieDto = new MovieCreateDto
        {
            Title = "Nonexistent Movie",
            DurationMinutes = 130,
            PosterUrl = "https://nonexistentmovie.jpg",
            TrailerUrl = "https://nonexistentmovie-trailer.jpg",
            BackgroundUrl = "https://nonexistentmovie-bg.jpg",
            Director = "Nonexistent Director",
            Cast = "Nonexistent Cast",
            Description = "Nonexistent Description",
            Rating = 5.0,
            CategoryId = context.Categories.First().Id
        };

        // Act
        var result = controller.Put(Guid.NewGuid(), updatedMovieDto);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public void Delete_WithValidId_RemovesMovie()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new MoviesController(context);
        var movieToDelete = context.Movies.First();
        var initialCount = context.Movies.Count();

        // Act
        controller.Delete(movieToDelete.Id);

        // Assert
        context.Movies.Count().Should().Be(initialCount - 1);
        context.Movies.FirstOrDefault(m => m.Id == movieToDelete.Id).Should().BeNull();
    }

    [Fact]
    public void Delete_WithInvalidId_DoesNothing()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new MoviesController(context);
        var initialCount = context.Movies.Count();

        // Act
        controller.Delete(Guid.NewGuid());

        // Assert
        context.Movies.Count().Should().Be(initialCount);
    }
}

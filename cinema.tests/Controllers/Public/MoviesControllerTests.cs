using cinema.context.Entities;
using cinema.context;
using Microsoft.EntityFrameworkCore;
using cinema.api.Controllers.Admin;
using cinema.api.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Moq;
using cinema.api.Models.Admin;
using cinema.api.Helpers;
using cinema.api.Controllers.Public;
using cinema.api.Public;

namespace cinema.tests.Controllers.Public;

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

    private api.Controllers.Public.MoviesController CreateController(CinemaDbContext context)
    {
        var mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

        return new api.Controllers.Public.MoviesController(context, mapper);
    }

    [Fact]
    public void Get_ReturnsAllSeats()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = CreateController(context);

        // Act
        var result = controller.Get().Result as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        var movies = result!.Value as List<PublicMovieDto>;
        movies.Should().HaveCount(3);
    }

    [Fact]
    public void Get_WithValidId_ReturnsSeat()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = CreateController(context);
        var movieId = context.Movies.First(m => m.Title == "Movie 1").Id;

        // Act
        var result = controller.Get(movieId).Result as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        result!.Value.Should().BeOfType<PublicMovieDto>();
        var movie = result.Value as PublicMovieDto;
        movie!.Title.Should().Be("Movie 1");
        movie!.Rating.Should().Be(1.1);
    }

    [Fact]
    public void Get_WithInvalidId_ReturnsNull()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = CreateController(context);

        // Act
        var result = controller.Get(Guid.NewGuid()).Result as NotFoundObjectResult;

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        result!.Value.Should().Be("Movie with that id was not found.");
    }
}

using cinema.context.Entities;
using cinema.context;
using Microsoft.EntityFrameworkCore;
using cinema.api.Controllers.Admin;
using cinema.api.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace cinema.tests.Controllers;

public class ScreeningsControllerTests
{
    private CinemaDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<CinemaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new CinemaDbContext(options);

        var movie = new Movie
        {
            Id = Guid.NewGuid(),
            Title = "Test Movie",
            DurationMinutes = 120,
            PosterUrl = "https://testmovie.jpg",
            TrailerUrl = "https://testmovie-trailer.jpg",
            BackgroundUrl = "https://testmovie-bg.jpg",
            Director = "Test Director",
            Cast = "Test Cast",
            Description = "Test Description",
            Rating = 5.0,
            Category = new Category { Id = Guid.NewGuid(), Name = "Test Category" }
        };

        context.Movies.Add(movie);
        context.Screenings.AddRange(new List<Screening>
            {
                new Screening { Id = Guid.NewGuid(), StartDateTime = DateTime.Now, EndDateTime = DateTime.Now.AddHours(2), MovieId = movie.Id },
                new Screening { Id = Guid.NewGuid(), StartDateTime = DateTime.Now.AddDays(1), EndDateTime = DateTime.Now.AddDays(1).AddHours(2), MovieId = movie.Id }
            });

        context.SaveChanges();
        return context;
    }

    [Fact]
    public void Get_ReturnsAllScreenings()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new ScreeningsController(context);
        var initialCount = context.Screenings.Count();

        // Act
        var result = controller.Get(new PageQuery());

        // Assert
        result.Should().NotBeNull();
        result.Content.Count().Should().Be(initialCount);
        result.TotalElements.Should().Be(initialCount);
    }

    [Fact]
    public void Get_WithValidId_ReturnsScreening()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new ScreeningsController(context);
        var screeningId = context.Screenings.First().Id;

        // Act
        var result = controller.Get(screeningId);

        // Assert
        result.Should().NotBeNull();
        result.Movie!.Title.Should().Be("Test Movie");
    }

    [Fact]
    public void Get_WithInvalidId_ReturnsNull()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new ScreeningsController(context);

        // Act
        var result = controller.Get(Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [InlineData("2024-11-18T10:00:00", "2024-11-18T12:00:00")]
    [InlineData("2024-11-19T14:00:00", "2024-11-19T16:00:00")]
    public void Post_CreateScreeningWithValidModel_ReturnsCreatedStatus(
            string startDateTimeStr,
            string endDateTimeStr)
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new ScreeningsController(context);
        var initialCount = context.Screenings.Count();
        var startDateTime = DateTime.Parse(startDateTimeStr);
        var endDateTime = DateTime.Parse(endDateTimeStr);
        var movieId = context.Movies.First().Id;

        var newScreeningDto = new ScreeningCreateDto
        {
            StartDateTime = startDateTime,
            EndDateTime = endDateTime,
            MovieId = movieId
        };

        // Act
        var result = controller.Post(newScreeningDto);

        // Assert
        result.Should().BeOfType<CreatedResult>();
        context.Screenings.Count().Should().Be(initialCount + 1);
    }

    [Fact]
    public void Put_UpdateExistingScreening_ReturnsOkStatus()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new ScreeningsController(context);
        var screeningId = context.Screenings.First().Id;
        var updatedDto = new ScreeningCreateDto
        {
            StartDateTime = DateTime.Now.AddDays(2),
            EndDateTime = DateTime.Now.AddDays(2).AddHours(2),
            MovieId = context.Movies.First().Id
        };

        // Act
        var result = controller.Put(screeningId, updatedDto);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var updatedScreening = context.Screenings.First(s => s.Id == screeningId);
        updatedScreening.StartDateTime.Should().BeCloseTo(updatedDto.StartDateTime, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Delete_RemoveExistingScreening_ReturnsNoContent()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new ScreeningsController(context);
        var screeningId = context.Screenings.First().Id;

        // Act
        controller.Delete(screeningId);

        // Assert
        context.Screenings.Any(s => s.Id == screeningId).Should().BeFalse();
    }
}

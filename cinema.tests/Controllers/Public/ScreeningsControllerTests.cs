using AutoMapper;
using cinema.api.Controllers.Public;
using cinema.api.Helpers;
using cinema.api.Models;
using cinema.context;
using cinema.context.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cinema.tests.Controllers.Public;

public class ScreeningsControllerTests
{
    private CinemaDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<CinemaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new CinemaDbContext(options);

        var actionCategory = new Category { Id = Guid.NewGuid(), Name = "Action" };
        var dramaCategory = new Category { Id = Guid.NewGuid(), Name = "Drama" };

        var movie1 = new Movie
        {
            Id = Guid.NewGuid(),
            Title = "Action Movie",
            DurationMinutes = 120,
            PosterUrl = "http://example.com/poster1.jpg",
            TrailerUrl = "http://example.com/trailer1.mp4",
            BackgroundUrl = "http://example.com/background1.jpg",
            Director = "Director A",
            Cast = "Actor A, Actor B",
            Description = "An action-packed movie.",
            Rating = 8.5,
            Category = actionCategory
        };

        var movie2 = new Movie
        {
            Id = Guid.NewGuid(),
            Title = "Drama Movie",
            DurationMinutes = 100,
            PosterUrl = "http://example.com/poster2.jpg",
            TrailerUrl = "http://example.com/trailer2.mp4",
            BackgroundUrl = "http://example.com/background2.jpg",
            Director = "Director B",
            Cast = "Actor C, Actor D",
            Description = "A heartfelt drama.",
            Rating = 9.0,
            Category = dramaCategory
        };

        context.Screenings.AddRange(new List<Screening>
        {
            new Screening
            {
                Id = Guid.NewGuid(),
                StartDateTime = DateTimeOffset.Now.AddDays(-1),
                EndDateTime = DateTimeOffset.Now.AddDays(-1).AddHours(2),
                Movie = movie1
            },
            new Screening
            {
                Id = Guid.NewGuid(),
                StartDateTime = DateTimeOffset.Now.AddHours(2),
                EndDateTime = DateTimeOffset.Now.AddHours(4),
                Movie = movie2
            }
        });

        context.Seats.AddRange(new Seat
        {
            Id = Guid.NewGuid(),
            Row = 'A',
            Number = 1
        }, new Seat
        {
            Id = Guid.NewGuid(),
            Row = 'A',
            Number = 2
        });

        context.SaveChanges();

        return context;
    }

    private ScreeningsController CreateController(CinemaDbContext context)
    {
        var mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
        return new ScreeningsController(context, mapper);
    }

    [Fact]
    public void Get_WithInvalidDateRange_ReturnsBadRequest()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = CreateController(context);

        var startDate = DateTimeOffset.Now.AddDays(2);
        var endDate = DateTimeOffset.Now.AddDays(1);

        // Act
        var result = controller.Get(startDate, endDate).Result;

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.Value.Should().Be("Data początkowa musi być wcześniejsza niż data końcowa.");
    }

    [Fact]
    public void Get_ReturnsScreeningsWithinDateRange()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = CreateController(context);

        var startDate = DateTimeOffset.Now.AddDays(-2);
        var endDate = DateTimeOffset.Now.AddDays(1);

        // Act
        var result = controller.Get(startDate, endDate).Result as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        var screenings = result!.Value as List<ScreeningDto>;
        screenings.Should().NotBeNull();
        screenings.Should().HaveCount(context.Screenings.Count());
    }

    [Fact]
    public void Get_WithValidId_ReturnsScreening()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = CreateController(context);
        var screeningId = context.Screenings.First().Id;

        // Act
        var result = controller.Get(screeningId).Result as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        var screening = result!.Value as ScreeningDto;
        screening.Should().NotBeNull();
        screening!.MovieId.Should().Be(context.Movies.First(x => x.Title == "Action Movie").Id);
    }

    [Fact]
    public void Get_WithInvalidId_ReturnsBadRequest()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = CreateController(context);

        // Act
        var result = controller.Get(Guid.NewGuid()).Result;

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.Value.Should().Be("Seans nie został znaleziony.");
    }

    [Fact]
    public void GetSeats_ReturnsSeatAvailability()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = CreateController(context);
        var screeningId = context.Screenings.First().Id;

        // Act
        var result = controller.GetSeats(screeningId).Result as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        var seatResult = result!.Value as SeatResult;
        seatResult.Should().NotBeNull();
        seatResult!.TotalSeats.Should().Be(2);
        seatResult!.AvailableSeats.Should().Be(2);
    }
}

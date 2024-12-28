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

        var screening = new Screening
        {
            Id = Guid.NewGuid(),
            StartDateTime = DateTime.Now,
            EndDateTime = DateTime.Now.AddHours(2),
            MovieId = movie.Id
        };
        context.Screenings.Add(screening);

        var seat1 = new Seat { Id = Guid.NewGuid(), Row = 'A', Number = 1 };
        var seat2 = new Seat { Id = Guid.NewGuid(), Row = 'A', Number = 2 };
        var seat3 = new Seat { Id = Guid.NewGuid(), Row = 'B', Number = 1 };
        var seat4 = new Seat { Id = Guid.NewGuid(), Row = 'B', Number = 2 };

        var seats = new List<Seat>
        {
            seat1,
            seat2,
            seat3,
            seat4
        };
        context.Seats.AddRange(seats);

        var order = new Order
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            PhoneNumber = "1234567890",
            Status = OrderStatus.Ready,
            ScreeningId = screening.Id,
            Seats = new List<Seat> { seat1, seat2 }
        };
        context.Orders.Add(order);

        context.SaveChanges();
        return context;
    }

    private ScreeningsController CreateController(CinemaDbContext context)
    {
        var mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

        return new ScreeningsController(context, mapper);
    }

    [Fact]
    public void Get_ReturnsAllScreenings()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = CreateController(context);
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
        var controller = CreateController(context);
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
        var controller = CreateController(context);

        // Act
        var result = controller.Get(Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [InlineData("2024-11-18T10:00:00")]
    [InlineData("2024-11-19T14:00:00")]
    public void Post_CreateScreeningWithValidModel_ReturnsCreatedStatus(string startDateTimeStr)
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = CreateController(context);
        var initialCount = context.Screenings.Count();
        var startDateTime = DateTime.Parse(startDateTimeStr);
        var movieId = context.Movies.First().Id;

        var newScreeningDto = new ScreeningCreateDto
        {
            StartDateTime = startDateTime,
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
        var controller = CreateController(context);
        var screeningId = context.Screenings.First().Id;
        var movie = context.Movies.First();
        var updatedDto = new ScreeningCreateDto
        {
            StartDateTime = DateTime.Now.AddDays(2),
            MovieId = movie.Id
        };

        // Act
        var result = controller.Put(screeningId, updatedDto);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var updatedScreening = context.Screenings.First(s => s.Id == screeningId);
        updatedScreening.StartDateTime.Should().BeCloseTo(updatedDto.StartDateTime, TimeSpan.FromSeconds(1));
        updatedScreening.EndDateTime.Should().BeCloseTo(updatedDto.StartDateTime.AddMinutes(movie.DurationMinutes + 30), TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Delete_RemoveExistingScreening_ReturnsNoContent()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = CreateController(context);
        var screeningId = context.Screenings.First().Id;

        // Act
        controller.Delete(screeningId);

        // Assert
        context.Screenings.Any(s => s.Id == screeningId).Should().BeFalse();
    }

    [Fact]
    public void GetSeats_WithValidScreeningId_ReturnsCorrectSeatResult()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = CreateController(context);
        var screeningId = context.Screenings.First().Id;

        // Act
        var result = controller.GetSeats(screeningId);

        // Assert
        result.Should().NotBeNull();
        result.TotalSeats.Should().Be(4);
        result.TakenSeats.Should().Be(2);
        result.AvailableSeats.Should().Be(2);
        result.Seats.Count.Should().Be(4);
        result.Seats.First(s => s.Row == 'A' && s.Number == 1).IsTaken.Should().BeTrue();
        result.Seats.First(s => s.Row == 'A' && s.Number == 2).IsTaken.Should().BeTrue();
        result.Seats.First(s => s.Row == 'B' && s.Number == 1).IsTaken.Should().BeFalse();
        result.Seats.First(s => s.Row == 'B' && s.Number == 2).IsTaken.Should().BeFalse();
    }

    [Fact]
    public void GetSeats_WithInvalidScreeningId_ReturnsEmptySeatResult()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = CreateController(context);
        var invalidScreeningId = Guid.NewGuid();

        // Act
        var result = controller.GetSeats(invalidScreeningId);

        // Assert
        result.Should().NotBeNull();
        result.TotalSeats.Should().Be(4);
        result.TakenSeats.Should().Be(0);
        result.AvailableSeats.Should().Be(4);
        result.Seats.Count.Should().Be(4);
        result.Seats.All(s => !s.IsTaken).Should().BeTrue();
    }

    [Fact]
    public void Post_CreateScreeningWithInvalidOverlappingTime_ReturnsBadRequest()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = CreateController(context);
        var initialCount = context.Screenings.Count();
        var movie = context.Movies.First();

        var screening = new Screening
        {
            Id = Guid.NewGuid(),
            StartDateTime = new DateTimeOffset(2024, 3, 10, 8, 30, 0, TimeSpan.Zero),
            EndDateTime = DateTime.Now.AddHours(2),
            MovieId = movie.Id
        };
        context.Screenings.Add(screening);
        context.SaveChanges();

        var startDateTime = new DateTimeOffset(2024, 3, 10, 8, 40, 0, TimeSpan.Zero);

        var newScreeningDto = new ScreeningCreateDto
        {
            StartDateTime = startDateTime,
            MovieId = movie.Id
        };

        // Act
        var result = controller.Post(newScreeningDto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        context.Screenings.Count().Should().Be(initialCount + 1);
    }
}

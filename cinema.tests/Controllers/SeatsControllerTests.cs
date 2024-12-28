using cinema.api.Controllers.Admin;
using cinema.context.Entities;
using cinema.context;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace cinema.tests.Controllers;

public class SeatsControllerTests
{
    private CinemaDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<CinemaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new CinemaDbContext(options);

        context.Seats.AddRange(new List<Seat>
        {
            new Seat { Id = Guid.NewGuid(), Row = 'A', Number = 1 },
            new Seat { Id = Guid.NewGuid(), Row = 'A', Number = 2 },
            new Seat { Id = Guid.NewGuid(), Row = 'B', Number = 1 },
            new Seat { Id = Guid.NewGuid(), Row = 'B', Number = 2 },
            new Seat { Id = Guid.NewGuid(), Row = 'C', Number = 1 },
            new Seat { Id = Guid.NewGuid(), Row = 'C', Number = 2 }
        });
        context.SaveChanges();

        return context;
    }

    [Fact]
    public void Get_ReturnsAllSeats()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new SeatsAdminController(context);

        // Act
        var result = controller.Get();

        // Assert
        result.Should().NotBeNull();
        var seats = (result.Result as OkObjectResult)!.Value as IEnumerable<Seat>;
        seats!.Count().Should().Be(6);
    }

    [Fact]
    public void Get_WithValidId_ReturnsSeat()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new SeatsAdminController(context);
        var seatId = context.Seats.First().Id;

        // Act
        var result = controller.Get(seatId);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var seat = (result.Result as OkObjectResult)?.Value as Seat;
        seat.Should().NotBeNull();
        seat!.Row.Should().Be('A');
        seat.Number.Should().Be(1);
    }

    [Fact]
    public void Get_WithInvalidId_ReturnsNull()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new SeatsAdminController(context);
        var invalidId = Guid.NewGuid();

        // Act
        var result = controller.Get(invalidId);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
        var message = (result.Result as NotFoundObjectResult)?.Value as string;
        message.Should().Be($"Seat with ID {invalidId} not found.");
    }

    [Fact]
    public void GetByRowAndNumber_WithValidRowAndNumber_ReturnsSeat()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new SeatsAdminController(context);

        // Act
        var result = controller.GetByRowAndNumber('B', 2);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var seat = (result.Result as OkObjectResult)?.Value as Seat;
        seat.Should().NotBeNull();
        seat!.Row.Should().Be('B');
        seat.Number.Should().Be(2);
    }

    [Fact]
    public void GetByRowAndNumber_WithInvalidRowAndNumber_ReturnsNull()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new SeatsAdminController(context);

        // Act
        var result = controller.GetByRowAndNumber('D', 4);

        // Assert
        result.Result.Should().BeOfType<NotFoundObjectResult>();
        var message = (result.Result as NotFoundObjectResult)?.Value as string;
        message.Should().Be("Seat in row 'D' with number '4' not found.");
    }
}
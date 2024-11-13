using cinema.api.Controllers.Admin;
using cinema.context.Entities;
using cinema.context;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;

namespace cinema.tests;

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
        var controller = new SeatsController(context);

        // Act
        var result = controller.Get();

        // Assert
        result.Should().NotBeNull();
        result.Count().Should().Be(6);
    }

    [Fact]
    public void Get_WithValidId_ReturnsSeat()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new SeatsController(context);
        var seatId = context.Seats.First().Id;

        // Act
        var result = controller.Get(seatId);

        // Assert
        result.Should().NotBeNull();
        result.Row.Should().Be('A');
        result.Number.Should().Be(1);
    }

    [Fact]
    public void Get_WithInvalidId_ReturnsNull()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new SeatsController(context);

        // Act
        var result = controller.Get(Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetByRowAndNumber_WithValidRowAndNumber_ReturnsSeat()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new SeatsController(context);

        // Act
        var result = controller.GetByRowAndNumber('B', 2);

        // Assert
        result.Should().NotBeNull();
        result.Row.Should().Be('B');
        result.Number.Should().Be(2);
    }

    [Fact]
    public void GetByRowAndNumber_WithInvalidRowAndNumber_ReturnsNull()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new SeatsController(context);

        // Act
        var result = controller.GetByRowAndNumber('D', 4);

        // Assert
        result.Should().BeNull();
    }
}
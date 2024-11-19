using cinema.context;
using cinema.context.Entities;
using cinema.api.Controllers.Admin;
using cinema.api.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace cinema.tests.Controllers;

public class OrdersControllerTests
{
    private CinemaDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<CinemaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new CinemaDbContext(options);

        var category = new Category { Id = Guid.NewGuid(), Name = "Test Category" };
        context.Categories.Add(category);

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
            Category = category
        };
        context.Movies.Add(movie);

        var screening = new Screening
        {
            Id = Guid.NewGuid(),
            StartDateTime = DateTime.Now,
            EndDateTime = DateTime.Now.AddHours(2),
            Movie = movie
        };
        context.Screenings.Add(screening);

        var seats = new List<Seat>
        {
            new Seat { Id = Guid.NewGuid(), Row = 'A', Number = 1 },
            new Seat { Id = Guid.NewGuid(), Row = 'A', Number = 2 },
            new Seat { Id = Guid.NewGuid(), Row = 'B', Number = 1 },
            new Seat { Id = Guid.NewGuid(), Row = 'B', Number = 2 }
        };
        context.Seats.AddRange(seats);

        var order = new Order
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            PhoneNumber = "1234567890",
            Status = OrderStatus.Pending,
            Screening = screening,
            Seats = new List<Seat> { seats[0], seats[2] }
        };
        context.Orders.Add(order);

        context.SaveChanges();
        return context;
    }

    [Fact]
    public void Get_WithValidId_ReturnsOrder()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new OrdersController(context);
        var orderId = context.Orders.First().Id;

        // Act
        var result = controller.Get(orderId);

        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be("test@example.com");
        result.Status.Should().Be(OrderStatus.Pending);
        result.Seats!.Count.Should().Be(2);
    }

    [Fact]
    public void Get_WithInvalidId_ReturnsNull()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new OrdersController(context);

        // Act
        var result = controller.Get(Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Post_CreateOrderWithValidData_ReturnsCreatedStatus()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new OrdersController(context);
        var screeningId = context.Screenings.First().Id;
        var seatIds = context.Seats.Select(s => s.Id).ToList();
        var orderDto = new OrderCreateDto
        {
            Email = "neworder@example.com",
            PhoneNumber = "0987654321",
            Status = OrderStatus.Ready,
            ScreeningId = screeningId,
            SeatIds = seatIds
        };

        // Act
        var result = controller.Post(orderDto);

        // Assert
        result.Should().BeOfType<CreatedResult>();
        context.Orders.Count().Should().Be(2);
        var newOrder = context.Orders.Last();
        newOrder.Email.Should().Be("neworder@example.com");
        newOrder.Seats!.Count.Should().Be(seatIds.Count);
    }

    [Fact]
    public void Put_UpdateOrderWithValidData_ReturnsUpdatedOrder()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new OrdersController(context);
        var existingOrder = context.Orders.First();
        var newSeatIds = context.Seats.Select(s => s.Id).ToList();
        var updatedOrderDto = new OrderCreateDto
        {
            Email = "updated@example.com",
            PhoneNumber = "1112223333",
            Status = OrderStatus.Ready,
            ScreeningId = existingOrder.ScreeningId!.Value,
            SeatIds = newSeatIds
        };

        // Act
        var result = controller.Put(existingOrder.Id, updatedOrderDto);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var updatedOrder = context.Orders.First();
        updatedOrder.Email.Should().Be("updated@example.com");
        updatedOrder.Status.Should().Be(OrderStatus.Ready);
        updatedOrder.Seats!.Count.Should().Be(newSeatIds.Count);
    }

    [Fact]
    public void Delete_WithValidId_RemovesOrder()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new OrdersController(context);
        var orderId = context.Orders.First().Id;
        var initialCount = context.Categories.Count();

        // Act
        controller.Delete(orderId);

        // Assert
        context.Orders.Count().Should().Be(initialCount - 1);
    }
}

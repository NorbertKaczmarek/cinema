using AutoMapper;
using cinema.api;
using cinema.api.Controllers.Admin;
using cinema.api.Helpers;
using cinema.api.Helpers.EmailSender;
using cinema.api.Models;
using cinema.api.Models.Admin;
using cinema.context;
using cinema.context.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace cinema.tests.Controllers;

public class OrdersControllerTests
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
            Category = new Category { Id = Guid.NewGuid(), Name = "Action" }
        };
        context.Movies.Add(movie);

        var screening = new Screening
        {
            Id = Guid.NewGuid(),
            StartDateTime = DateTime.Now.AddHours(1),
            EndDateTime = DateTime.Now.AddHours(2),
            Movie = movie
        };
        context.Screenings.Add(screening);

        var seats = new List<Seat>
        {
            new Seat { Id = Guid.NewGuid(), Row = 'A', Number = 1 },
            new Seat { Id = Guid.NewGuid(), Row = 'A', Number = 2 },
            new Seat { Id = Guid.NewGuid(), Row = 'B', Number = 1 },
            new Seat { Id = Guid.NewGuid(), Row = 'B', Number = 2 },
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

    private OrdersAdminController CreateController(CinemaDbContext context)
    {
        var emailOptionsMock = new Mock<EmailOptions>();
        var emailSenderMock = new Mock<IEmailSender>();
        var mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

        return new OrdersAdminController(context, emailOptionsMock.Object, emailSenderMock.Object, mapper);
    }


    [Fact]
    public void Get_WithValidId_ReturnsOrder()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = CreateController(context);

        var orderId = context.Orders.First().Id;

        // Act
        var result = controller.Get(orderId);

        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be("test@example.com");
        result.Status.Should().Be("Pending");
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

    [Fact]
    public void Post_CreateOrderWithAvailableSeats_ReturnsCreatedStatus()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = CreateController(context);

        var movie = context.Movies.First();
        var screening = new Screening
        {
            Id = Guid.NewGuid(),
            StartDateTime = DateTime.Now.AddDays(1),
            EndDateTime = DateTime.Now.AddDays(1).AddHours(2),
            Movie = movie
        };
        context.Screenings.Add(screening);
        context.SaveChanges();

        var seatIds = context.Seats.Select(s => s.Id).ToList();
        var initialCount = context.Orders.Count();

        var orderDto = new OrderCreateDto
        {
            Email = "test@example.com",
            PhoneNumber = "123-456-7890",
            Status = "Pending",
            ScreeningId = screening.Id,
            SeatIds = seatIds
        };

        // Act
        var result = controller.Post(orderDto);

        // Assert
        result.Should().BeOfType<CreatedResult>();
        context.Orders.Count().Should().Be(initialCount + 1);
    }

    [Fact]
    public void Post_CreateOrderWithTakenSeats_ReturnsBadRequest()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = CreateController(context);

        var screening = context.Screenings.First();
        var seatIds = context.Seats.Select(s => s.Id).ToList();

        var initialOrder = new Order
        {
            Email = "initial@example.com",
            PhoneNumber = "111-111-1111",
            Status = OrderStatus.Pending,
            ScreeningId = screening.Id,
            Seats = context.Seats.ToList()
        };
        context.Orders.Add(initialOrder);
        context.SaveChanges();

        var orderDto = new OrderCreateDto
        {
            Email = "test@example.com",
            PhoneNumber = "123-456-7890",
            Status = "Pending",
            ScreeningId = screening.Id,
            SeatIds = seatIds
        };

        // Act
        var result = controller.Post(orderDto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.Value.Should().Be("The following seats are already taken: A1, A2, B1, B2");
    }

    [Fact]
    public void Post_CreateOrderWithInvalidSeatIds_ReturnsBadRequest()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = CreateController(context);

        var screening = context.Screenings.First();

        var invalidSeatIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

        var orderDto = new OrderCreateDto
        {
            Email = "test@example.com",
            PhoneNumber = "123-456-7890",
            Status = "Pending",
            ScreeningId = screening.Id,
            SeatIds = invalidSeatIds
        };

        // Act
        var result = controller.Post(orderDto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.Value.Should().Be("One or more seat IDs are invalid.");
    }

    [Fact]
    public void Put_UpdateOrderWithValidData_ReturnsUpdatedOrder()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = CreateController(context);

        var existingOrder = context.Orders.First();
        var newSeatIds = context.Seats.Select(s => s.Id).ToList();
        var updatedOrderDto = new OrderUpdateDto
        {
            Email = "updated@example.com",
            PhoneNumber = "1112223333",
            Status = "Ready",
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
        var controller = CreateController(context);

        var orderId = context.Orders.First().Id;
        var initialCount = context.Orders.Count();

        // Act
        controller.Delete(orderId);

        // Assert
        context.Orders.Count().Should().Be(initialCount - 1);
    }
}

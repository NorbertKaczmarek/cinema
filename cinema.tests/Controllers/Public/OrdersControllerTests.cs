using AutoMapper;
using cinema.api.Controllers.Public;
using cinema.api.Models;
using cinema.context;
using cinema.context.Entities;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using FluentAssertions;
using cinema.api.Models.Admin;
using cinema.api;
using Microsoft.EntityFrameworkCore;
using cinema.api.Helpers.EmailSender;

namespace cinema.tests.Controllers.Public;

public class OrdersControllerTests
{
    private CinemaDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<CinemaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new CinemaDbContext(options);

        context.Screenings.AddRange(new List<Screening>
        {
            new Screening
            {
                Id = Guid.NewGuid(),
                StartDateTime = DateTimeOffset.Now.AddHours(2),
                EndDateTime = DateTimeOffset.Now.AddHours(4),
                Movie = new Movie
                {
                    Id = Guid.NewGuid(),
                    Title = "Movie A",
                    DurationMinutes = 111,
                    PosterUrl = "https://movie1.jpg",
                    TrailerUrl = "https://movie1.jpg",
                    BackgroundUrl = "https://movie1.jpg",
                    Director = "Director 1",
                    Cast = "Cast 1",
                    Description = "Description 1",
                    Rating = 1.1,
                    Category = context.Categories.FirstOrDefault(x => x.Name == "Dramat")
                }
            }
        });

        context.Seats.AddRange(new List<Seat>
        {
            new Seat { Id = Guid.NewGuid(), Row = 'A', Number = 1 },
            new Seat { Id = Guid.NewGuid(), Row = 'A', Number = 2 },
            new Seat { Id = Guid.NewGuid(), Row = 'B', Number = 1 }
        });

        context.SaveChanges();

        return context;
    }

    private OrdersController CreateController(CinemaDbContext context)
    {
        var emailOptionsMock = new Mock<EmailOptions>();
        var emailSenderMock = new Mock<IEmailSender>();
        var mapperMock = new Mock<IMapper>();

        return new OrdersController(context, emailOptionsMock.Object, emailSenderMock.Object, mapperMock.Object);
    }

    [Fact]
    public void Post_ValidOrder_ReturnsCreatedResult()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = CreateController(context);

        var dto = new OrderCreateDto
        {
            Email = "test@example.com",
            SeatIds = new List<Guid> { context.Seats.First().Id },
            ScreeningId = context.Screenings.First().Id
        };

        // Act
        var result = controller.Post(dto) as CreatedResult;

        // Assert
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(201);
    }

    [Fact]
    public void Post_InvalidOrder_MissingEmail_ReturnsBadRequest()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = CreateController(context);

        var dto = new OrderCreateDto
        {
            Email = null,
            SeatIds = new List<Guid> { context.Seats.First().Id },
            ScreeningId = context.Screenings.First().Id
        };

        // Act
        var result = controller.Post(dto) as BadRequestObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.Value.Should().Be("Invalid order data.");
    }

    [Fact]
    public void Post_InvalidScreening_ReturnsBadRequest()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = CreateController(context);

        var dto = new OrderCreateDto
        {
            Email = "test@example.com",
            SeatIds = new List<Guid> { context.Seats.First().Id },
            ScreeningId = Guid.NewGuid()
        };

        // Act
        var result = controller.Post(dto) as BadRequestObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.Value.Should().Be("Invalid screening ID.");
    }

    [Fact]
    public void Post_SeatAlreadyTaken_ReturnsBadRequest()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = CreateController(context);

        var existingOrder = new Order
        {
            Email = "existing@example.com",
            PhoneNumber = "",
            ScreeningId = context.Screenings.First().Id,
            Seats = new List<Seat> { context.Seats.First() },
            Status = OrderStatus.Pending
        };
        context.Orders.Add(existingOrder);
        context.SaveChanges();

        var dto = new OrderCreateDto
        {
            Email = "test@example.com",
            SeatIds = new List<Guid> { context.Seats.First().Id },
            ScreeningId = context.Screenings.First().Id
        };

        // Act
        var result = controller.Post(dto) as BadRequestObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.Value.Should().Be("The following seats are already taken: A1");
    }

    [Fact]
    public void Post_InvalidSeatIds_ReturnsBadRequest()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = CreateController(context);

        var dto = new OrderCreateDto
        {
            Email = "test@example.com",
            SeatIds = new List<Guid> { Guid.NewGuid() },
            ScreeningId = context.Screenings.First().Id
        };

        // Act
        var result = controller.Post(dto) as BadRequestObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.Value.Should().Be("One or more seat IDs are invalid.");
    }
}

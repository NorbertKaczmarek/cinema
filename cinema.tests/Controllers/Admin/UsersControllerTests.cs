using cinema.context;
using cinema.context.Entities;
using cinema.api.Models;
using Microsoft.EntityFrameworkCore;
using cinema.api.Controllers.Admin;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using AutoMapper;
using Moq;
using cinema.api.Models.Admin;
using cinema.api.Helpers;
using cinema.api;
using cinema.api.Helpers.EmailSender;

namespace cinema.tests.Controllers.Admin;

public class UsersControllerTests
{
    private readonly CinemaDbContext _context;
    private readonly UsersController _controller;

    public UsersControllerTests()
    {
        var options = new DbContextOptionsBuilder<CinemaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new CinemaDbContext(options);

        SeedDatabase();

        _controller = CreateController(_context);
    }

    private UsersController CreateController(CinemaDbContext context)
    {
        var emailSenderMock = new Mock<IEmailSender>();
        var mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

        return new UsersController(context, emailSenderMock.Object, mapper);
    }

    private void SeedDatabase()
    {
        if (!_context.Users.Any())
        {
            _context.Users.Add(new User
            {
                Id = Guid.NewGuid(),
                Email = "test1@example.com",
                FirstName = "Test",
                LastName = "User1",
                IsAdmin = false,
                Salt = "OpVnQG7P+2J2ZMxpWn4QOA==",
                SaltedHashedPassword = "iHj0p3mQDQYhYMioLWCL/T6pbhyPzP9gkqeY8uAMJM8="
            });

            _context.Users.Add(new User
            {
                Id = Guid.NewGuid(),
                Email = "test2@example.com",
                FirstName = "Test",
                LastName = "User2",
                IsAdmin = true,
                Salt = "sampleSalt",
                SaltedHashedPassword = "samplePasswordHash"
            });

            _context.SaveChanges();
        }
    }

    [Fact]
    public void Get_ShouldReturnAllUsers()
    {
        // Act
        var result = _controller.Get(new PageQuery());

        // Assert
        result.Should().NotBeNull();
        result.TotalElements.Should().Be(2);
    }

    [Fact]
    public void Post_WithValidData_ShouldCreateNewUser()
    {
        // Arrange
        var newUserDto = new UserCreateDto
        {
            Email = "newuser@example.com",
            FirstName = "New",
            LastName = "User"
        };

        // Act
        var result = _controller.Post(newUserDto);

        // Assert
        result.Should().BeOfType<OkResult>();
        _context.Users.Should().HaveCount(3);
    }

    [Fact]
    public void Post_WithExistingEmail_ShouldReturnBadRequest()
    {
        // Arrange
        var newUserDto = new UserCreateDto
        {
            Email = "test1@example.com",
            FirstName = "Duplicate",
            LastName = "User"
        };

        // Act
        var result = _controller.Post(newUserDto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public void Put_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var updateUserDto = new UserUpdateDto
        {
            Email = "nonexistent@example.com",
            FirstName = "Nonexistent",
            LastName = "User",
            Password = "NewPassword123",
            NewPassword = "Password123",
            ConfirmNewPassword = "Password123"
        };

        // Act
        var result = _controller.Put(Guid.NewGuid(), updateUserDto);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public void Delete_WithValidId_ShouldDeleteUser()
    {
        // Arrange
        var existingUser = _context.Users.First();

        // Act
        _controller.Delete(existingUser.Id);

        // Assert
        _context.Users.Should().HaveCount(1);
        _context.Users.Find(existingUser.Id).Should().BeNull();
    }

    [Fact]
    public void Delete_WithInvalidId_ShouldDoNothing()
    {
        // Arrange
        var initialCount = _context.Users.Count();

        // Act
        _controller.Delete(Guid.NewGuid());

        // Assert
        _context.Users.Should().HaveCount(initialCount);
    }
}

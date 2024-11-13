using cinema.context;
using cinema.context.Entities;
using cinema.api.Models;
using Microsoft.EntityFrameworkCore;
using cinema.api.Controllers.Admin;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;

namespace cinema.tests.Controllers;

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

        _controller = new UsersController(_context);
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
                Salt = "sampleSalt",
                SaltedHashedPassword = "samplePasswordHash"
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
        var result = _controller.Get();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
    }

    [Fact]
    public void Get_WithValidId_ShouldReturnUser()
    {
        // Arrange
        var existingUser = _context.Users.First();

        // Act
        var result = _controller.Get(existingUser.Id);

        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be(existingUser.Email);
    }

    [Fact]
    public void Get_WithInvalidId_ShouldReturnNull()
    {
        // Act
        var result = _controller.Get(Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Post_WithValidData_ShouldCreateNewUser()
    {
        // Arrange
        var newUserDto = new UserCreateDto
        {
            Email = "newuser@example.com",
            FirstName = "New",
            LastName = "User",
            IsAdmin = false,
            Password = "Password123",
            ConfirmPassword = "Password123"
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
            LastName = "User",
            IsAdmin = false,
            Password = "Password123",
            ConfirmPassword = "Password123"
        };

        // Act
        var result = _controller.Post(newUserDto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public void Put_WithValidData_ShouldUpdateUser()
    {
        // Arrange
        var existingUser = _context.Users.First();
        var updateUserDto = new UserCreateDto
        {
            Email = existingUser.Email,
            FirstName = "UpdatedFirstName",
            LastName = "UpdatedLastName",
            IsAdmin = existingUser.IsAdmin,
            Password = "NewPassword123",
            ConfirmPassword = "NewPassword123"
        };

        // Act
        var result = _controller.Put(existingUser.Id, updateUserDto);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var updatedUser = _context.Users.Find(existingUser.Id)!;
        updatedUser.Should().NotBeNull();
        updatedUser.FirstName.Should().Be("UpdatedFirstName");
        updatedUser.LastName.Should().Be("UpdatedLastName");
    }

    [Fact]
    public void Put_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var updateUserDto = new UserCreateDto
        {
            Email = "nonexistent@example.com",
            FirstName = "Nonexistent",
            LastName = "User",
            IsAdmin = false,
            Password = "Password123",
            ConfirmPassword = "Password123"
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

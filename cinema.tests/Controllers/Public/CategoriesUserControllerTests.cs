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

public class CategoriesPublicControllerTests
{
    private CinemaDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<CinemaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new CinemaDbContext(options);

        context.Categories.AddRange(new List<Category>
            {
                new Category { Id = Guid.NewGuid(), Name = "Action" },
                new Category { Id = Guid.NewGuid(), Name = "Drama" },
                new Category { Id = Guid.NewGuid(), Name = "Comedy" }
            });
        context.SaveChanges();

        return context;
    }

    private CategoriesUserController CreateController(CinemaDbContext context)
    {
        var mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

        return new CategoriesUserController(context, mapper);
    }

    [Fact]
    public void Get_ReturnsAllCategories()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = CreateController(context);

        // Act
        var result = controller.Get().Result as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        var categories = result!.Value as List<CategoryDto>;
        categories.Should().HaveCount(3);
    }

    [Fact]
    public void Get_WithValidId_ReturnsCategory()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = CreateController(context);
        var categoryId = context.Categories.First().Id;

        // Act
        var result = controller.Get(categoryId).Result as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        var category = result!.Value as CategoryDto;
        category.Should().NotBeNull();
        category!.Name.Should().Be("Action");
    }

    [Fact]
    public void Get_WithInvalidId_ReturnsNull()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = CreateController(context);

        // Act
        var result = controller.Get(Guid.NewGuid()).Result;

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var badRequestResult = result as NotFoundObjectResult;
        badRequestResult!.Value.Should().Be("Category with that id was not found.");
    }
}
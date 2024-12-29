using AutoMapper;
using cinema.api;
using cinema.api.Controllers.Admin;
using cinema.api.Helpers;
using cinema.api.Models;
using cinema.api.Models.Admin;
using cinema.context;
using cinema.context.Entities;
using FluentAssertions;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace cinema.tests.Controllers.Admin;

public class CategoriesControllerTests
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

    private CategoriesController CreateController(CinemaDbContext context)
    {
        var mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

        return new CategoriesController(context, mapper);
    }

    [Fact]
    public void Get_ReturnsAllCategories()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = CreateController(context);
        var query = new PageQuery { };

        // Act
        var result = controller.Get(query).Result as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        var categories = result!.Value as PageResult<CategoryDto>;
        categories!.Content.Should().HaveCount(3);
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

    [Theory]
    [InlineData("categoryName1")]
    [InlineData("categoryName2")]
    [InlineData("categoryName3")]
    public void Post_WithValidData_ShouldCreateCategory(string categoryName)
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = CreateController(context);
        var newCategoryDto = new CategoryCreateDto { Name = categoryName };

        // Act
        var result = controller.Post(newCategoryDto).Result as CreatedResult;

        // Assert
        var category = result!.Value as CategoryDto;
        category.Should().NotBeNull();
        category!.Name.Should().Be(categoryName);
        var createdCategoryId = result.Location!;
        category.Id.Should().Be(new Guid(createdCategoryId.Split('/').Last()));
    }

    [Theory]
    [InlineData("Action")]
    [InlineData("Drama")]
    [InlineData("Comedy")]
    public void Post_WithExistingCategoryName_ShouldReturnBadRequest(string categoryName)
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = CreateController(context);
        var existingCategoryDto = new CategoryCreateDto { Name = categoryName };

        // Act
        var result = controller.Post(existingCategoryDto).Result;

        // Assert
        result.Should().BeOfType<ConflictObjectResult>();
        var conflictResult = result as ConflictObjectResult;
        conflictResult!.Value.Should().Be("Category with that name already exists.");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Post_WithInvalidCategoryName_ShouldReturnBadRequest(string categoryName)
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = CreateController(context);
        var invalidCategoryDto = new CategoryCreateDto { Name = categoryName };

        // Act
        var result = controller.Post(invalidCategoryDto).Result;

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Theory]
    [InlineData("Updated Category1")]
    [InlineData("Updated Category2")]
    [InlineData("Updated Category3")]
    public void Put_WithValidData_ShouldUpdateCategory(string categoryName)
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = CreateController(context);
        var existingCategory = context.Categories.First();
        var updatedCategoryDto = new CategoryCreateDto { Name = categoryName };

        // Act
        var result = controller.Put(existingCategory.Id, updatedCategoryDto).Result;

        // Assert
        result.Should().BeOfType<CreatedResult>();
        var updatedCategory = context.Categories.Find(existingCategory.Id)!;
        updatedCategory.Should().NotBeNull();
        updatedCategory.Name.Should().Be(categoryName);
    }

    [Theory]
    [InlineData("Updated Category1")]
    [InlineData("Updated Category2")]
    [InlineData("Updated Category3")]
    public void Put_WithNonExistentCategory_ShouldReturnNotFound(string categoryName)
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = CreateController(context);
        var nonExistentCategoryId = Guid.NewGuid();
        var updatedCategoryDto = new CategoryCreateDto { Name = categoryName };

        // Act
        var result = controller.Put(nonExistentCategoryId, updatedCategoryDto).Result;

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public void Put_WithExistingCategoryName_ShouldReturnBadRequest()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = CreateController(context);
        var existingcategoryName = new Guid().ToString();
        var editedcategoryOldName = new Guid().ToString();
        var editedcategoryNewName = new Guid().ToString();

        var existingCategory = new Category { Name = existingcategoryName };
        var editedCategory = new Category { Name = editedcategoryOldName };
        context.Categories.AddRange(existingCategory, editedCategory);
        context.SaveChanges();

        var updatedCategoryDto = new CategoryCreateDto { Name = editedcategoryNewName };

        // Act
        var result = controller.Put(existingCategory.Id, updatedCategoryDto).Result;

        // Assert
        result.Should().BeOfType<ConflictObjectResult>();
        var conflictObjectResultResult = result as ConflictObjectResult;
        conflictObjectResultResult!.Value.Should().Be("Category with that name already exists.");
    }

    [Fact]
    public void Delete_WithValidId_ShouldDeleteCategory()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = CreateController(context);
        var categoryToDelete = context.Categories.First();
        var initialCount = context.Categories.Count();

        // Act
        controller.Delete(categoryToDelete.Id);

        // Assert
        context.Categories.Count().Should().Be(initialCount - 1);
        var deletedCategory = context.Categories.Find(categoryToDelete.Id);
        deletedCategory.Should().BeNull();
    }

    [Fact]
    public void Delete_WithInvalidId_ShouldNotDeleteCategory()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = CreateController(context);
        var invalidCategoryId = Guid.NewGuid();
        var initialCount = context.Categories.Count();

        // Act
        controller.Delete(invalidCategoryId);

        // Assert
        context.Categories.Count().Should().Be(initialCount);
        var category = context.Categories.First();
        category.Should().NotBeNull();
    }
}
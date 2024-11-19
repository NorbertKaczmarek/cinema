using cinema.api.Controllers.Admin;
using cinema.api.Models;
using cinema.context;
using cinema.context.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cinema.tests.Controllers;

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

    [Fact]
    public void Get_ReturnsAllCategories()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new CategoriesController(context);
        var initialCount = context.Categories.Count();

        // Act
        var result = controller.Get(new PageQuery());

        // Assert
        result.Should().NotBeNull();
        result.TotalElements.Should().Be(initialCount);
    }

    [Fact]
    public void Get_WithValidId_ReturnsCategory()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new CategoriesController(context);
        var categoryId = context.Categories.First().Id;

        // Act
        var result = controller.Get(categoryId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeOfType<Category>();
        var category = okResult.Value as Category;
        category!.Name.Should().Be("Action");
    }

    [Fact]
    public void Get_WithInvalidId_ReturnsNull()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new CategoriesController(context);

        // Act
        var result = controller.Get(Guid.NewGuid());

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
        var controller = new CategoriesController(context);
        var newCategoryDto = new CategoryCreateDto { CategoryName = categoryName };

        // Act
        var result = controller.Post(newCategoryDto);

        // Assert
        result.Should().BeOfType<CreatedResult>();
        var createdCategoryId = (result as CreatedResult)!.Location!;
        var createdCategory = context.Categories.Find(new Guid(createdCategoryId.Split('/').Last()))!;

        createdCategory.Should().NotBeNull();
        createdCategory.Name.Should().Be(categoryName);
    }

    [Theory]
    [InlineData("Action")]
    [InlineData("Drama")]
    [InlineData("Comedy")]
    public void Post_WithExistingCategoryName_ShouldReturnBadRequest(string categoryName)
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new CategoriesController(context);
        var existingCategoryDto = new CategoryCreateDto { CategoryName = categoryName };

        // Act
        var result = controller.Post(existingCategoryDto);

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
        var controller = new CategoriesController(context);
        var invalidCategoryDto = new CategoryCreateDto { CategoryName = categoryName };

        // Act
        var result = controller.Post(invalidCategoryDto);

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
        var controller = new CategoriesController(context);
        var existingCategory = context.Categories.First();
        var updatedCategoryDto = new CategoryCreateDto { CategoryName = categoryName };

        // Act
        var result = controller.Put(existingCategory.Id, updatedCategoryDto);

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
        var controller = new CategoriesController(context);
        var nonExistentCategoryId = Guid.NewGuid();
        var updatedCategoryDto = new CategoryCreateDto { CategoryName = categoryName };

        // Act
        var result = controller.Put(nonExistentCategoryId, updatedCategoryDto);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public void Put_WithExistingCategoryName_ShouldReturnBadRequest()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new CategoriesController(context);
        var existingcategoryName = (new Guid()).ToString();
        var editedcategoryOldName = (new Guid()).ToString();
        var editedcategoryNewName = (new Guid()).ToString();

        var existingCategory = new Category { Name = existingcategoryName };
        var editedCategory = new Category { Name = editedcategoryOldName };
        context.Categories.AddRange(existingCategory, editedCategory);
        context.SaveChanges();

        var updatedCategoryDto = new CategoryCreateDto { CategoryName = editedcategoryNewName };

        // Act
        var result = controller.Put(existingCategory.Id, updatedCategoryDto);

        // Assert
        result.Should().BeOfType<ConflictObjectResult>();
        var conflictResult = result as ConflictObjectResult;
        conflictResult!.Value.Should().Be("Category with that name already exists.");
    }

    [Fact]
    public void Delete_WithValidId_ShouldDeleteCategory()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new CategoriesController(context);
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
        var controller = new CategoriesController(context);
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
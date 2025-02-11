﻿using AutoMapper;
using cinema.api.Models.Admin;
using cinema.api.Models;
using cinema.context.Entities;
using cinema.context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace cinema.api.Controllers.Admin;

[ApiController]
[Route("api/admin/categories")]
[ApiExplorerSettings(GroupName = "Admin")]
[Authorize(Roles = "User,Admin")]
public class CategoriesController : ControllerBase
{
    private readonly CinemaDbContext _context;
    private readonly IMapper _mapper;

    public CategoriesController(CinemaDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves all categories with optional pagination and search functionality.
    /// </summary>
    /// <returns>A paginated list of categories or a full list if Size is set to 0.</returns>
    [HttpGet]
    public ActionResult<PageResult<CategoryDto>> Get([FromQuery] PageQuery query)
    {
        var baseQuery = _context.Categories.AsQueryable();

        baseQuery = baseQuery
            .Where
            (
                c => query.Phrase == null ||
                (
                    c.Name.ToLower().Contains(query.Phrase.ToLower())
                )
            );

        var totalCount = baseQuery.Count();

        var result = query.Size == 0
            ? baseQuery.ToList()
            : baseQuery.Skip(query.Size * query.Page).Take(query.Size).ToList();

        var resultDto = _mapper.Map<List<CategoryDto>>(result);

        return Ok(new PageResult<CategoryDto>(resultDto, totalCount, query.Size));
    }

    /// <summary>
    /// Retrieves a specific category by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the category to retrieve.</param>
    /// <returns>
    /// The requested category if found; otherwise, a 404 Not Found response.
    /// </returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Category), 200)]
    [ProducesResponseType(typeof(string), 404)]
    public ActionResult<CategoryDto> Get(Guid id)
    {
        var category = getById(id);
        if (category is null) return NotFound("Kategoria o podanym identyfikatorze nie została znaleziona.");

        var categoryDto = _mapper.Map<CategoryDto>(category);
        return Ok(categoryDto);
    }

    private Category? getById(Guid id)
    {
        return _context.Categories.FirstOrDefault(m => m.Id == id);
    }

    /// <summary>
    /// Creates a new category.
    /// </summary>
    /// <param name="dto">An object containing the category name:
    /// <list type="bullet">
    /// <item><term>Name</term>: The name of the category to be created (required).</item>
    /// </list>
    /// </param>
    /// <returns>
    /// The created category if successful; otherwise, a 400 Bad Request or 409 Conflict response.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(Category), 201)]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(string), 409)]
    public ActionResult<CategoryDto> Post([FromBody] CategoryCreateDto dto)
    {
        if (dto == null || dto.Name == null || dto.Name.Trim() == "")
            return BadRequest("Nieprawidłowe dane kategorii.");

        var category = _context.Categories.FirstOrDefault(x => x.Name == dto.Name);
        if (category != null) return Conflict("Kategoria o tej nazwie już istnieje.");

        var newCategory = new Category { Name = dto.Name };
        _context.Categories.Add(newCategory);
        _context.SaveChanges();

        var categoryDto = _mapper.Map<CategoryDto>(newCategory);
        return Created($"/api/admin/categories/{categoryDto.Id}", categoryDto);
    }

    /// <summary>
    /// Updates an existing category by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the category to update.</param>
    /// <param name="dto">An object containing the updated category name:
    /// <list type="bullet">
    /// <item><term>Name</term>: The new name for the category (required).</item>
    /// </list>
    /// </param>
    /// <returns>
    /// The updated category if successful; otherwise, a 400 Bad Request, 404 Not Found, or 409 Conflict response.
    /// </returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Category), 201)]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(string), 409)]
    public ActionResult<CategoryDto> Put(Guid id, [FromBody] CategoryCreateDto dto)
    {
        if (dto == null || dto.Name == null || dto.Name.Trim() == "")
            return BadRequest("Nieprawidłowe dane kategorii.");

        var existingCategory = getById(id);
        if (existingCategory == null) return NotFound($"Kategoria o identyfikatorze {id} nie została znaleziona.");

        var categoryWithThatName = _context.Categories.FirstOrDefault(x => x.Name == dto.Name);
        if (categoryWithThatName != null && categoryWithThatName.Id != id)
            return Conflict("Kategoria o tej nazwie już istnieje.");

        existingCategory.Name = dto.Name;
        _context.SaveChanges();

        var categoryDto = _mapper.Map<CategoryDto>(existingCategory);
        return Created($"/api/admin/categories/{categoryDto.Id}", categoryDto);
    }

    /// <summary>
    /// Deletes an existing category by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the category to delete.</param>
    /// <returns>
    /// A 204 No Content response if successful; otherwise, a 404 Not Found response if the category does not exist.
    /// </returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(string), 404)]
    public ActionResult Delete(Guid id)
    {
        var category = getById(id);
        if (category == null) return NotFound("Kategoria nie została znaleziona.");

        _context.Categories.Remove(category);
        _context.SaveChanges();

        return NoContent();
    }
}

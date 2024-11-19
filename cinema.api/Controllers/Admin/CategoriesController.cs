using cinema.api.Models;
using cinema.context;
using cinema.context.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cinema.api.Controllers.Admin;

[Route("api/admin/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly CinemaDbContext _context;

    public CategoriesController(CinemaDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public PageResult<Category> Get([FromQuery] PageQuery query)
    {
        var baseQuery = _context
            .Categories
            .Where(
                c => query.Phrase == null ||
                (
                    c.Name.ToLower().Contains(query.Phrase.ToLower())
                )
            );

        var totalCount = baseQuery.Count();

        List<Category> result;

        if (query.Size == 0)
        {
            result = baseQuery.ToList();
        }
        else
        {
            result = baseQuery
                .Skip(query.Size * query.Page)
                .Take(query.Size)
                .ToList();
        }

        return new PageResult<Category>(result, totalCount, query.Size);
    }

    [HttpGet("{id}")]
    public ActionResult Get(Guid id)
    {
        var category = getById(id);
        if (category is null) return NotFound("Category with that id was not found.");
        return Ok(category);
    }

    private Category? getById(Guid id)
    {
        return _context.Categories.FirstOrDefault(m => m.Id == id);
    }

    [HttpPost]
    public ActionResult Post([FromBody] CategoryCreateDto dto)
    {
        if (dto == null || dto.CategoryName == null || dto.CategoryName.Trim() == "") 
            return BadRequest("Invalid model.");

        var category = _context.Categories.FirstOrDefault(x => x.Name == dto.CategoryName);
        if (category != null) return Conflict("Category with that name already exists.");

        var newCategory = new Category { Name = dto.CategoryName };
        _context.Categories.Add(newCategory);
        _context.SaveChanges();

        return Created($"/api/admin/categories/{newCategory.Id}", newCategory);
    }

    [HttpPut("{id}")]
    public ActionResult Put(Guid id, [FromBody] CategoryCreateDto dto)
    {
        if (dto == null || dto.CategoryName == null) return BadRequest("Invalid category data.");

        var existingCategory = getById(id);
        if (existingCategory == null) return NotFound($"Category with id {id} not found.");

        var categoryWithThatName = _context.Categories.FirstOrDefault(x => x.Name == dto.CategoryName);
        if (categoryWithThatName != null && categoryWithThatName.Id != id) 
            return Conflict("Category with that name already exists.");

        existingCategory.Name = dto.CategoryName;
        _context.SaveChanges();

        return Created($"/api/admin/categories/{existingCategory.Id}", existingCategory);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(Guid id)
    {
        var category = getById(id);
        if (category == null) return NotFound("Category not found.");

        _context.Categories.Remove(category);
        _context.SaveChanges();

        return NoContent();
    }
}

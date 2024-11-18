using cinema.api.Models;
using cinema.context;
using cinema.context.Entities;
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
                m => query.Phrase == null ||
                (
                    m.Name.ToLower().Contains(query.Phrase.ToLower())
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
    public Category Get(Guid id)
    {
        return getById(id);
    }

    private Category getById(Guid id)
    {
        return _context.Categories.FirstOrDefault(m => m.Id == id)!;
    }

    [HttpPost]
    public ActionResult Post([FromBody] CategoryCreateDto dto)
    {
        if (dto == null || dto.CategoryName == null) return BadRequest();

        var category = _context.Categories.FirstOrDefault(x => x.Name == dto.CategoryName);
        if (category != null) return BadRequest();

        var newCategory = new Category { Name = dto.CategoryName };
        _context.Categories.Add(newCategory);
        _context.SaveChanges();

        return Created($"/api/admin/categories/{newCategory.Id}", null);
    }

    [HttpPut("{id}")]
    public ActionResult Put(Guid id, [FromBody] CategoryCreateDto dto)
    {
        if (dto == null || dto.CategoryName == null) return BadRequest("Invalid category data.");

        var existingCategory = getById(id);

        if (existingCategory == null)
        {
            return NotFound($"Category with id {id} not found.");
        }

        existingCategory.Name = dto.CategoryName;

        _context.SaveChanges();

        return Ok(existingCategory);
    }

    [HttpDelete("{id}")]
    public void Delete(Guid id)
    {
        var category = getById(id);
        if (category == null) return;

        _context.Categories.Remove(category);
        _context.SaveChanges();
    }
}

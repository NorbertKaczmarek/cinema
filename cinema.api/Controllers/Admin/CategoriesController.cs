using cinema.api.Models;
using cinema.context;
using cinema.context.Entities;
using Microsoft.AspNetCore.Mvc;

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
    public IEnumerable<Category> Get()
    {
        return _context.Categories.ToList();
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

    [HttpDelete("{id}")]
    public void Delete(Guid id)
    {
        var category = getById(id);
        if (category == null) return;

        _context.Categories.Remove(category);
        _context.SaveChanges();
    }
}

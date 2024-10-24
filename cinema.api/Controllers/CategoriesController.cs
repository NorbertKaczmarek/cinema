using cinema.context;
using cinema.context.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cinema.api.Controllers;

[Route("api/[controller]")]
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
    public ActionResult Post([FromBody] string categoryName)
    {
        // TODO unique Name
        if (categoryName == null) return BadRequest();
        var newCategory = new Category { Name = categoryName };

        _context.Categories.Add(newCategory);
        _context.SaveChanges();
        
        return Created($"/api/Categories/{newCategory.Id}", null);
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

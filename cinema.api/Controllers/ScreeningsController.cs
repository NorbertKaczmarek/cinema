using cinema.context.Entities;
using cinema.context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cinema.api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ScreeningsController : ControllerBase
{
    private readonly CinemaDbContext _context;

    public ScreeningsController(CinemaDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IEnumerable<Screening> Get()
    {
        return _context
            .Screenings
            .Include(s => s.Movie)
            .ThenInclude(m => m.Category)
            .ToList();
    }

    [HttpGet("{id}")]
    public Screening Get(Guid id)
    {
        return getById(id);
    }

    private Screening getById(Guid id)
    {
        return _context
            .Screenings
            .Include(s => s.Movie)
            .ThenInclude(m => m.Category)
            .FirstOrDefault(m => m.Id == id)!;
    }

    [HttpPost]
    public void Post([FromBody] string value)
    {
        // TODO
    }

    [HttpDelete("{id}")]
    public void Delete(Guid id)
    {
        var screening = getById(id);
        if (screening == null) return;

        _context.Screenings.Remove(screening);
        _context.SaveChanges();
    }
}

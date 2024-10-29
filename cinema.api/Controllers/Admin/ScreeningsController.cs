using cinema.context.Entities;
using cinema.context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using cinema.api.Models;

namespace cinema.api.Controllers.Admin;

[Route("api/admin/[controller]")]
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
            .ThenInclude(m => m!.Category)
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
            .ThenInclude(m => m!.Category)
            .FirstOrDefault(m => m.Id == id)!;
    }

    [HttpPost]
    public ActionResult Post([FromBody] ScreeningCreateDto dto)
    {
        if (dto == null) return BadRequest();

        var screening = new Screening()
        {
            StartDateTime = dto.StartDateTime,
            EndDateTime = dto.EndDateTime,
            MovieId = dto.MovieId
        };

        _context.Screenings.Add(screening);
        _context.SaveChanges();
        return Created($"/api/admin/screenings/{screening.Id}", null);
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

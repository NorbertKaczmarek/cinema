using cinema.context;
using cinema.context.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cinema.api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
    private readonly CinemaDbContext _context;

    public MoviesController(CinemaDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IEnumerable<Movie> Get()
    {
        return _context.Movies.Include(m => m.Category).ToList();
    }

    [HttpGet("{id}")]
    public Movie Get(Guid id)
    {
        return getById(id);
    }

    private Movie getById(Guid id)
    {
        return _context.Movies.Include(m => m.Category).FirstOrDefault(m => m.Id == id)!;
    }

    [HttpPost]
    public void Post([FromBody] Movie movie)
    {
        // TODO
    }

    [HttpDelete("{id}")]
    public void Delete(Guid id)
    {
        var movie = getById(id);
        if (movie == null) return;

        _context.Movies.Remove(movie);
        _context.SaveChanges();
    }
}

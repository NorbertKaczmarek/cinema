using cinema.api.Models;
using cinema.context;
using cinema.context.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace cinema.api.Controllers.Admin;

[Route("api/admin/[controller]")]
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
    public ActionResult Post([FromBody] MovieCreateDto dto)
    {
        if (dto == null) return BadRequest();

        var newMovie = new Movie()
        {
            Title = dto.Title,
            Duration = dto.Duration,
            PosterUrl = dto.PosterUrl,
            Director = dto.Director,
            Cast = dto.Cast,
            Description = dto.Description,
            Rating = dto.Rating,
            Category = _context.Categories.FirstOrDefault(x => x.Name == dto.CategoryName)
        };

        _context.Movies.Add(newMovie);
        _context.SaveChanges();

        return Created($"/api/admin/movies/{newMovie.Id}", null);
    }

    [HttpDelete("{id}")]
    public void Delete(Guid id)
    {
        var movie = _context.Movies.FirstOrDefault(m => m.Id == id)!;
        if (movie == null) return;

        _context.Movies.Remove(movie);
        _context.SaveChanges();
    }
}

using cinema.api.Models;
using cinema.context;
using cinema.context.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    public PageResult<Movie> Get([FromQuery] PageQuery query)
    {
        var baseQuery = _context
            .Movies
            .Include(m => m.Category)
            .Where(
                m => query.Phrase == null ||
                (
                    m.Title.ToLower().Contains(query.Phrase.ToLower())
                )
            );

        var totalCount = baseQuery.Count();

        var result = baseQuery
            .Skip(query.Size * query.Page)
            .Take(query.Size)
            .ToList();

        return new PageResult<Movie>(result, totalCount, query.Size);
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
            DurationMinutes = dto.DurationMinutes,
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

    [HttpPut("{id}")]
    public ActionResult Put(Guid id, [FromBody] MovieCreateDto dto)
    {
        if (dto == null) return BadRequest("Invalid movie data.");

        var existingMovie = getById(id);

        if (existingMovie == null)
        {
            return NotFound($"Movie with id {id} not found.");
        }

        existingMovie.Title = dto.Title;
        existingMovie.DurationMinutes = dto.DurationMinutes;
        existingMovie.PosterUrl = dto.PosterUrl;
        existingMovie.Director = dto.Director;
        existingMovie.Cast = dto.Cast;
        existingMovie.Description = dto.Description;
        existingMovie.Rating = dto.Rating;
        existingMovie.Category = _context.Categories.FirstOrDefault(x => x.Name == dto.CategoryName);

        _context.SaveChanges();

        return Ok(existingMovie);
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

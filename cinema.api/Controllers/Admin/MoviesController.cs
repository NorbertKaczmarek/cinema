﻿using cinema.api.Models;
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

    /// <summary>
    /// Retrieves all movies with optional pagination and search functionality.
    /// </summary>
    /// <returns>A paginated list of movies or a full list if Size is set to 0.</returns>
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

        List<Movie> result;

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

        return new PageResult<Movie>(result, totalCount, query.Size);
    }

    /// <summary>
    /// Retrieves a specific movie by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the movie to retrieve.</param>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Movie), 200)]
    [ProducesResponseType(typeof(string), 404)]
    public ActionResult Get(Guid id)
    {
        var movie = getById(id);
        if (movie is null) return NotFound("Movie with that id was not found.");
        return Ok(movie);
    }

    private Movie? getById(Guid id)
    {
        return _context.Movies.Include(m => m.Category).FirstOrDefault(m => m.Id == id)!;
    }

    /// <summary>
    /// Creates a new movie.
    /// </summary>
    /// <param name="dto">Data transfer object containing movie details.</param>
    [HttpPost]
    [ProducesResponseType(typeof(Movie), 201)]
    [ProducesResponseType(typeof(string), 400)]
    public ActionResult Post([FromBody] MovieCreateDto dto)
    {
        if (dto == null) return BadRequest("Invalid movie data.");

        var newMovie = new Movie()
        {
            Title = dto.Title,
            DurationMinutes = dto.DurationMinutes,
            PosterUrl = dto.PosterUrl,
            TrailerUrl = dto.TrailerUrl,
            BackgroundUrl = dto.BackgroundUrl,
            Director = dto.Director,
            Cast = dto.Cast,
            Description = dto.Description,
            Rating = dto.Rating,
            CategoryId = dto.CategoryId
        };

        _context.Movies.Add(newMovie);
        _context.SaveChanges();

        return Created($"/api/admin/movies/{newMovie.Id}", newMovie);
    }

    /// <summary>
    /// Updates an existing movie.
    /// </summary>
    /// <param name="id">The ID of the movie to update.</param>
    /// <param name="dto">Data transfer object containing updated movie details.</param>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Category), 201)]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(string), 404)]
    public ActionResult Put(Guid id, [FromBody] MovieCreateDto dto)
    {
        if (dto == null) return BadRequest("Invalid movie data.");

        var existingMovie = getById(id);

        if (existingMovie == null) return NotFound($"Movie with id {id} not found.");

        existingMovie.Title = dto.Title;
        existingMovie.DurationMinutes = dto.DurationMinutes;
        existingMovie.PosterUrl = dto.PosterUrl;
        existingMovie.TrailerUrl = dto.TrailerUrl;
        existingMovie.BackgroundUrl = dto.BackgroundUrl;
        existingMovie.Director = dto.Director;
        existingMovie.Cast = dto.Cast;
        existingMovie.Description = dto.Description;
        existingMovie.Rating = dto.Rating;
        existingMovie.CategoryId = dto.CategoryId;

        _context.SaveChanges();

        return Created($"/api/admin/movies/{existingMovie.Id}", existingMovie);
    }

    /// <summary>
    /// Deletes an existing movie  by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the movie to delete.</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(string), 404)]
    public ActionResult Delete(Guid id)
    {
        var movie = getById(id);
        if (movie == null) return NotFound($"Movie with id {id} not found.");

        _context.Movies.Remove(movie);
        _context.SaveChanges();

        return NoContent();
    }
}

using AutoMapper;
using cinema.api.Public;
using cinema.context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cinema.api.Controllers.Public;

[ApiController]
[Route("api/user/movies")]
[ApiExplorerSettings(GroupName = "User")]
public class MoviesController : ControllerBase
{
    private readonly CinemaDbContext _context;
    private readonly IMapper _mapper;

    public MoviesController(CinemaDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PublicMovieDto>> Get()
    {
        var result = _context
            .Movies
            .Include(m => m.Category)
            .Include(m => m.Screenings)
            .OrderByDescending(m => m.CreatedOnUtc)
            .ToList();

        var resultDto = _mapper.Map<List<PublicMovieDto>>(result);

        return Ok(resultDto);
    }

    [HttpGet("{id}")]
    public ActionResult<PublicMovieDto> Get(Guid id)
    {
        var movie = _context
            .Movies
            .Include(m => m.Category)
            .Include(m => m.Screenings)
            .FirstOrDefault(m => m.Id == id)!;
        if (movie is null) return NotFound("Film o podanym ID nie został znaleziony.");

        var movieDto = _mapper.Map<PublicMovieDto>(movie);
        return Ok(movieDto);
    }
}

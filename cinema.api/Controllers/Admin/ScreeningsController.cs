using AutoMapper;
using cinema.api.Models.Admin;
using cinema.api.Models;
using cinema.context.Entities;
using cinema.context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace cinema.api.Controllers.Admin;

[ApiController]
[Route("api/admin/screenings")]
[ApiExplorerSettings(GroupName = "Admin")]
[Authorize(Roles = "User,Admin")]
public class ScreeningsController : ControllerBase
{
    private readonly CinemaDbContext _context;
    private readonly IMapper _mapper;

    public ScreeningsController(CinemaDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public PageResult<ScreeningDto> Get([FromQuery] PageQuery query)
    {
        DateTime.TryParse(query.Phrase, out var parsedDate);

        var baseQuery = _context
            .Screenings
            .Include(s => s.Movie)
            .ThenInclude(m => m!.Category)
            .Where(
                s => query.Phrase == null ||
                (
                    s.Movie!.Title.ToLower().Contains(query.Phrase.ToLower()) ||
                    s.StartDateTime.Date == parsedDate.Date
                )
            )
            .OrderByDescending(s => s.StartDateTime);

        var totalCount = baseQuery.Count();

        List<Screening> result;

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

        var resultDto = _mapper.Map<List<ScreeningDto>>(result);
        return new PageResult<ScreeningDto>(resultDto, totalCount, query.Size);
    }

    [HttpGet("{id}")]
    public ScreeningDto Get(Guid id)
    {
        var screening = getById(id);
        if (screening is null) return null!;

        var screeningDto = _mapper.Map<ScreeningDto>(screening);
        return screeningDto;
    }

    private Screening getById(Guid id)
    {
        return _context
            .Screenings
            .Include(s => s.Movie)
            .ThenInclude(m => m!.Category)
            .FirstOrDefault(m => m.Id == id)!;
    }

    [HttpGet("{id}/seats")]
    public SeatResult GetSeats(Guid id)
    {
        var allSeats = _context
            .Seats
            .ToList();

        var takenSeats = _context
            .Orders
            .Where(o => o.ScreeningId == id)
            .Include(o => o.Seats)
            .SelectMany(o => o.Seats!)
            .Select(s => s.Id)
            .ToHashSet();

        var seatDtos = allSeats
            .Select(s => new SeatResultDto
            {
                Id = s.Id,
                Row = s.Row,
                Number = s.Number,
                IsTaken = takenSeats.Contains(s.Id)
            })
            .OrderBy(s => s.Number)
            .OrderBy(s => s.Row)
            .ToList();

        var totalSeats = allSeats.Count;
        var takenSeatsCount = seatDtos.Count(seat => seat.IsTaken);
        var availableSeatsCount = totalSeats - takenSeatsCount;

        var seatResult = new SeatResult
        {
            TotalSeats = totalSeats,
            TakenSeats = takenSeatsCount,
            AvailableSeats = availableSeatsCount,
            Seats = seatDtos
        };

        return seatResult;
    }

    [HttpPost]
    public ActionResult Post([FromBody] ScreeningCreateDto dto)
    {
        if (dto == null) return BadRequest("Nieprawidłowe dane seansu.");

        var movie = _context.Movies.FirstOrDefault(x => x.Id == dto.MovieId);
        if (movie == null) return BadRequest("Film nie został znaleziony.");

        var newStartDateTime = dto.StartDateTime;
        var newEndDateTime = newStartDateTime + TimeSpan.FromMinutes(movie.DurationMinutes + 30);

        var bufferedNewStart = newStartDateTime - TimeSpan.FromMinutes(30);
        var bufferedNewEnd = newEndDateTime + TimeSpan.FromMinutes(30);

        var overlappingScreening = _context
            .Screenings
            .Any(s =>
                (bufferedNewStart < s.EndDateTime && bufferedNewEnd > s.StartDateTime)
            );

        if (overlappingScreening) return BadRequest("Czas seansu pokrywa się z innym seansem.");

        var screening = new Screening()
        {
            StartDateTime = newStartDateTime,
            EndDateTime = newEndDateTime,
            MovieId = dto.MovieId
        };

        _context.Screenings.Add(screening);
        _context.SaveChanges();

        var screeningDto = _mapper.Map<ScreeningDto>(screening);
        return Created($"/api/admin/screenings/{screeningDto.Id}", null);
    }

    [HttpPut("{id}")]
    public ActionResult Put(Guid id, [FromBody] ScreeningCreateDto dto)
    {
        var existingScreening = getById(id);
        if (existingScreening == null) return NotFound("Seans nie został znaleziony.");

        var movie = _context.Movies.FirstOrDefault(x => x.Id == dto.MovieId);
        if (movie == null) return BadRequest("Film nie został znaleziony.");

        var newStartDateTime = dto.StartDateTime;
        var newEndDateTime = newStartDateTime + TimeSpan.FromMinutes(movie.DurationMinutes + 30);

        var overlappingScreening = _context
            .Screenings
            .Where(s => s.Id != id)
            .Any(s =>
                s.StartDateTime < newEndDateTime &&
                s.EndDateTime > newStartDateTime
            );

        if (overlappingScreening) return BadRequest("Czas seansu pokrywa się z innym seansem.");

        existingScreening.StartDateTime = newStartDateTime;
        existingScreening.EndDateTime = newEndDateTime;
        existingScreening.MovieId = dto.MovieId;

        _context.SaveChanges();

        var screeningDto = _mapper.Map<ScreeningDto>(existingScreening);
        return Ok(screeningDto);
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

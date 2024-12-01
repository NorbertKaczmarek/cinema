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
    public PageResult<Screening> Get([FromQuery] PageQuery query)
    {
        var baseQuery = _context
            .Screenings
            .Include(s => s.Movie)
            .ThenInclude(m => m!.Category)
            .Where(
                s => query.Phrase == null ||
                (
                    s!.Movie!.Title.ToLower().Contains(query.Phrase.ToLower())
                )
            );

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

        return new PageResult<Screening>(result, totalCount, query.Size);
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
            .Select(s => new SeatDto
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

    [HttpPut("{id}")]
    public ActionResult Put(Guid id, [FromBody] ScreeningCreateDto dto)
    {
        var existingScreening = getById(id);
        if (existingScreening == null) return NotFound("Screening not found.");

        existingScreening.StartDateTime = dto.StartDateTime;
        existingScreening.EndDateTime = dto.EndDateTime;
        existingScreening.MovieId = dto.MovieId;

        _context.SaveChanges();
        return Ok(existingScreening);
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

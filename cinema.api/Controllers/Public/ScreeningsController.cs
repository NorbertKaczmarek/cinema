using AutoMapper;
using cinema.api.Models;
using cinema.context;
using cinema.context.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cinema.api.Controllers.Public;

[ApiController]
[Route("api/user/screenings")]
[ApiExplorerSettings(GroupName = "User")]
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
    public ActionResult<IEnumerable<ScreeningDto>> Get(DateTimeOffset startDate, DateTimeOffset endDate)
    {
        if (startDate > endDate)
        {
            return BadRequest("Start date must be earlier than end date.");
        }

        var result = _context
            .Screenings
            .Include(s => s.Movie)
            .ThenInclude(m => m!.Category)
            .Where(s => s.StartDateTime >= startDate && s.EndDateTime <= endDate)
            .OrderBy(s => s.StartDateTime)
            .ToList();

        var resultDto = _mapper.Map<List<ScreeningDto>>(result);

        return Ok(resultDto);
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
}

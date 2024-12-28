using AutoMapper;
using cinema.api.Models;
using cinema.context;
using cinema.context.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace cinema.api.Controllers.Public;

[Route("api/user/screenings")]
[ApiController]
public class ScreeningsUserController : ControllerBase
{
    private readonly CinemaDbContext _context;
    private readonly IMapper _mapper;

    public ScreeningsUserController(CinemaDbContext context, IMapper mapper)
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
}

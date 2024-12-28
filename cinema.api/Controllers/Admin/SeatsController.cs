using AutoMapper;
using cinema.api.Models;
using cinema.context;
using cinema.context.Entities;
using Microsoft.AspNetCore.Mvc;
using Mono.TextTemplating;

namespace cinema.api.Controllers.Admin;

[ApiController]
[Route("api/admin/seats")]
[ApiExplorerSettings(GroupName = "Admin")]
public class SeatsController : ControllerBase
{
    private readonly CinemaDbContext _context;
    private readonly IMapper _mapper;

    public SeatsController(CinemaDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves all seats.
    /// </summary>
    /// <returns>A list of all seats in the database.</returns>
    [HttpGet]
    public ActionResult<IEnumerable<SeatDto>> Get()
    {
        var seats = _context.Seats.ToList();

        var resultDto = _mapper.Map<List<SeatDto>>(seats);

        return Ok(resultDto);
    }

    /// <summary>
    /// Retrieves a specific seat by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the seat to retrieve.</param>
    /// <returns>The seat if found, or a 404 status if not found.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(SeatDto), 200)]
    [ProducesResponseType(typeof(string), 404)]
    public ActionResult<SeatDto> Get(Guid id)
    {
        var seat = _context.Seats.FirstOrDefault(s => s.Id == id);

        if (seat == null) return NotFound($"Seat with ID {id} not found.");

        var resultDto = _mapper.Map<SeatDto>(seat);

        return Ok(resultDto);
    }

    /// <summary>
    /// Retrieves a specific seat by its row and number.
    /// </summary>
    /// <param name="row">The row letter of the seat.</param>
    /// <param name="number">The number of the seat in the specified row.</param>
    /// <returns>The seat if found, or a 404 status if not found.</returns>
    [HttpGet("{row}/{number}")]
    [ProducesResponseType(typeof(Seat), 200)]
    [ProducesResponseType(typeof(string), 404)]
    public ActionResult<SeatDto> GetByRowAndNumber(char row, int number)
    {
        var seat = _context.Seats.FirstOrDefault(s => s.Number == number && s.Row == row);

        if (seat == null) return NotFound($"Seat in row '{row}' with number '{number}' not found.");

        var resultDto = _mapper.Map<SeatDto>(seat);

        return Ok(resultDto);
    }
}

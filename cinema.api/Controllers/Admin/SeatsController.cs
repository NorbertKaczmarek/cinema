using cinema.context;
using cinema.context.Entities;
using Microsoft.AspNetCore.Mvc;

namespace cinema.api.Controllers.Admin;

[Route("api/admin/[controller]")]
[ApiController]
public class SeatsController : ControllerBase
{
    private readonly CinemaDbContext _context;

    public SeatsController(CinemaDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all seats.
    /// </summary>
    /// <returns>A list of all seats in the database.</returns>
    [HttpGet]
    public ActionResult<IEnumerable<Seat>> Get()
    {
        var seats = _context.Seats.ToList();
        return Ok(seats);
    }

    /// <summary>
    /// Retrieves a specific seat by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the seat to retrieve.</param>
    /// <returns>The seat if found, or a 404 status if not found.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Seat), 200)]
    [ProducesResponseType(typeof(string), 404)]
    public ActionResult<Seat> Get(Guid id)
    {
        var seat = _context.Seats.FirstOrDefault(s => s.Id == id);

        if (seat == null) return NotFound($"Seat with ID {id} not found.");

        return Ok(seat);
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
    public ActionResult<Seat> GetByRowAndNumber(char row, int number)
    {
        var seat = _context.Seats.FirstOrDefault(s => s.Number == number && s.Row == row);

        if (seat == null) return NotFound($"Seat in row '{row}' with number '{number}' not found.");

        return Ok(seat);
    }
}

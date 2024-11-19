using cinema.context.Entities;
using cinema.context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using cinema.api.Models;

namespace cinema.api.Controllers.Admin;

[Route("api/admin/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly CinemaDbContext _context;

    public OrdersController(CinemaDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public PageResult<Order> Get([FromQuery] PageQuery query)
    {
        var baseQuery = _context
            .Orders
            .Include(o => o.Seats)
            .Include(o => o.Screening)
            .ThenInclude(s => s!.Movie)
            .ThenInclude(m => m!.Category)
            .Where(
                o => query.Phrase == null ||
                (
                    o.Email.ToLower().Contains(query.Phrase.ToLower())
                )
            );

        var totalCount = baseQuery.Count();

        List<Order> result;

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

        return new PageResult<Order>(result, totalCount, query.Size);
    }

    [HttpGet("{id}")]
    public Order Get(Guid id)
    {
        return getById(id);
    }

    private Order getById(Guid id)
    {
        return _context
            .Orders
            .Include(o => o.Seats)
            .Include(o => o.Screening)
            .ThenInclude(s => s!.Movie)
            .ThenInclude(m => m!.Category)
            .FirstOrDefault(o => o.Id == id)!;
    }

    [HttpPost]
    public ActionResult Post([FromBody] OrderCreateDto dto)
    {
        if (dto == null) return BadRequest("Invalid order data.");

        var screening = _context.Screenings.FirstOrDefault(s => s.Id == dto.ScreeningId);
        if (screening == null) return BadRequest("Invalid screening ID.");

        var requestedSeats = _context.Seats.Where(s => dto.SeatIds.Contains(s.Id)).ToList();
        if (requestedSeats.Count != dto.SeatIds.Count) return BadRequest("One or more seat IDs are invalid.");

        var takenSeatIds = _context.Orders
            .Where(o => o.ScreeningId == dto.ScreeningId)
            .SelectMany(o => o.Seats!)
            .Select(s => s.Id)
            .ToHashSet();

        var takenSeats = requestedSeats.Where(seat => takenSeatIds.Contains(seat.Id)).ToList();
        if (takenSeats.Any())
        {
            var takenSeatNumbers = string.Join(", ", takenSeats.Select(s => $"{s.Row}{s.Number}"));
            return BadRequest($"The following seats are already taken: {takenSeatNumbers}");
        }

        var newOrder = new Order
        {
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Status = dto.Status,
            Screening = screening,
            Seats = requestedSeats,
        };

        _context.Orders.Add(newOrder);
        _context.SaveChanges();

        return Created($"/api/admin/orders/{newOrder.Id}", newOrder);
    }

    [HttpPut("{id}")]
    public ActionResult Put(Guid id, [FromBody] OrderCreateDto dto)
    {
        if (dto == null) return BadRequest("Invalid order data.");

        var existingOrder = getById(id);
        if (existingOrder == null) return NotFound($"Order with id {id} not found.");

        var newScreening = _context.Screenings.FirstOrDefault(s => s.Id == dto.ScreeningId);
        if (newScreening == null) return BadRequest("Invalid screening ID.");

        existingOrder.Screening = newScreening;

        var newSeats = _context.Seats.Where(s => dto.SeatIds.Contains(s.Id)).ToList();
        if (newSeats.Count != dto.SeatIds.Count) return BadRequest("One or more seat IDs are invalid.");

        existingOrder.Seats = newSeats;

        existingOrder.Email = dto.Email;
        existingOrder.PhoneNumber = dto.PhoneNumber;
        existingOrder.Status = dto.Status;

        _context.SaveChanges();

        return Ok(existingOrder);
    }

    [HttpDelete("{id}")]
    public void Delete(Guid id)
    {
        var order = getById(id);
        if (order == null) return;

        _context.Orders.Remove(order);
        _context.SaveChanges();
    }
}

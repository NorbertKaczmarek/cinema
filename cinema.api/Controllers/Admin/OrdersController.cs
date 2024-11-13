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
    public IEnumerable<Order> Get()
    {
        return _context
            .Orders
            .Include(o => o.Seats)
            .Include(o => o.Screening)
            .ThenInclude(s => s!.Movie)
            .ThenInclude(m => m!.Category)
            .ToList();
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

        var seats = _context.Seats.Where(s => dto.SeatIds.Contains(s.Id)).ToList();
        if (seats.Count != dto.SeatIds.Count) return BadRequest("One or more seat IDs are invalid.");

        var newOrder = new Order
        {
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Status = dto.Status,
            Screening = screening,
            Seats = seats,
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

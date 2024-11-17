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

    [HttpGet]
    public IEnumerable<Seat> Get()
    {
        return _context.Seats.ToList();
    }

    [HttpGet("{id}")]
    public Seat Get(Guid id)
    {
        return _context.Seats.FirstOrDefault(s => s.Id == id)!;
    }

    [HttpGet("{row}/{number}")]
    public Seat GetByRowAndNumber(char row, int number)
    {
        return _context.Seats.FirstOrDefault(s => s.Number == number && s.Row == row)!;
    }
}

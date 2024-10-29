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
        return getById(id);
    }

    private Seat getById(Guid id)
    {
        return _context.Seats.FirstOrDefault(m => m.Id == id)!;
    }

    [HttpPost]
    public void Post([FromBody] string value)
    {
        // TODO
    }

    [HttpDelete("{id}")]
    public void Delete(Guid id)
    {
        var seat = getById(id);
        if (seat == null) return;

        _context.Seats.Remove(seat);
        _context.SaveChanges();
    }
}

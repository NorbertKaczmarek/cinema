using cinema.context.Entities;
using cinema.context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cinema.api.Controllers;

[Route("api/[controller]")]
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
        return _context.Orders.ToList();
    }

    [HttpGet("{id}")]
    public Order Get(Guid id)
    {
        return getById(id);
    }

    private Order getById(Guid id)
    {
        return _context.Orders.FirstOrDefault(m => m.Id == id)!;
    }

    [HttpPost]
    public void Post([FromBody] string value)
    {
        // TODO
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

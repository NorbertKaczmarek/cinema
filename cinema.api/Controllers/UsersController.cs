using cinema.context.Entities;
using cinema.context;
using Microsoft.AspNetCore.Mvc;

namespace cinema.api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly CinemaDbContext _context;

    public UsersController(CinemaDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IEnumerable<User> Get()
    {
        return _context.Users.ToList();
    }

    [HttpGet("{id}")]
    public User Get(Guid id)
    {
        return getById(id);
    }

    private User getById(Guid id)
    {
        return _context.Users.FirstOrDefault(m => m.Id == id)!;
    }

    [HttpPost]
    public void Post([FromBody] string value)
    {
        // TODO
    }

    [HttpDelete("{id}")]
    public void Delete(Guid id)
    {
        var user = getById(id);
        if (user == null) return;

        _context.Users.Remove(user);
        _context.SaveChanges();
    }
}

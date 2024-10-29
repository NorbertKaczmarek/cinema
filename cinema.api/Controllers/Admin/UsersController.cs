using cinema.context.Entities;
using cinema.context;
using Microsoft.AspNetCore.Mvc;
using cinema.api.Models;
using Microsoft.AspNetCore.Identity;

namespace cinema.api.Controllers.Admin;

[Route("api/admin/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly CinemaDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UsersController(CinemaDbContext context, IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
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

    private User? getUserByEmail(string email)
    {
        var user = _context
            .Users
            .FirstOrDefault(u => u.Email == email);

        return user;
    }

    [HttpPost]
    public ActionResult Post([FromBody] UserCreateDto dto)
    {
        if (getUserByEmail(dto.Email) != null) return BadRequest();
        if (dto.Password != dto.ConfirmPassword) return BadRequest();

        var newUser = new User()
        {
            IsAdmin = dto.IsAdmin,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            PasswordHash = ""
        };
        var hashedPasword = _passwordHasher.HashPassword(newUser, dto.Password);
        newUser.PasswordHash = hashedPasword;
        _context.Users.Add(newUser);
        _context.SaveChanges();

        return Ok();
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

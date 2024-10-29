using cinema.api.Models;
using cinema.context;
using cinema.context.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace cinema.api.Controllers.Admin;

[Route("api/admin/[controller]")]
[ApiController]
[Authorize]
public class AuthController : ControllerBase
{
    private readonly CinemaDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly AuthenticationOptions _options;

    public AuthController(CinemaDbContext context, IPasswordHasher<User> passwordHasher, AuthenticationOptions options)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _options = options;
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

    [HttpPost("login")]
    [AllowAnonymous]
    public ActionResult<string> Login([FromBody] UserLoginDto dto)
    {
        var user = getUserByEmail(dto.Email);
        if (user is null) return BadRequest();

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
        if (result == PasswordVerificationResult.Failed) return BadRequest();

        return Ok(generateJSONWebToken(user));
    }

    private User? getUserByEmail(string email)
    {
        var user = _context
            .Users
            .FirstOrDefault(u => u.Email == email);

        return user;
    }

    private string generateJSONWebToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.IssuerSigningKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _options.ValidIssuer,
            audience: _options.ValidAudience,
            claims: new List<Claim>
            {
                new Claim("Id", user.Id.ToString()),
                new Claim("FullName", string.Concat(user.FirstName, " ", user.LastName)),
            },
            notBefore: null,
            expires: DateTime.UtcNow.AddHours(6),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

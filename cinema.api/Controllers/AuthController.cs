using cinema.api.Helpers;
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
using System.Security.Cryptography;
using System.Text;

using static System.Convert;

namespace cinema.api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AuthController : ControllerBase
{
    private readonly CinemaDbContext _context;
    private readonly AuthenticationOptions _options;

    public AuthController(CinemaDbContext context, AuthenticationOptions options)
    {
        _context = context;
        _options = options;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public ActionResult<string> Login([FromBody] UserLoginDto dto)
    {
        var user = getUserByEmail(dto.Email);
        if (user is null) return BadRequest();

        var result = SalterAndHasher.CheckPassword(dto.Password, user.Salt, user.SaltedHashedPassword);
        if (result == false) return BadRequest();

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

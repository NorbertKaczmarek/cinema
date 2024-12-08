﻿using cinema.context.Entities;
using cinema.context;
using Microsoft.AspNetCore.Mvc;
using cinema.api.Models;
using cinema.api.Helpers;

namespace cinema.api.Controllers.Admin;

[Route("api/admin/[controller]")]
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
        if (getUserByEmail(dto.Email) != null) return BadRequest("User already exists.");
        if (string.IsNullOrWhiteSpace(dto.Password)) return BadRequest("Password is empty.");
        if (dto.Password != dto.ConfirmPassword) return BadRequest("Passwords do not match.");

        (var saltText, var saltedHashedPassword) = SalterAndHasher.getSaltAndSaltedHashedPassword(dto.Password);

        var newUser = new User()
        {
            IsAdmin = dto.IsAdmin,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Salt = saltText,
            SaltedHashedPassword = saltedHashedPassword,
        };
        _context.Users.Add(newUser);
        _context.SaveChanges();

        return Ok();
    }

    [HttpPut("{id}")]
    public ActionResult Put(Guid id, [FromBody] UserCreateDto dto)
    {
        var existingUser = getById(id);
        if (existingUser == null) return NotFound("User not found.");
        
        existingUser.FirstName = dto.FirstName;
        existingUser.LastName = dto.LastName;
        existingUser.IsAdmin = dto.IsAdmin;

        if (string.IsNullOrWhiteSpace(dto.Password)) return BadRequest("Password is empty.");
        if (dto.Password != dto.ConfirmPassword) return BadRequest("Passwords do not match.");

        (var saltText, var saltedHashedPassword) = SalterAndHasher.getSaltAndSaltedHashedPassword(dto.Password);
        existingUser.Salt = saltText;
        existingUser.SaltedHashedPassword = saltedHashedPassword;

        _context.SaveChanges();
        return Ok(existingUser);
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

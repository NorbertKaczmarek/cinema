﻿namespace cinema.context.Entities;

public class User
{
    public Guid Id { get; set; }
    public bool IsAdmin { get; set; }
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PasswordHash { get; set; }
}

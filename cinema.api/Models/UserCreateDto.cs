﻿namespace cinema.api.Models;

public class UserCreateDto
{
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}

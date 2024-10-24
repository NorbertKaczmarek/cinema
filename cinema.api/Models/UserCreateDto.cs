namespace cinema.api.Models;

public class UserCreateDto
{
    public bool IsAdmin { get; set; }
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Password { get; set; }
    public required string ConfirmPassword { get; set; }
}

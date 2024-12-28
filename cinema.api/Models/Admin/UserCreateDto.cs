namespace cinema.api.Models.Admin;

public class UserCreateDto
{
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}

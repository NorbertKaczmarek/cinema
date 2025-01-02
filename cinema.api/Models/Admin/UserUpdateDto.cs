namespace cinema.api.Models.Admin;

public class UserUpdateDto
{
    public required string Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public required string Password { get; set; }
    public string? NewPassword { get; set; }
    public string? ConfirmNewPassword { get; set; }
}

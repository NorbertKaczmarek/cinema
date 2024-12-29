namespace cinema.api.Models.Public;

public class PublicOrderCreateDto
{
    public required string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public Guid ScreeningId { get; set; }
    public List<Guid> SeatIds { get; set; } = new List<Guid>();
}

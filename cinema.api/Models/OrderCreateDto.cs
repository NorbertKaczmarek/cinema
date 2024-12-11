namespace cinema.api.Models;

public class OrderCreateDto
{
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public string Status { get; set; }
    public Guid ScreeningId { get; set; }
    public List<Guid> SeatIds { get; set; } = new List<Guid>();
}

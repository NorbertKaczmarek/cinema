namespace cinema.api.Models;

public class OrderUpdateDto
{
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Status { get; set; }
    public Guid? ScreeningId { get; set; }
    public List<Guid>? SeatIds { get; set; } = new List<Guid>();
}

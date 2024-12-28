namespace cinema.api.Models.Admin;

public class OrderCreateDto
{
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Status { get; set; }
    public Guid ScreeningId { get; set; }
    public List<Guid> SeatIds { get; set; } = new List<Guid>();
}

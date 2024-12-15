using cinema.context.Entities;

namespace cinema.api.Models;

public class OrderDto
{
    public Guid Id { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Status { get; set; }
    public Guid? ScreeningId { get; set; }
    public virtual Screening? Screening { get; set; }
    public virtual List<Seat>? Seats { get; set; }
}

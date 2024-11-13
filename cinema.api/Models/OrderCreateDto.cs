using cinema.context.Entities;
using System.Text.Json.Serialization;

namespace cinema.api.Models;

public class OrderCreateDto
{
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OrderStatus Status { get; set; }
    public Guid ScreeningId { get; set; }
    public List<Guid> SeatIds { get; set; } = new List<Guid>();
}

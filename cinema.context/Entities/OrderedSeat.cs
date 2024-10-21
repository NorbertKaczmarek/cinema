using System.ComponentModel.DataAnnotations.Schema;

namespace cinema.context.Entities;

public class OrderedSeat
{
    [Column(TypeName = "char(36)")]
    public Guid Id { get; set; }

    [Column(TypeName = "char(36)")]
    public Guid SeatId { get; set; }

    [Column(TypeName = "char(36)")]
    public Guid OrderId { get; set; }
}

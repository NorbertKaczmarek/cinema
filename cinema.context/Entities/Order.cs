using System.ComponentModel.DataAnnotations.Schema;

namespace cinema.context.Entities;

public enum OrderStatus
{
    Pending,
    Ready,
    Cancelled
}

public class Order
{
    [Column(TypeName = "char(36)")]
    public Guid Id { get; set; }


    [Column(TypeName = "varchar(100)")]
    public required string Email { get; set; }


    [Column(TypeName = "varchar(100)")]
    public required string PhoneNumber { get; set; }


    [Column(TypeName = "varchar(100)")]
    public OrderStatus Status { get; set; }

    [Column(TypeName = "char(36)")]
    public Guid ScreeningId { get; set; }
}

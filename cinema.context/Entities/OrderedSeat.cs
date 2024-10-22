namespace cinema.context.Entities;

public class OrderedSeat
{
    public Guid Id { get; set; }
    public Guid SeatId { get; set; }
    public Guid OrderId { get; set; }
}

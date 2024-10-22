﻿namespace cinema.context.Entities;

public enum OrderStatus
{
    Pending,
    Ready,
    Cancelled
}

public class Order
{
    public Guid Id { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public OrderStatus Status { get; set; }
    public Guid ScreeningId { get; set; }
}

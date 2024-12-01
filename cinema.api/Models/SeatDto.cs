namespace cinema.api.Models;

public class SeatDto
{
    public Guid Id { get; set; }
    public char Row { get; set; }
    public int Number { get; set; }
    public bool IsTaken { get; set; }
}

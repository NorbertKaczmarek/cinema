namespace cinema.api.Models;

public class SeatResult
{
    public int TotalSeats { get; set; }
    public int TakenSeats { get; set; }
    public int AvailableSeats { get; set; }
    public required List<SeatResultDto> Seats { get; set; }
}
public class SeatResultDto
{
    public Guid Id { get; set; }
    public char Row { get; set; }
    public int Number { get; set; }
    public bool IsTaken { get; set; }
}
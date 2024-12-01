namespace cinema.api.Models;

public class SeatResult
{
    public int TotalSeats { get; set; }
    public int TakenSeats { get; set; }
    public int AvailableSeats { get; set; }
    public required List<SeatDto> Seats { get; set; }
}

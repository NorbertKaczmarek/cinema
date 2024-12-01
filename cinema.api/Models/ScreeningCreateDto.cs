namespace cinema.api.Models;

public class ScreeningCreateDto
{
    public DateTimeOffset StartDateTime { get; set; }
    public DateTimeOffset EndDateTime { get; set; }
    public Guid MovieId { get; set; }
}

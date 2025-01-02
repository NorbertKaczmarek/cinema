namespace cinema.api.Models.Admin;

public class ScreeningCreateDto
{
    public DateTimeOffset StartDateTime { get; set; }
    public Guid MovieId { get; set; }
}

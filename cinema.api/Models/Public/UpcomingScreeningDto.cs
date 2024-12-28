namespace cinema.api.Models.Public;

public class UpcomingScreeningDto
{
    public Guid Id { get; set; }
    public DateTimeOffset StartDateTime { get; set; }
    public DateTimeOffset EndDateTime { get; set; }
}

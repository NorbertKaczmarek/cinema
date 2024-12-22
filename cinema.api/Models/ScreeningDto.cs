using cinema.context.Entities;

namespace cinema.api.Models;

public class ScreeningDto
{
    public Guid Id { get; set; }
    public DateTimeOffset StartDateTime { get; set; }
    public DateTimeOffset EndDateTime { get; set; }
    public Guid? MovieId { get; set; }
    public virtual Movie? Movie { get; set; }
}

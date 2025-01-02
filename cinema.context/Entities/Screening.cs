namespace cinema.context.Entities;

public class Screening : IAuditableEntity
{
    public Guid Id { get; set; }
    public DateTimeOffset StartDateTime { get; set; }
    public DateTimeOffset EndDateTime { get; set; }
    public Guid? MovieId { get; set; }
    public virtual Movie? Movie { get; set; }
    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }
}

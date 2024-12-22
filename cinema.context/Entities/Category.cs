namespace cinema.context.Entities;

public class Category : IAuditableEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }
}

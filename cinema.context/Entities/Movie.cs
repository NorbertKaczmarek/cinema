﻿namespace cinema.context.Entities;

public class Movie : IAuditableEntity
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public int DurationMinutes { get; set; }
    public required string PosterUrl { get; set; }
    public required string TrailerUrl { get; set; }
    public required string BackgroundUrl { get; set; }
    public required string Director { get; set; }
    public required string Cast { get; set; }
    public required string Description { get; set; }
    public double Rating { get; set; }
    public Guid? CategoryId {  get; set; }
    public virtual Category? Category { get; set; }
    public virtual ICollection<Screening> Screenings { get; set; } = new List<Screening>();
    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }
}

using cinema.context.Entities;

namespace cinema.api.Public;

public class PublicMovieDto
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
    public Guid? CategoryId { get; set; }
    public virtual Category? Category { get; set; }
    public List<Screening>? UpcomingScreenings { get; set; }
}

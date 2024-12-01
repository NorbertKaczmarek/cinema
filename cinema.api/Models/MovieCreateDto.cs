namespace cinema.api.Models;

public class MovieCreateDto
{
    public required string Title { get; set; }
    public int DurationMinutes { get; set; }
    public required string PosterUrl { get; set; }
    public required string TrailerUrl { get; set; }
    public required string BackgroundUrl { get; set; }
    public required string Director { get; set; }
    public required string Cast { get; set; }
    public required string Description { get; set; }
    public double Rating { get; set; }
    public Guid CategoryId { get; set; }
}

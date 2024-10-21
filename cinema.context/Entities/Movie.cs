using System.ComponentModel.DataAnnotations.Schema;

namespace cinema.context.Entities;

public class Movie
{
    [Column(TypeName = "char(36)")]
    public Guid Id { get; set; }

    [Column(TypeName = "varchar(100)")]
    public required string Title { get; set; }

    [Column(TypeName = "time(6)")]
    public TimeSpan Duration { get; set; }

    [Column(TypeName = "varchar(100)")]
    public required string PosterUrl { get; set; }

    [Column(TypeName = "varchar(100)")]
    public required string Director { get; set; }

    [Column(TypeName = "varchar(100)")]
    public required string Cast { get; set; }

    [Column(TypeName = "varchar(100)")]
    public required string Description { get; set; }

    [Column(TypeName = "double")]
    public double Rating { get; set; }

    [Column(TypeName = "char(36)")]
    public Guid CategoryId {  get; set; }
}

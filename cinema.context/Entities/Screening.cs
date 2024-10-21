using System.ComponentModel.DataAnnotations.Schema;

namespace cinema.context.Entities;

public class Screening
{
    [Column(TypeName = "char(36)")]
    public Guid Id { get; set; }

    [Column(TypeName = "datetime(6)")]
    public DateTimeOffset StartDateTime { get; set; }

    [Column(TypeName = "datetime(6)")]
    public DateTimeOffset EndDateTime { get; set; }

    [Column(TypeName = "char(36)")]
    public Guid MovieId { get; set; }
}

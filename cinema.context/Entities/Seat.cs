using System.ComponentModel.DataAnnotations.Schema;

namespace cinema.context.Entities;

public class Seat
{
    [Column(TypeName = "char(36)")]
    public Guid Id { get; set; }

    [Column(TypeName = "varchar(1)")]
    public char Row { get; set; }

    [Column(TypeName = "int")]
    public int Number { get; set; }
}

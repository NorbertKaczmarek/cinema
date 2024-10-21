using System.ComponentModel.DataAnnotations.Schema;

namespace cinema.context.Entities;

public class Category
{
    [Column(TypeName = "char(36)")]
    public Guid Id { get; set; }

    [Column(TypeName = "varchar(100)")]
    public required string Name { get; set; }
}

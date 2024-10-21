using System.ComponentModel.DataAnnotations.Schema;

namespace cinema.context.Entities;

public class User
{
    [Column(TypeName = "char(36)")]
    public Guid Id { get; set; }

    [Column(TypeName = "tinyint(1)")]
    public Boolean IsAdmin { get; set; }

    [Column(TypeName = "varchar(100)")]
    public required string Email { get; set; }

    [Column(TypeName = "varchar(100)")]
    public required string FirstName { get; set; }


    [Column(TypeName = "varchar(100)")]
    public required string LastName { get; set; }


    [Column(TypeName = "varchar(100)")]
    public required string PasswordHash { get; set; }
}

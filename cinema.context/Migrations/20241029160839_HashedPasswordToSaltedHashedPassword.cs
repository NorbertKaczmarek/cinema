using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cinema.context.Migrations
{
    /// <inheritdoc />
    public partial class HashedPasswordToSaltedHashedPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Users",
                newName: "SaltedHashedPassword");

            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "Users",
                type: "varchar(100)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salt",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "SaltedHashedPassword",
                table: "Users",
                newName: "PasswordHash");
        }
    }
}

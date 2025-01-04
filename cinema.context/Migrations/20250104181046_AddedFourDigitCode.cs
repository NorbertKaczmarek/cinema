using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cinema.context.Migrations
{
    /// <inheritdoc />
    public partial class AddedFourDigitCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FourDigitCode",
                table: "Orders",
                type: "varchar(4)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FourDigitCode",
                table: "Orders");
        }
    }
}

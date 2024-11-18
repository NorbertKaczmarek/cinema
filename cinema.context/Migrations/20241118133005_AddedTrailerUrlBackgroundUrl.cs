using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cinema.context.Migrations
{
    /// <inheritdoc />
    public partial class AddedTrailerUrlBackgroundUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BackgroundUrl",
                table: "Movies",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "TrailerUrl",
                table: "Movies",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackgroundUrl",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "TrailerUrl",
                table: "Movies");
        }
    }
}

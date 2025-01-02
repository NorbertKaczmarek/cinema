using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cinema.context.Migrations
{
    /// <inheritdoc />
    public partial class AddedAuditing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedOnUtc",
                table: "Users",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOnUtc",
                table: "Users",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedOnUtc",
                table: "Screenings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOnUtc",
                table: "Screenings",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedOnUtc",
                table: "Orders",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOnUtc",
                table: "Orders",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TrailerUrl",
                table: "Movies",
                type: "varchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "BackgroundUrl",
                table: "Movies",
                type: "varchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedOnUtc",
                table: "Movies",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOnUtc",
                table: "Movies",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedOnUtc",
                table: "Categories",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedOnUtc",
                table: "Categories",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOnUtc",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ModifiedOnUtc",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedOnUtc",
                table: "Screenings");

            migrationBuilder.DropColumn(
                name: "ModifiedOnUtc",
                table: "Screenings");

            migrationBuilder.DropColumn(
                name: "CreatedOnUtc",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ModifiedOnUtc",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CreatedOnUtc",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "ModifiedOnUtc",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "CreatedOnUtc",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ModifiedOnUtc",
                table: "Categories");

            migrationBuilder.AlterColumn<string>(
                name: "TrailerUrl",
                table: "Movies",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "BackgroundUrl",
                table: "Movies",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}

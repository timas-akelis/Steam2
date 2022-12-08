using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Steam2.Migrations
{
    public partial class FixGameBull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HoursPlayed",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "RecentHoursPlayer",
                table: "Game");

            migrationBuilder.AddColumn<double>(
                name: "HoursPlayed",
                table: "Library",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "RecentHoursPlayer",
                table: "Library",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HoursPlayed",
                table: "Library");

            migrationBuilder.DropColumn(
                name: "RecentHoursPlayer",
                table: "Library");

            migrationBuilder.AddColumn<double>(
                name: "HoursPlayed",
                table: "Game",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "RecentHoursPlayer",
                table: "Game",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}

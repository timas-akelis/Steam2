using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Steam2.Migrations
{
    public partial class whoknowshonestlywasworkingonthecart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GamesID",
                table: "Order",
                newName: "GameIDs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GameIDs",
                table: "Order",
                newName: "GamesID");
        }
    }
}

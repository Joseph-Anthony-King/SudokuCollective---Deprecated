using Microsoft.EntityFrameworkCore.Migrations;

namespace SudokuCollective.WebApi.Migrations
{
    public partial class AddAppIdToGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppId",
                table: "Games",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppId",
                table: "Games");
        }
    }
}

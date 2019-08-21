using Microsoft.EntityFrameworkCore.Migrations;

namespace SudokuApp.WebApp.Migrations
{
    public partial class AddMatrixToDifficulyRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DifficultyId",
                table: "SudokuMatrix",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SudokuMatrix_DifficultyId",
                table: "SudokuMatrix",
                column: "DifficultyId");

            migrationBuilder.AddForeignKey(
                name: "FK_SudokuMatrix_Difficulties_DifficultyId",
                table: "SudokuMatrix",
                column: "DifficultyId",
                principalTable: "Difficulties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SudokuMatrix_Difficulties_DifficultyId",
                table: "SudokuMatrix");

            migrationBuilder.DropIndex(
                name: "IX_SudokuMatrix_DifficultyId",
                table: "SudokuMatrix");

            migrationBuilder.DropColumn(
                name: "DifficultyId",
                table: "SudokuMatrix");
        }
    }
}

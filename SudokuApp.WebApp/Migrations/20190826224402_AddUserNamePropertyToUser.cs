using Microsoft.EntityFrameworkCore.Migrations;

namespace SudokuApp.WebApp.Migrations
{
    public partial class AddUserNamePropertyToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_SudokuMatrix_SudokuMatrixId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_SudokuCell_SudokuMatrix_SudokuMatrixId",
                table: "SudokuCell");

            migrationBuilder.DropForeignKey(
                name: "FK_SudokuMatrix_Difficulties_DifficultyId",
                table: "SudokuMatrix");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SudokuMatrix",
                table: "SudokuMatrix");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SudokuCell",
                table: "SudokuCell");

            migrationBuilder.RenameTable(
                name: "SudokuMatrix",
                newName: "SudokuMatrices");

            migrationBuilder.RenameTable(
                name: "SudokuCell",
                newName: "SudokuCells");

            migrationBuilder.RenameIndex(
                name: "IX_SudokuMatrix_DifficultyId",
                table: "SudokuMatrices",
                newName: "IX_SudokuMatrices_DifficultyId");

            migrationBuilder.RenameIndex(
                name: "IX_SudokuCell_SudokuMatrixId",
                table: "SudokuCells",
                newName: "IX_SudokuCells_SudokuMatrixId");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Users",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SudokuMatrices",
                table: "SudokuMatrices",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SudokuCells",
                table: "SudokuCells",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_SudokuMatrices_SudokuMatrixId",
                table: "Games",
                column: "SudokuMatrixId",
                principalTable: "SudokuMatrices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SudokuCells_SudokuMatrices_SudokuMatrixId",
                table: "SudokuCells",
                column: "SudokuMatrixId",
                principalTable: "SudokuMatrices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SudokuMatrices_Difficulties_DifficultyId",
                table: "SudokuMatrices",
                column: "DifficultyId",
                principalTable: "Difficulties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_SudokuMatrices_SudokuMatrixId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_SudokuCells_SudokuMatrices_SudokuMatrixId",
                table: "SudokuCells");

            migrationBuilder.DropForeignKey(
                name: "FK_SudokuMatrices_Difficulties_DifficultyId",
                table: "SudokuMatrices");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserName",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SudokuMatrices",
                table: "SudokuMatrices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SudokuCells",
                table: "SudokuCells");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "SudokuMatrices",
                newName: "SudokuMatrix");

            migrationBuilder.RenameTable(
                name: "SudokuCells",
                newName: "SudokuCell");

            migrationBuilder.RenameIndex(
                name: "IX_SudokuMatrices_DifficultyId",
                table: "SudokuMatrix",
                newName: "IX_SudokuMatrix_DifficultyId");

            migrationBuilder.RenameIndex(
                name: "IX_SudokuCells_SudokuMatrixId",
                table: "SudokuCell",
                newName: "IX_SudokuCell_SudokuMatrixId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SudokuMatrix",
                table: "SudokuMatrix",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SudokuCell",
                table: "SudokuCell",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_SudokuMatrix_SudokuMatrixId",
                table: "Games",
                column: "SudokuMatrixId",
                principalTable: "SudokuMatrix",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SudokuCell_SudokuMatrix_SudokuMatrixId",
                table: "SudokuCell",
                column: "SudokuMatrixId",
                principalTable: "SudokuMatrix",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SudokuMatrix_Difficulties_DifficultyId",
                table: "SudokuMatrix",
                column: "DifficultyId",
                principalTable: "Difficulties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

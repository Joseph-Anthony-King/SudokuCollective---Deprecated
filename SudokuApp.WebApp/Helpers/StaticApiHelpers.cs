using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SudokuApp.Models;
using SudokuApp.WebApp.Models.DataModel;

namespace SudokuApp.WebApp.Helpers {

    public static class StaticApiHelpers {

        public async static Task<List<SudokuCell>> ResetSudokuCells(Game game, ApplicationDbContext context) {

            // Reset the matrix sudoku cells
            game.SudokuMatrix.SudokuCells = null;

            var cells = await context.SudokuCells
                .Where(cell => cell.SudokuMatrix.Id == game.SudokuMatrixId)
                .OrderBy(cell => cell.Index)
                .ToListAsync();

            return cells;
        }
    }
}

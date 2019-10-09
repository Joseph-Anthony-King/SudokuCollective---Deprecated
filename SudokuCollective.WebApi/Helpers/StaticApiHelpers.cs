using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SudokuCollective.Domain;
using SudokuCollective.WebApi.Models.DataModel;

namespace SudokuCollective.WebApi.Helpers {

    public static class StaticApiHelpers {

        public async static Task<SudokuMatrix> AttachSudokuMatrix(
            Game game, 
            ApplicationDbContext context) {
                    
                var sudokuMatrix = await context.SudokuMatrices
                    .Where(m => m.Id == game.SudokuMatrixId)
                    .Include(m => m.Difficulty)
                    .FirstOrDefaultAsync();

                sudokuMatrix.Difficulty.Matrices = null;
                
                sudokuMatrix.SudokuCells = 
                    await context.SudokuCells
                        .Where(cell => cell.SudokuMatrix.Id == sudokuMatrix.Id)
                        .OrderBy(cell => cell.Index)
                        .ToListAsync();

                return sudokuMatrix;
        }
    }
}

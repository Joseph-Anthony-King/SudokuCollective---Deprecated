using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SudokuCollective.Domain;
using SudokuCollective.Domain.Models;
using SudokuCollective.WebApi.Models.DataModel;

namespace SudokuCollective.WebApi.Helpers {

    public static class StaticApiHelpers {

        public async static Task AttachSudokuCells(
            this SudokuMatrix matrix,
            DatabaseContext context) {

            matrix.SudokuCells = await context.SudokuCells
                .Where(cell => cell.SudokuMatrixId == matrix.Id)
                .ToListAsync();
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SudokuCollective.Core.Interfaces.APIModels.PageModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Data.Models;

namespace SudokuCollective.Data.Helpers
{
    public static class StaticDataHelpers
    {
        public async static Task AttachSudokuCells(
            this ISudokuMatrix matrix,
            DatabaseContext context)
        {

            var cells = await context.SudokuCells
                .Where(cell => cell.SudokuMatrixId == matrix.Id)
                .ToListAsync();

            matrix.SudokuCells = cells;
        }

        public async static Task<bool> IsGameInActiveApp(this IGame game, DatabaseContext context)
        {
            var app = await context.Apps.FirstOrDefaultAsync(a => a.Id == game.AppId);

            return app.IsActive;
        }

        public static bool IsPageValid(IPaginator paginator, List<IEntityBase> entities)
        {
            if (paginator.ItemsPerPage * paginator.Page > entities.Count && paginator.Page == 1)
            {
                return true;
            }
            else if (paginator.ItemsPerPage * paginator.Page > entities.Count && paginator.Page > 1)
            {
                return false;
            }
            else
            {
                return paginator.ItemsPerPage * paginator.Page <= entities.Count;
            }
        }
    }
}

﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SudokuCollective.Core.Interfaces.APIModels.PageModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Data.Models;

namespace SudokuCollective.Data.Helpers
{
    public static class StaticApiHelpers
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
            var app = await context.Apps.FirstOrDefaultAsync(predicate: a => a.Id == game.AppId);

            return app.IsActive;
        }

        public static bool IsPageValid(IPageListModel pageListModel, List<IEntityBase> entities)
        {
            if (pageListModel.Page == 1)
            {
                return pageListModel.ItemsPerPage >= entities.Count;
            }
            else
            {
                return pageListModel.ItemsPerPage * pageListModel.Page <= entities.Count;
            }
        }
    }
}

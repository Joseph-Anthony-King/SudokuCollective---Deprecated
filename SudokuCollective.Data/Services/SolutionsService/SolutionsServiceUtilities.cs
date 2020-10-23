using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Data.Models;

namespace SudokuCollective.Data.Services
{
    internal static class SolutionsServiceUtilities
    {
        internal static async Task<List<ISudokuSolution>> RetrieveGames(
            IBaseRequest baseRequestRO,
            DatabaseContext context,
            int userId = 0)
        {
            var result = new List<ISudokuSolution>();
            var pageListModel = baseRequestRO.PageListModel;

            if (baseRequestRO.PageListModel == null)
            {
                if (userId == 0)
                {
                    result = (await context.SudokuSolutions
                        .Include(s => s.Game)
                        .ThenInclude(g => g.User)
                        .Where(s => s.DateSolved > DateTime.MinValue)
                        .ToListAsync()).ConvertAll(s => s as ISudokuSolution);
                }
                else
                {
                    result = (await context.SudokuSolutions
                        .Include(s => s.Game)
                        .ThenInclude(g => g.User)
                        .Where(s => s.DateSolved > DateTime.MinValue
                            && s.Game.User.Id == userId)
                        .ToListAsync()).ConvertAll(s => s as ISudokuSolution);
                }
            }
            else if (pageListModel.SortBy == SortValue.ID)
            {
                if (pageListModel.OrderByDescending)
                {
                    if (userId == 0)
                    {
                        result = (await context.SudokuSolutions
                            .OrderByDescending(s => s.Id)
                            .Include(s => s.Game)
                            .ThenInclude(g => g.User)
                            .Where(s => s.DateSolved > DateTime.MinValue)
                            .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                            .Take(pageListModel.ItemsPerPage)
                            .ToListAsync()).ConvertAll(s => s as ISudokuSolution);
                    }
                    else
                    {
                        result = (await context.SudokuSolutions
                            .OrderByDescending(s => s.Id)
                            .Include(s => s.Game)
                            .ThenInclude(g => g.User)
                            .Where(s => s.DateSolved > DateTime.MinValue
                                && s.Game.User.Id == userId)
                            .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                            .Take(pageListModel.ItemsPerPage)
                            .ToListAsync()).ConvertAll(s => s as ISudokuSolution);
                    }
                }
                else
                {
                    if (userId == 0)
                    {
                        result = (await context.SudokuSolutions
                            .OrderBy(s => s.Id)
                            .Include(s => s.Game)
                            .ThenInclude(g => g.User)
                            .Where(s => s.DateSolved > DateTime.MinValue)
                            .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                            .Take(pageListModel.ItemsPerPage)
                            .ToListAsync()).ConvertAll(s => s as ISudokuSolution);
                    }
                    else
                    {
                        result = (await context.SudokuSolutions
                            .OrderBy(s => s.Id)
                            .Include(s => s.Game)
                            .ThenInclude(g => g.User)
                            .Where(s => s.DateSolved > DateTime.MinValue
                                && s.Game.User.Id == userId)
                            .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                            .Take(pageListModel.ItemsPerPage)
                            .ToListAsync()).ConvertAll(s => s as ISudokuSolution);
                    }
                }
            }
            else
            {
                if (pageListModel.OrderByDescending)
                {
                    if (userId == 0)
                    {
                        result = (await context.SudokuSolutions
                            .OrderByDescending(s => s.DateSolved)
                            .Include(s => s.Game)
                            .ThenInclude(g => g.User)
                            .Where(s => s.DateSolved > DateTime.MinValue)
                            .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                            .Take(pageListModel.ItemsPerPage)
                            .ToListAsync()).ConvertAll(s => s as ISudokuSolution);
                    }
                    else
                    {
                        result = (await context.SudokuSolutions
                            .OrderByDescending(s => s.DateSolved)
                            .Include(s => s.Game)
                            .ThenInclude(g => g.User)
                            .Where(s => s.DateSolved > DateTime.MinValue
                                && s.Game.User.Id == userId)
                            .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                            .Take(pageListModel.ItemsPerPage)
                            .ToListAsync()).ConvertAll(s => s as ISudokuSolution);
                    }
                }
                else
                {
                    if (userId == 0)
                    {
                        result = (await context.SudokuSolutions
                            .OrderBy(s => s.DateSolved)
                            .Include(s => s.Game)
                            .ThenInclude(g => g.User)
                            .Where(s => s.DateSolved > DateTime.MinValue)
                            .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                            .Take(pageListModel.ItemsPerPage)
                            .ToListAsync()).ConvertAll(s => s as ISudokuSolution);
                    }
                    else
                    {
                        result = (await context.SudokuSolutions
                            .OrderBy(s => s.DateSolved)
                            .Include(s => s.Game)
                            .ThenInclude(g => g.User)
                            .Where(s => s.DateSolved > DateTime.MinValue
                                && s.Game.User.Id == userId)
                            .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                            .Take(pageListModel.ItemsPerPage)
                            .ToListAsync()).ConvertAll(s => s as ISudokuSolution);
                    }
                }
            }

            return result;
        }
    }
}

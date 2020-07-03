using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SudokuCollective.Domain.Models;
using SudokuCollective.WebApi.Models.DataModels;
using SudokuCollective.WebApi.Models.Enums;
using SudokuCollective.WebApi.Models.RequestModels;

namespace SudokuCollective.WebApi.Services {

    internal static class SolutionsServiceUtilities {

        internal static async Task<List<SudokuSolution>> RetrieveGames(
            BaseRequest baseRequestRO, 
            DatabaseContext context,
            int userId = 0) {

            var result = new List<SudokuSolution>();
            var pageListModel = baseRequestRO.PageListModel;

            if (baseRequestRO.PageListModel == null) {

                if (userId == 0) {

                    result = await context.SudokuSolutions
                        .Include(s => s.Game)
                        .ThenInclude(g => g.User)
                        .Where(s => s.DateSolved > DateTime.MinValue)
                        .ToListAsync();

                } else {

                    result = await context.SudokuSolutions
                        .Include(s => s.Game)
                        .ThenInclude(g => g.User)
                        .Where(s => s.DateSolved > DateTime.MinValue 
                            && s.Game.User.Id == userId)
                        .ToListAsync();
                }

            } else if (pageListModel.SortBy == SortValue.ID) {

                if (pageListModel.OrderByDescending) {

                    if (userId == 0) {

                        result = await context.SudokuSolutions
                            .OrderByDescending(s => s.Id)
                            .Include(s => s.Game)
                            .ThenInclude(g => g.User)
                            .Where(s => s.DateSolved > DateTime.MinValue)
                            .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                            .Take(pageListModel.ItemsPerPage)
                            .ToListAsync();

                    } else {

                        result = await context.SudokuSolutions
                            .OrderByDescending(s => s.Id)
                            .Include(s => s.Game)
                            .ThenInclude(g => g.User)
                            .Where(s => s.DateSolved > DateTime.MinValue 
                                && s.Game.User.Id == userId)
                            .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                            .Take(pageListModel.ItemsPerPage)
                            .ToListAsync();
                    }

                } else {

                    if (userId == 0) {

                        result = await context.SudokuSolutions
                            .OrderBy(s => s.Id)
                            .Include(s => s.Game)
                            .ThenInclude(g => g.User)
                            .Where(s => s.DateSolved > DateTime.MinValue)
                            .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                            .Take(pageListModel.ItemsPerPage)
                            .ToListAsync();

                    } else {

                        result = await context.SudokuSolutions
                            .OrderBy(s => s.Id)
                            .Include(s => s.Game)
                            .ThenInclude(g => g.User)
                            .Where(s => s.DateSolved > DateTime.MinValue 
                                && s.Game.User.Id == userId)
                            .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                            .Take(pageListModel.ItemsPerPage)
                            .ToListAsync();
                    }
                }

            } else {

                if (pageListModel.OrderByDescending) {

                    if (userId == 0) {

                        result = await context.SudokuSolutions
                            .OrderByDescending(s => s.DateSolved)
                            .Include(s => s.Game)
                            .ThenInclude(g => g.User)
                            .Where(s => s.DateSolved > DateTime.MinValue)
                            .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                            .Take(pageListModel.ItemsPerPage)
                            .ToListAsync();

                    } else {

                        result = await context.SudokuSolutions
                            .OrderByDescending(s => s.DateSolved)
                            .Include(s => s.Game)
                            .ThenInclude(g => g.User)
                            .Where(s => s.DateSolved > DateTime.MinValue 
                                && s.Game.User.Id == userId)
                            .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                            .Take(pageListModel.ItemsPerPage)
                            .ToListAsync();
                    }

                } else {

                    if (userId == 0) {

                        result = await context.SudokuSolutions
                            .OrderBy(s => s.DateSolved)
                            .Include(s => s.Game)
                            .ThenInclude(g => g.User)
                            .Where(s => s.DateSolved > DateTime.MinValue)
                            .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                            .Take(pageListModel.ItemsPerPage)
                            .ToListAsync();

                    } else {

                        result = await context.SudokuSolutions
                            .OrderBy(s => s.DateSolved)
                            .Include(s => s.Game)
                            .ThenInclude(g => g.User)
                            .Where(s => s.DateSolved > DateTime.MinValue 
                                && s.Game.User.Id == userId)
                            .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                            .Take(pageListModel.ItemsPerPage)
                            .ToListAsync();
                    }
                }
            }

            return result;
        }
        
    }
}

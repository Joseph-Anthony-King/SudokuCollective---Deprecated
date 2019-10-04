using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SudokuCollective.Models;
using SudokuCollective.WebApi.Models;
using SudokuCollective.WebApi.Models.DataModel;
using SudokuCollective.WebApi.Models.Enums;

namespace SudokuCollective.WebApi.Services {

    internal static class UsersServiceUtilities {

        internal static async Task<List<User>> RetrieveUsers(
            PageListModel pageListModel, ApplicationDbContext context) {

            var result = new List<User>();

            if (pageListModel == null) {

                result = await context.Users
                    .OrderBy(u => u.Id)
                    .Include(u => u.Games)
                    .Include(u => u.Roles)
                    .Include(u => u.Apps)
                    .ToListAsync();

            } else if (pageListModel.SortBy == SortValue.ID) {

                if (pageListModel.OrderByDescending) {

                    result = await context.Users
                        .OrderByDescending(u => u.Id)
                        .Include(u => u.Games)
                        .Include(u => u.Roles)
                        .Include(u => u.Apps)
                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                        .Take(pageListModel.ItemsPerPage)
                        .ToListAsync();

                } else {

                    result = await context.Users
                        .OrderBy(u => u.Id)
                        .Include(u => u.Games)
                        .Include(u => u.Roles)
                        .Include(u => u.Apps)
                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                        .Take(pageListModel.ItemsPerPage)
                        .ToListAsync();
                }

            } else if (pageListModel.SortBy == SortValue.USERNAME) {

                if (pageListModel.OrderByDescending) {

                    result = await context.Users
                        .OrderByDescending(u => u.UserName)
                        .Include(u => u.Games)
                        .Include(u => u.Roles)
                        .Include(u => u.Apps)
                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                        .Take(pageListModel.ItemsPerPage)
                        .ToListAsync();

                } else {

                    result = await context.Users
                        .OrderBy(u => u.UserName)
                        .Include(u => u.Games)
                        .Include(u => u.Roles)
                        .Include(u => u.Apps)
                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                        .Take(pageListModel.ItemsPerPage)
                        .ToListAsync();
                }

            } else if (pageListModel.SortBy == SortValue.FIRSTNAME) {

                if (pageListModel.OrderByDescending) {

                    result = await context.Users
                        .OrderByDescending(u => u.FirstName)
                        .Include(u => u.Games)
                        .Include(u => u.Roles)
                        .Include(u => u.Apps)
                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                        .Take(pageListModel.ItemsPerPage)
                        .ToListAsync();

                } else {

                    result = await context.Users
                        .OrderBy(u => u.FirstName)
                        .Include(u => u.Games)
                        .Include(u => u.Roles)
                        .Include(u => u.Apps)
                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                        .Take(pageListModel.ItemsPerPage)
                        .ToListAsync();
                }

            } else if (pageListModel.SortBy == SortValue.LASTNAME) {

                if (pageListModel.OrderByDescending) {

                    result = await context.Users
                        .OrderByDescending(u => u.LastName)
                        .Include(u => u.Games)
                        .Include(u => u.Roles)
                        .Include(u => u.Apps)
                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                        .Take(pageListModel.ItemsPerPage)
                        .ToListAsync();

                } else {

                    result = await context.Users
                        .OrderBy(u => u.LastName)
                        .Include(u => u.Games)
                        .Include(u => u.Roles)
                        .Include(u => u.Apps)
                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                        .Take(pageListModel.ItemsPerPage)
                        .ToListAsync();
                }

            } else if (pageListModel.SortBy == SortValue.FULLNAME) {

                if (pageListModel.OrderByDescending) {

                    result = await context.Users
                        .OrderByDescending(u => u.FullName)
                        .Include(u => u.Games)
                        .Include(u => u.Roles)
                        .Include(u => u.Apps)
                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                        .Take(pageListModel.ItemsPerPage)
                        .ToListAsync();

                } else {

                    result = await context.Users
                        .OrderBy(u => u.FullName)
                        .Include(u => u.Games)
                        .Include(u => u.Roles)
                        .Include(u => u.Apps)
                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                        .Take(pageListModel.ItemsPerPage)
                        .ToListAsync();
                }

            } else if (pageListModel.SortBy == SortValue.NICKNAME) {

                if (pageListModel.OrderByDescending) {

                    result = await context.Users
                        .OrderByDescending(u => u.NickName)
                        .Include(u => u.Games)
                        .Include(u => u.Roles)
                        .Include(u => u.Apps)
                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                        .Take(pageListModel.ItemsPerPage)
                        .ToListAsync();

                } else {

                    result = await context.Users
                        .OrderBy(u => u.NickName)
                        .Include(u => u.Games)
                        .Include(u => u.Roles)
                        .Include(u => u.Apps)
                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                        .Take(pageListModel.ItemsPerPage)
                        .ToListAsync();
                }

            } else if (pageListModel.SortBy == SortValue.GAMECOUNT) {

                if (pageListModel.OrderByDescending) {

                    result = await context.Users
                        .OrderByDescending(u => u.Games.Count)
                        .Include(u => u.Games)
                        .Include(u => u.Roles)
                        .Include(u => u.Apps)
                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                        .Take(pageListModel.ItemsPerPage)
                        .ToListAsync();

                } else {

                    result = await context.Users
                        .OrderBy(u => u.Games.Count)
                        .Include(u => u.Games)
                        .Include(u => u.Roles)
                        .Include(u => u.Apps)
                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                        .Take(pageListModel.ItemsPerPage)
                        .ToListAsync();
                }

            } else if (pageListModel.SortBy == SortValue.APPCOUNT) {

                if (pageListModel.OrderByDescending) {

                    result = await context.Users
                        .OrderByDescending(u => u.Apps.Count)
                        .Include(u => u.Games)
                        .Include(u => u.Roles)
                        .Include(u => u.Apps)
                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                        .Take(pageListModel.ItemsPerPage)
                        .ToListAsync();

                } else {

                    result = await context.Users
                        .OrderBy(u => u.Apps.Count)
                        .Include(u => u.Games)
                        .Include(u => u.Roles)
                        .Include(u => u.Apps)
                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                        .Take(pageListModel.ItemsPerPage)
                        .ToListAsync();
                }

            } else {

                if (pageListModel.OrderByDescending) {

                    result = await context.Users
                        .OrderByDescending(u => u.DateCreated)
                        .Include(u => u.Games)
                        .Include(u => u.Roles)
                        .Include(u => u.Apps)
                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                        .Take(pageListModel.ItemsPerPage)
                        .ToListAsync();

                } else {

                    result = await context.Users
                        .OrderBy(u => u.DateCreated)
                        .Include(u => u.Games)
                        .Include(u => u.Roles)
                        .Include(u => u.Apps)
                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                        .Take(pageListModel.ItemsPerPage)
                        .ToListAsync();
                }
            }

            return result;
        }

        internal static bool IsUserValid(User user) {

            var result = true;

            if (string.IsNullOrEmpty(user.UserName) &&
                string.IsNullOrEmpty(user.FirstName) &&
                string.IsNullOrEmpty(user.LastName) &&
                string.IsNullOrEmpty(user.NickName) &&
                string.IsNullOrEmpty(user.Email) &&
                string.IsNullOrEmpty(user.Password) &&
                user.Id == 0) {

                result = false;
            }

            return result;
        }
    }
}

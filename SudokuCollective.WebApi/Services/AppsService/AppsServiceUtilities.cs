using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SudokuCollective.Models;
using SudokuCollective.WebApi.Models;
using SudokuCollective.WebApi.Models.DataModel;
using SudokuCollective.WebApi.Models.Enums;
using SudokuCollective.WebApi.Models.RequestModels;
using SudokuCollective.WebApi.Models.TaskModels.UserRequests;

namespace SudokuCollective.WebApi.Services {

    internal static class AppsServiceUtilities {

        internal static async Task<List<App>> RetrieveApps(
            PageListModel pageListModel, ApplicationDbContext context) {

            var result = new List<App>();

            if (pageListModel == null) {

                result = await context.Apps
                    .OrderBy(a => a.Id)
                    .Include(a => a.Users)
                    .ToListAsync();

            } else if (pageListModel.SortBy == SortValue.ID) {

                if (pageListModel.OrderByDescending) {

                    result = await context.Apps
                        .OrderByDescending(u => u.Id)
                        .Include(a => a.Users)
                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                        .Take(pageListModel.ItemsPerPage)
                        .ToListAsync();

                } else {

                    result = await context.Apps
                        .OrderBy(u => u.Id)
                        .Include(a => a.Users)
                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                        .Take(pageListModel.ItemsPerPage)
                        .ToListAsync();
                }

            } else if (pageListModel.SortBy == SortValue.NAME) {

                if (pageListModel.OrderByDescending) {

                    result = await context.Apps
                        .OrderByDescending(u => u.Name)
                        .Include(a => a.Users)
                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                        .Take(pageListModel.ItemsPerPage)
                        .ToListAsync();

                } else {

                    result = await context.Apps
                        .OrderBy(u => u.Name)
                        .Include(a => a.Users)
                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                        .Take(pageListModel.ItemsPerPage)
                        .ToListAsync();
                }

            } else if (pageListModel.SortBy == SortValue.USERCOUNT) {

                if (pageListModel.OrderByDescending) {

                    result = await context.Apps
                        .OrderByDescending(u => u.Users.Count)
                        .Include(a => a.Users)
                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                        .Take(pageListModel.ItemsPerPage)
                        .ToListAsync();

                } else {

                    result = await context.Apps
                        .OrderBy(u => u.Users.Count)
                        .Include(a => a.Users)
                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                        .Take(pageListModel.ItemsPerPage)
                        .ToListAsync();
                }

            } else {

                if (pageListModel.OrderByDescending) {

                    result = await context.Apps
                        .OrderByDescending(u => u.DateCreated)
                        .Include(a => a.Users)
                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                        .Take(pageListModel.ItemsPerPage)
                        .ToListAsync();

                } else {

                    result = await context.Apps
                        .OrderBy(u => u.DateCreated)
                        .Include(a => a.Users)
                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                        .Take(pageListModel.ItemsPerPage)
                        .ToListAsync();
                }
            }

            return result;
        }

        internal static async Task<UserListTaskResult> RetrieveUsers(
            BaseRequestRO baseRequestRO, 
            bool fullRecord, 
            ApplicationDbContext context) {

            var userListTaskResult = new UserListTaskResult();

            try {

                var pageListModel = baseRequestRO.PageListModel;

                var app = await context.Apps
                    .Include(a => a.Users)
                    .FirstOrDefaultAsync(predicate: a => a.License.Equals(baseRequestRO.License));

                app.Users.OrderBy(u => u.UserId);

                if (fullRecord) {

                    foreach (var user in app.Users) {

                        user.User = await context.Users
                            .Include(u => u.Games)
                            .Include(u => u.Roles)
                            .FirstOrDefaultAsync(predicate: u => u.Id == user.UserId);

                        user.User.Roles = await context.UsersRoles
                            .Include(ur => ur.Role)
                            .Where(ur => ur.UserId == user.UserId)
                            .ToListAsync();

                        foreach (var userRole in user.User.Roles) {

                            userRole.Role.Users = null;
                        }
                    }

                } else {

                    foreach (var user in app.Users) {

                        user.User = await context.Users
                            .Include(u => u.Games)
                            .FirstOrDefaultAsync(predicate: u => u.Id == user.UserId);
                    }
                }

                var users = new List<User>();

                if (pageListModel == null) {

                    users = app.Users
                        .Select(ua => ua.User)
                        .OrderBy(u => u.Id)
                        .ToList();

                } else if (pageListModel.SortBy == SortValue.ID) {

                    if (pageListModel.OrderByDescending) {

                        users = app.Users
                            .Select(ua => ua.User)
                            .OrderByDescending(u => u.Id)
                            .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                            .Take(pageListModel.ItemsPerPage)
                            .ToList();

                    } else {

                        users = app.Users
                            .Select(ua => ua.User)
                            .OrderBy(u => u.Id)
                            .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                            .Take(pageListModel.ItemsPerPage)
                            .ToList();
                    }

                } else if (pageListModel.SortBy == SortValue.USERNAME) {

                    if (pageListModel.OrderByDescending) {

                        users = app.Users
                            .Select(ua => ua.User)
                            .OrderByDescending(u => u.UserName)
                            .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                            .Take(pageListModel.ItemsPerPage)
                            .ToList();

                    } else {

                        users = app.Users
                            .Select(ua => ua.User)
                            .OrderBy(u => u.UserName)
                            .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                            .Take(pageListModel.ItemsPerPage)
                            .ToList();
                    }

                } else if (pageListModel.SortBy == SortValue.FIRSTNAME) {

                    if (pageListModel.OrderByDescending) {

                        users = app.Users
                            .Select(ua => ua.User)
                            .OrderByDescending(u => u.FirstName)
                            .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                            .Take(pageListModel.ItemsPerPage)
                            .ToList();

                    } else {

                        users = app.Users
                            .Select(ua => ua.User)
                            .OrderBy(u => u.FirstName)
                            .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                            .Take(pageListModel.ItemsPerPage)
                            .ToList();
                    }

                } else if (pageListModel.SortBy == SortValue.LASTNAME) {

                    if (pageListModel.OrderByDescending) {

                        users = app.Users
                            .Select(ua => ua.User)
                            .OrderByDescending(u => u.LastName)
                            .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                            .Take(pageListModel.ItemsPerPage)
                            .ToList();

                    } else {

                        users = app.Users
                            .Select(ua => ua.User)
                            .OrderBy(u => u.LastName)
                            .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                            .Take(pageListModel.ItemsPerPage)
                            .ToList();
                    }

                } else if (pageListModel.SortBy == SortValue.FULLNAME) {

                    if (pageListModel.OrderByDescending) {

                        users = app.Users
                            .Select(ua => ua.User)
                            .OrderByDescending(u => u.FullName)
                            .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                            .Take(pageListModel.ItemsPerPage)
                            .ToList();

                    } else {

                        users = app.Users
                            .Select(ua => ua.User)
                            .OrderBy(u => u.LastName)
                            .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                            .Take(pageListModel.ItemsPerPage)
                            .ToList();
                    }

                } else if (pageListModel.SortBy == SortValue.NICKNAME) {

                    if (pageListModel.OrderByDescending) {

                        users = app.Users
                            .Select(ua => ua.User)
                            .OrderByDescending(u => u.NickName)
                            .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                            .Take(pageListModel.ItemsPerPage)
                            .ToList();

                    } else {

                        users = app.Users
                            .Select(ua => ua.User)
                            .OrderBy(u => u.LastName)
                            .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                            .Take(pageListModel.ItemsPerPage)
                            .ToList();
                    }

                } else if (pageListModel.SortBy == SortValue.GAMECOUNT) {

                    if (pageListModel.OrderByDescending) {

                        users = app.Users
                            .Select(ua => ua.User)
                            .OrderByDescending(u => u.Games.Count)
                            .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                            .Take(pageListModel.ItemsPerPage)
                            .ToList();

                    } else {

                        users = app.Users
                            .Select(ua => ua.User)
                            .OrderBy(u => u.Games.Count)
                            .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                            .Take(pageListModel.ItemsPerPage)
                            .ToList();
                    }

                } else {

                    if (pageListModel.OrderByDescending) {

                        users = app.Users
                            .Select(ua => ua.User)
                            .OrderByDescending(u => u.DateCreated)
                            .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                            .Take(pageListModel.ItemsPerPage)
                            .ToList();

                    } else {

                        users = app.Users
                            .Select(ua => ua.User)
                            .OrderBy(u => u.DateCreated)
                            .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                            .Take(pageListModel.ItemsPerPage)
                            .ToList();
                    }
                }

                foreach (var user in users) {

                    user.Apps = null;
                }

                userListTaskResult.Success = true;
                userListTaskResult.Users = users;

                return userListTaskResult;

            } catch (Exception e) {

                userListTaskResult.Message = e.Message;

                return userListTaskResult;
            }
        }
    }
}

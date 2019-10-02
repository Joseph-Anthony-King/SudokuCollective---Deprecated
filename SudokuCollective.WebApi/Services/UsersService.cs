using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SudokuCollective.Models;
using SudokuCollective.Models.Enums;
using SudokuCollective.WebApi.Helpers;
using SudokuCollective.WebApi.Models.DataModel;
using SudokuCollective.WebApi.Models.RequestModels.RegisterRequests;
using SudokuCollective.WebApi.Models.RequestModels.UserRequests;
using SudokuCollective.WebApi.Models.TaskModels.UserRequests;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.WebApi.Services {

    public class UsersService : IUsersService {

        private readonly ApplicationDbContext _context;
        private readonly IAppsService _appsService;

        public UsersService(ApplicationDbContext context, IAppsService appsService) {

            _context = context;
            _appsService = appsService;
        }

        public async Task<UserTaskResult> GetUser(int id, bool fullRecord = true) {

            var createdDate = DateTime.UtcNow;
            var user = new User() {

                Id = 0,
                UserName = String.Empty,
                FirstName = String.Empty,
                LastName = String.Empty,
                NickName = string.Empty,
                DateCreated = createdDate,
                DateUpdated = createdDate,
                Email = string.Empty,
                Password = string.Empty
            };

            var userTaskResult = new UserTaskResult() {

                Result = false,
                User = user
            };

            try {

                if (fullRecord) {

                    user = await _context.Users
                        .Include(u => u.Games)
                        .Include(u => u.Roles)
                        .Include(u => u.Apps)
                        .FirstOrDefaultAsync(
                            predicate: u => u.Id == id);

                    if (user == null) {

                        user = new User() {

                            Id = 0,
                            UserName = String.Empty,
                            FirstName = String.Empty,
                            LastName = String.Empty,
                            NickName = string.Empty,
                            DateCreated = createdDate,
                            DateUpdated = createdDate,
                            Email = string.Empty,
                            Password = string.Empty
                        };

                        userTaskResult.User = user;

                        return userTaskResult;
                    }

                    foreach (var game in user.Games) {

                        game.SudokuMatrix = await StaticApiHelpers
                            .AttachSudokuMatrix(game, _context);
                    }

                    foreach (var userRole in user.Roles) {

                        userRole.Role = await _context.Roles
                            .FirstOrDefaultAsync(
                                predicate: r => r.Id == userRole.RoleId);

                        userRole.Role.Users = null;
                    }

                    foreach (var userApp in user.Apps) {

                        userApp.App = await _context.Apps
                            .FirstOrDefaultAsync(
                                predicate: a => a.Id == userApp.AppId);

                        userApp.App.Users = null;
                    }

                    user.Games.OrderBy(g => g.Id);
                    user.Roles.OrderBy(r => r.RoleId);
                    user.Apps.OrderBy(a => a.AppId);

                    userTaskResult.Result = true;
                    userTaskResult.User = user;

                } else {

                    user = await _context.Users
                        .Include(u => u.Roles)
                        .Include(u => u.Apps)
                        .FirstOrDefaultAsync(
                            predicate: u => u.Id == id);

                    user.Games = null;
                    user.Roles.OrderBy(r => r.RoleId);
                    user.Apps.OrderBy(a => a.AppId);

                    userTaskResult.Result = true;
                    userTaskResult.User = user;
                }

                return userTaskResult;

            } catch (Exception) {

                return userTaskResult;
            }
        }

        public async Task<UserListTaskResult> GetUsers(bool fullRecord = true) {

            var users = new List<User>();

            var userListTaskResult = new UserListTaskResult() {

                Result = false,
                Users = users
            };

            try {

                if (fullRecord) {

                    users = await _context.Users
                        .OrderBy(u => u.Id)
                        .Include(u => u.Games)
                        .Include(u => u.Roles)
                        .Include(u => u.Apps)
                        .ToListAsync();

                    foreach (var user in users) {

                        foreach (var game in user.Games) {

                            game.SudokuMatrix = await _context.SudokuMatrices
                                .Include(m => m.Difficulty)
                                .FirstOrDefaultAsync(
                                    predicate: m => m.Id == game.SudokuMatrixId);

                            game.SudokuMatrix.Difficulty.Matrices = null;
                        }

                        foreach (var userRole in user.Roles) {

                            userRole.Role = await _context.Roles
                                .FirstOrDefaultAsync(
                                    predicate: r => r.Id == userRole.RoleId);

                            userRole.Role.Users = null;
                        }

                        foreach (var userApp in user.Apps) {

                            userApp.App = await _context.Apps
                                .FirstOrDefaultAsync(
                                    predicate: a => a.Id == userApp.AppId);

                            userApp.App.Users = null;
                        }

                        user.Games.OrderBy(g => g.Id);
                        user.Roles.OrderBy(r => r.RoleId);
                        user.Apps.OrderBy(a => a.AppId);
                    }

                    userListTaskResult.Result = true;
                    userListTaskResult.Users = users;

                } else {

                    users = await _context.Users
                        .OrderBy(u => u.Id)
                        .Include(u => u.Roles)
                        .Include(u => u.Apps)
                        .ToListAsync();

                    foreach (var user in users) {

                        foreach (var userRole in user.Roles) {

                            userRole.Role = await _context.Roles
                                .FirstOrDefaultAsync(
                                    predicate: r => r.Id == userRole.RoleId);

                            userRole.Role.Users = null;
                        }

                        user.Games = null;
                        user.Roles.OrderBy(r => r.RoleId);
                        user.Apps.OrderBy(a => a.AppId);
                    }

                    userListTaskResult.Result = true;
                    userListTaskResult.Users = users;
                }

                return userListTaskResult;

            } catch (Exception) {

                return userListTaskResult;
            }
        }

        public async Task<UserTaskResult> CreateUser(RegisterRO registerRO) {

            var createdDate = DateTime.UtcNow;
            var user = new User() {

                Id = 0,
                UserName = String.Empty,
                FirstName = String.Empty,
                LastName = String.Empty,
                NickName = string.Empty,
                DateCreated = createdDate,
                DateUpdated = createdDate,
                Email = string.Empty,
                Password = string.Empty
            };

            var app = new App() {

                Id = 0,
                Name = string.Empty,
                License = string.Empty,
                OwnerId = 0,
                DateCreated = createdDate,
                DevUrl = string.Empty,
                LiveUrl = string.Empty
            };

            var userTaskResult = new UserTaskResult() {

                Result = false,
                User = user
            };

            try {
                
                var regex = new Regex("^[a-zA-Z0-9-._]*$");

                if (regex.IsMatch(registerRO.UserName)) {

                    if (_context.Apps.Any(a => a.License.Equals(registerRO.License))) {

                        app = await _context.Apps
                            .Include(a => a.Users)
                            .FirstOrDefaultAsync(
                                predicate: a => a.License.Equals(registerRO.License));
                    }

                    var salt = BCrypt.Net.BCrypt.GenerateSalt();

                    user = new User() {

                        UserName = registerRO.UserName,
                        FirstName = registerRO.FirstName,
                        LastName = registerRO.LastName,
                        NickName = registerRO.NickName,
                        DateCreated = createdDate,
                        DateUpdated = createdDate,
                        Email = registerRO.Email,
                        Password = BCrypt.Net.BCrypt
                            .HashPassword(registerRO.Password, salt)
                    };

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    if (user.Id != 0) {

                        var defaultRole = await _context.Roles
                            .FirstOrDefaultAsync(
                                predicate: p => p.Id == 4);

                        UserRole newUserRole = new UserRole {

                            UserId = user.Id,
                            User = user,
                            RoleId = defaultRole.Id,
                            Role = defaultRole
                        };

                        _context.UsersRoles.Add(newUserRole);
                        await _context.SaveChangesAsync();

                        UserApp newUserApp = new UserApp {

                            UserId = user.Id,
                            User = user,
                            AppId = app.Id,
                            App = app
                        };

                        _context.UsersApps.Add(newUserApp);
                        await _context.SaveChangesAsync();

                        userTaskResult.Result = true;
                        userTaskResult.User = user;
                    }

                } else {

                    userTaskResult.User.UserName = "Remove-Special-Characters";
                }

                return userTaskResult;

            } catch (Exception) {

                return userTaskResult;
            }
        }

        public async Task<UserTaskResult> UpdateUser(
            int id, UpdateUserRO updateUserRO) {

            var createdDate = DateTime.UtcNow;
            var userTaskResult = new UserTaskResult() {

                Result = false,
                User = new User() {

                    Id = 0,
                    UserName = string.Empty,
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    NickName = string.Empty,
                    DateCreated = createdDate,
                    DateUpdated = createdDate,
                    Email = string.Empty,
                    Password = string.Empty
                }
            };

            try {

                var emailIsUnique = true;
                var userIdExists = false;
                var user = new User();
                var users = await _context.Users.ToListAsync();
                var updatedDate = DateTime.UtcNow;

                foreach (var u in users) {

                    if (u.Email.Equals(updateUserRO.Email) && u.Id != id) {

                        emailIsUnique = false;
                    }

                    if (u.Id == id) {

                        userIdExists = true;
                    }
                }

                if (emailIsUnique && userIdExists) {

                    user = await _context.Users.FindAsync(id);

                    user.UserName = updateUserRO.UserName;
                    user.FirstName = updateUserRO.FirstName;
                    user.LastName = updateUserRO.LastName;
                    user.NickName = updateUserRO.NickName;
                    user.Email = updateUserRO.Email;
                    user.DateUpdated = updatedDate;

                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();

                    userTaskResult.Result = true;
                    userTaskResult.User = user;

                    return userTaskResult;

                } else {

                    var outGoingEmail = string.Empty;

                    if (!emailIsUnique) {

                        outGoingEmail = updateUserRO.Email;
                    }

                    user = new User() {

                        Id = 0,
                        UserName = String.Empty,
                        FirstName = String.Empty,
                        LastName = String.Empty,
                        NickName = string.Empty,
                        DateCreated = updatedDate,
                        DateUpdated = updatedDate,
                        Email = outGoingEmail,
                        Password = string.Empty
                    };

                    userTaskResult.Result = false;
                    userTaskResult.User = user;
                }

                return userTaskResult;

            } catch (Exception) {

                return userTaskResult;
            }
        }

        public async Task<bool> UpdatePassword(int id, UpdatePasswordRO updatePasswordRO) {

            var result = false;

            try {

                var user = await _context.Users
                    .SingleOrDefaultAsync(
                        predicate: u => u.Id == id);

                if (user != null &&
                    BCrypt.Net.BCrypt.Verify(updatePasswordRO.OldPassword, user.Password)) {

                    var salt = BCrypt.Net.BCrypt.GenerateSalt();

                    user.Password = BCrypt.Net.BCrypt
                        .HashPassword(updatePasswordRO.NewPassword, salt);

                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();

                    user.DateUpdated = DateTime.UtcNow;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();

                    result = true;
                }

                return result;

            } catch (Exception) {

                return result;
            }
        }

        public async Task<bool> DeleteUser(int id) {

            var result = false;

            try {

                var user = await _context.Users
                    .Include(u => u.Games)
                    .ThenInclude(g => g.SudokuMatrix)
                    .FirstOrDefaultAsync(predicate: u => u.Id == id);

                if (user == null) {

                    return result;
                }

                if (user.Games.Count > 0) {
                
                    foreach (var game in user.Games) {

                        game.SudokuMatrix = await StaticApiHelpers
                            .AttachSudokuMatrix(game, _context);

                        if (game.ContinueGame) {

                            var solution = await _context.SudokuSolutions
                                .FirstOrDefaultAsync(
                                    predicate: s => s.Id == game.SudokuSolutionId);

                            _context.SudokuSolutions.Remove(solution);
                        }
                    }
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return result = true;

            } catch (Exception) {

                return result;
            }
        }

        public async Task<bool> AddUserRoles(int id, List<int> roleIds) {

            var result = false;

            try {

                var user = await _context.Users
                    .Include(u => u.Roles)
                    .FirstOrDefaultAsync(predicate: u => u.Id == id);

                foreach (var userRole in user.Roles) {

                    userRole.Role = await _context.Roles
                        .FirstOrDefaultAsync(predicate: r => r.Id == userRole.RoleId);
                }

                var roles = new List<Role>();
                var userRoles = new List<UserRole>();

                foreach (var roleId in roleIds) {

                    var role = await _context.Roles
                        .FirstOrDefaultAsync(predicate: r => r.Id == roleId);

                    /* There is only superuser system wide.  The following filter
                       prevents any other users from elevating their roles to
                       superuser. */
                    if (role.RoleLevel != RoleLevel.SUPERUSER) {

                        roles.Add(role);
                    }
                }

                foreach (var role in roles) {

                    if (!user.Roles.Any(ur => ur.RoleId == role.Id)) {

                        userRoles.Add(new UserRole() {
                            UserId = user.Id,
                            User = user,
                            RoleId = role.Id,
                            Role = role
                        });
                    }
                }

                if (userRoles.Count > 0) {

                    _context.UsersRoles.AddRange(userRoles);
                    await _context.SaveChangesAsync();

                    user.DateUpdated = DateTime.UtcNow;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                }

                return result = true;

            } catch (Exception) {

                return result;
            }
        }

        public async Task<bool> RemoveUserRoles(int id, List<int> roleIds) {

            var result = false;

            try {

                var user = await _context.Users
                    .Include(u => u.Roles)
                    .FirstOrDefaultAsync(predicate: u => u.Id == id);

                var usersRoles = await _context.UsersRoles
                    .Where(ur => ur.UserId == id && roleIds.Contains(ur.RoleId))
                    .ToListAsync();

                _context.UsersRoles.RemoveRange(usersRoles);
                await _context.SaveChangesAsync();

                user.DateUpdated = DateTime.UtcNow;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return result = true;

            } catch (Exception) {

                return result;
            }
        }

        public async Task<bool> ActivateUser(int id) {

            var result = false;

            try {

                var user = await _context.Users
                    .Include(u => u.Games)
                    .ThenInclude(g => g.SudokuMatrix)
                    .FirstOrDefaultAsync(predicate: u => u.Id == id);

                if (user == null) {

                    return result;
                }

                if (!user.IsActive) {
                    
                    user.ActivateUser();
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();

                    user.DateUpdated = DateTime.UtcNow;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                }

                return result = true;

            } catch (Exception) {

                return result;
            }
        }

        public async Task<bool> DeactivateUser(int id) {

            var result = false;

            try {

                var user = await _context.Users
                    .Include(u => u.Games)
                    .ThenInclude(g => g.SudokuMatrix)
                    .FirstOrDefaultAsync(predicate: u => u.Id == id);

                if (user == null) {

                    return result;
                }

                if (user.IsActive) {
                    
                    user.DeactiveUser();
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();

                    user.DateUpdated = DateTime.UtcNow;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                }

                return result = true;

            } catch (Exception) {

                return result;
            }
        }

        public bool IsUserValid(User user) {

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

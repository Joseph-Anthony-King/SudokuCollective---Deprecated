using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.APIModels.PageModels;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Data.Helpers;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.ResultModels;
using SudokuCollective.Domain.Models;

namespace SudokuCollective.Data.Services
{
    public class UsersService : IUsersService
    {
        private readonly DatabaseContext _context;

        public UsersService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IUserResult> GetUser(int id, bool fullRecord = false)
        {
            var createdDate = DateTime.UtcNow;
            var user = new User();

            var userResult = new UserResult();

            try
            {
                if (fullRecord)
                {
                    user = (User)await _context.Users
                        .Include(u => u.Games)
                        .Include(u => u.Roles)
                        .Include(u => u.Apps)
                        .FirstOrDefaultAsync(
                            predicate: u => u.Id == id);

                    if (user == null)
                    {
                        userResult.Message = "User not found";

                        return userResult;
                    }

                    foreach (var game in user.Games)
                    {
                        await game.SudokuMatrix.AttachSudokuCells(_context);
                    }

                    foreach (var userRole in user.Roles)
                    {
                        userRole.Role = await _context.Roles
                            .FirstOrDefaultAsync(
                                predicate: r => r.Id == userRole.RoleId);

                        userRole.Role.Users = null;
                    }

                    foreach (var userApp in user.Apps)
                    {
                        userApp.App = await _context.Apps
                            .FirstOrDefaultAsync(
                                predicate: a => a.Id == userApp.AppId);

                        userApp.App.Users = null;
                    }

                    user.Games.OrderBy(g => g.Id);
                    user.Roles.OrderBy(r => r.RoleId);
                    user.Apps.OrderBy(a => a.AppId);

                    userResult.Success = true;
                    userResult.User = user;
                }
                else
                {
                    user = (User)await _context.Users
                        .Include(u => u.Roles)
                        .Include(u => u.Apps)
                        .FirstOrDefaultAsync(
                            predicate: u => u.Id == id);

                    if (user == null)
                    {
                        userResult.Message = "User not found";

                        return userResult;
                    }

                    foreach (var userRole in user.Roles)
                    {
                        userRole.Role = await _context.Roles
                            .FirstOrDefaultAsync(
                                predicate: r => r.Id == userRole.RoleId);

                        userRole.Role.Users = null;
                    }

                    user.Roles.OrderBy(r => r.RoleId);
                    user.Apps.OrderBy(a => a.AppId);

                    userResult.Success = true;
                    userResult.User = user;
                }

                return userResult;
            }
            catch (Exception e)
            {
                userResult.Message = e.Message;
                return userResult;
            }
        }

        public async Task<IUsersResult> GetUsers(
            IPageListModel pageListModel,
            bool fullRecord = false)
        {
            var users = new List<IUser>();

            var userListTaskResult = new UsersResult();

            try
            {
                users = await UsersServiceUtilities.RetrieveUsers(pageListModel, _context);

                if (fullRecord)
                {
                    foreach (var user in users)
                    {
                        foreach (var game in user.Games)
                        {
                            game.SudokuMatrix = await _context.SudokuMatrices
                                .Include(m => m.Difficulty)
                                .FirstOrDefaultAsync(
                                    predicate: m => m.Id == game.SudokuMatrixId);

                            game.SudokuMatrix.Difficulty.Matrices = null;
                        }

                        foreach (var userRole in user.Roles)
                        {
                            userRole.Role = await _context.Roles
                                .FirstOrDefaultAsync(
                                    predicate: r => r.Id == userRole.RoleId);

                            userRole.Role.Users = null;
                        }

                        foreach (var userApp in user.Apps)
                        {
                            userApp.App = await _context.Apps
                                .FirstOrDefaultAsync(
                                    predicate: a => a.Id == userApp.AppId);

                            userApp.App.Users = null;
                        }

                        user.Games.OrderBy(g => g.Id);
                        user.Roles.OrderBy(r => r.RoleId);
                        user.Apps.OrderBy(a => a.AppId);
                    }

                    userListTaskResult.Success = true;
                    userListTaskResult.Users = users;

                }
                else
                {
                    users = await UsersServiceUtilities.RetrieveUsers(pageListModel, _context);

                    foreach (var user in users)
                    {
                        foreach (var userRole in user.Roles)
                        {
                            userRole.Role = await _context.Roles
                                .FirstOrDefaultAsync(
                                    predicate: r => r.Id == userRole.RoleId);

                            userRole.Role.Users = null;
                        }

                        user.Games = null;
                        user.Roles.OrderBy(r => r.RoleId);
                        user.Apps.OrderBy(a => a.AppId);
                    }

                    userListTaskResult.Success = true;
                    userListTaskResult.Users = users;
                }

                return userListTaskResult;

            }
            catch (Exception e)
            {
                userListTaskResult.Message = e.Message;

                return userListTaskResult;
            }
        }

        public async Task<IUserResult> CreateUser(
            IRegisterRequest registerRequest,
            bool addAdmin = false)
        {
            var createdDate = DateTime.UtcNow;
            var user = new User();
            var app = new App();

            var userResult = new UserResult();

            if (await _context.Users.AnyAsync(u => u.UserName == registerRequest.UserName) ||
                await _context.Users.AnyAsync(u => u.Email == registerRequest.Email) ||
                string.IsNullOrEmpty(registerRequest.UserName) ||
                string.IsNullOrEmpty(registerRequest.Email))
            {
                if (await _context.Users.AnyAsync(u => u.UserName == registerRequest.UserName))
                {
                    userResult.Message = "Username is not unique";
                }
                else if (await _context.Users.AnyAsync(u => u.Email == registerRequest.Email))
                {
                    userResult.Message = "Email is not unique";
                }
                else if (string.IsNullOrEmpty(registerRequest.UserName))
                {
                    userResult.Message = "Username is required";
                }
                else
                {
                    userResult.Message = "Email is required";
                }

                return userResult;
            }
            else
            {
                try
                {
                    var regex = new Regex("^[a-zA-Z0-9-._]*$");

                    if (regex.IsMatch(registerRequest.UserName))
                    {
                        if (_context.Apps.Any(a => a.License.Equals(registerRequest.License)))
                        {
                            app = (App)await _context.Apps
                                .Include(a => a.Users)
                                .FirstOrDefaultAsync(
                                    predicate: a => a.License.Equals(registerRequest.License));
                        }

                        var salt = BCrypt.Net.BCrypt.GenerateSalt();

                        user = new User(
                            0,
                            registerRequest.UserName,
                            registerRequest.FirstName,
                            registerRequest.LastName,
                            registerRequest.NickName,
                            registerRequest.Email,
                            BCrypt.Net.BCrypt.HashPassword(registerRequest.Password, salt),
                            true,
                            createdDate,
                            DateTime.MinValue);

                        _context.Users.Add(user);
                        await _context.SaveChangesAsync();

                        if (user.Id != 0)
                        {
                            var defaultRole = await _context.Roles
                                .FirstOrDefaultAsync(
                                    predicate: p => p.Id == 4);

                            UserRole newUserRole = new UserRole
                            {
                                UserId = user.Id,
                                User = user,
                                RoleId = defaultRole.Id,
                                Role = defaultRole
                            };

                            _context.UsersRoles.Add(newUserRole);
                            await _context.SaveChangesAsync();

                            if (addAdmin)
                            {
                                var adminRole = await _context.Roles
                                    .FirstOrDefaultAsync(
                                        predicate: p => p.Id == 3);

                                UserRole newAdminRole = new UserRole
                                {
                                    UserId = user.Id,
                                    User = user,
                                    RoleId = adminRole.Id,
                                    Role = adminRole
                                };

                                _context.UsersRoles.Add(newAdminRole);
                                await _context.SaveChangesAsync();
                            }

                            UserApp newUserApp = new UserApp
                            {
                                UserId = user.Id,
                                User = user,
                                AppId = app.Id,
                                App = app
                            };

                            _context.UsersApps.Add(newUserApp);
                            await _context.SaveChangesAsync();

                            userResult.Success = true;
                            userResult.User = user;
                        }
                    }
                    else
                    {
                        userResult.Message = "User name can contain alphanumeric " +
                            "charaters or the following (._-), user name contained invalid characters.";
                    }

                    return userResult;

                }
                catch (Exception e)
                {
                    userResult.Message = e.Message;

                    return userResult;
                }
            }
        }

        public async Task<IUserResult> UpdateUser(
            int id, IUpdateUserRequest updateUserRequest)
        {
            var userResult = new UserResult();

            if (await _context.Users.AnyAsync(u => u.UserName == updateUserRequest.UserName && u.Id != id) ||
                await _context.Users.AnyAsync(u => u.Email == updateUserRequest.Email && u.Id != id) ||
                string.IsNullOrEmpty(updateUserRequest.UserName) ||
                string.IsNullOrEmpty(updateUserRequest.Email))
            {
                if (await _context.Users.AnyAsync(u => u.UserName == updateUserRequest.UserName && u.Id != id))
                {
                    userResult.Message = "Username is not unique";
                }
                else if (await _context.Users.AnyAsync(u => u.Email == updateUserRequest.Email && u.Id != id))
                {
                    userResult.Message = "Email is not unique";
                }
                else if (string.IsNullOrEmpty(updateUserRequest.UserName))
                {
                    userResult.Message = "Username is required";
                }
                else
                {
                    userResult.Message = "Email is required";
                }

                return userResult;

            }
            else
            {
                try
                {
                    var regex = new Regex("^[a-zA-Z0-9-._]*$");

                    if (regex.IsMatch(updateUserRequest.UserName))
                    {
                        var emailIsUnique = true;
                        var userIdExists = false;
                        var user = new User();
                        var users = await _context.Users.ToListAsync();
                        var updatedDate = DateTime.UtcNow;

                        foreach (var u in users)
                        {
                            if (u.Email.Equals(updateUserRequest.Email) && u.Id != id)
                            {
                                emailIsUnique = false;
                            }

                            if (u.Id == id)
                            {
                                userIdExists = true;
                            }
                        }

                        if (emailIsUnique && userIdExists)
                        {
                            user = (User)await _context.Users.FindAsync(id);

                            user.UserName = updateUserRequest.UserName;
                            user.FirstName = updateUserRequest.FirstName;
                            user.LastName = updateUserRequest.LastName;
                            user.NickName = updateUserRequest.NickName;
                            user.Email = updateUserRequest.Email;
                            user.DateUpdated = updatedDate;

                            _context.Users.Update(user);
                            await _context.SaveChangesAsync();

                            userResult.Success = true;
                            userResult.User = user;

                            return userResult;

                        }
                        else
                        {
                            if (!emailIsUnique && userIdExists)
                            {
                                userResult.Message = "User email is not unique";
                            }

                            if (!userIdExists)
                            {
                                userResult.Message = "User not found";
                            }
                        }
                    }
                    else
                    {
                        userResult.Message = "User name can contain alphanumeric " +
                            "charaters or the following (._-), user name contained invalid characters.";
                    }

                    return userResult;

                }
                catch (Exception e)
                {
                    userResult.Message = e.Message;

                    return userResult;
                }
            }
        }

        public async Task<IBaseResult> UpdatePassword(int id, IUpdatePasswordRequest updatePasswordRO)
        {
            var baseTaskResult = new BaseResult();
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            var oldPassword = BCrypt.Net.BCrypt.HashPassword(updatePasswordRO.OldPassword, salt);

            try
            {
                var user = await _context.Users
                    .SingleOrDefaultAsync(
                        predicate: u => u.Id == id);

                if (user != null)
                {
                    if (!BCrypt.Net.BCrypt.Verify(updatePasswordRO.OldPassword, user.Password))
                    {
                        user = null;

                        baseTaskResult.Message = "Old password was incorrect";

                        return baseTaskResult;
                    }
                }
                else
                {
                    baseTaskResult.Message = "User not found";

                    return baseTaskResult;
                }

                if (user != null &&
                    BCrypt.Net.BCrypt.Verify(updatePasswordRO.OldPassword, user.Password))
                {
                    user.Password = BCrypt.Net.BCrypt
                        .HashPassword(updatePasswordRO.NewPassword, salt);

                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();

                    user.DateUpdated = DateTime.UtcNow;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();

                    baseTaskResult.Success = true;
                }

                return baseTaskResult;
            }
            catch (Exception e)
            {
                baseTaskResult.Message = e.Message;

                return baseTaskResult;
            }
        }

        public async Task<IBaseResult> DeleteUser(int id)
        {
            var baseTaskResult = new BaseResult();

            try
            {
                var user = await _context.Users
                    .Include(u => u.Games)
                    .ThenInclude(g => g.SudokuMatrix)
                    .FirstOrDefaultAsync(predicate: u => u.Id == id);

                if (user == null)
                {
                    baseTaskResult.Message = "User not found";

                    return baseTaskResult;
                }

                if (user.Games.Count > 0)
                {
                    foreach (var game in user.Games)
                    {
                        await game.SudokuMatrix.AttachSudokuCells(_context);

                        if (game.ContinueGame)
                        {
                            var solution = await _context.SudokuSolutions
                                .FirstOrDefaultAsync(
                                    predicate: s => s.Id == game.SudokuSolutionId);

                            _context.SudokuSolutions.Remove(solution);
                        }
                    }
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                baseTaskResult.Success = true;

                return baseTaskResult;
            }
            catch (Exception e)
            {
                baseTaskResult.Message = e.Message;

                return baseTaskResult;
            }
        }

        public async Task<IBaseResult> AddUserRoles(int id, List<int> roleIds)
        {
            var baseTaskResult = new BaseResult();

            try
            {
                var user = await _context.Users
                    .Include(u => u.Roles)
                    .FirstOrDefaultAsync(predicate: u => u.Id == id);

                if (user == null)
                {
                    baseTaskResult.Message = "User not found";

                    return baseTaskResult;
                }

                foreach (var userRole in user.Roles)
                {
                    userRole.Role = await _context.Roles
                        .FirstOrDefaultAsync(predicate: r => r.Id == userRole.RoleId);
                }

                var roles = new List<Role>();
                var userRoles = new List<UserRole>();

                foreach (var roleId in roleIds)
                {
                    var role = await _context.Roles
                        .FirstOrDefaultAsync(predicate: r => r.Id == roleId);

                    /* There is only superuser system wide.  The following filter
                       prevents any other users from elevating their roles to
                       superuser. */
                    if (role.RoleLevel != RoleLevel.SUPERUSER)
                    {
                        roles.Add((Role)role);
                    }
                }

                foreach (var role in roles)
                {
                    if (!user.Roles.Any(ur => ur.RoleId == role.Id))
                    {
                        userRoles.Add(new UserRole()
                        {
                            UserId = user.Id,
                            User = user,
                            RoleId = role.Id,
                            Role = role
                        });
                    }
                }

                if (userRoles.Count > 0)
                {
                    _context.UsersRoles.AddRange(userRoles);
                    await _context.SaveChangesAsync();

                    user.DateUpdated = DateTime.UtcNow;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                }

                baseTaskResult.Success = true;

                return baseTaskResult;
            }
            catch (Exception e)
            {
                baseTaskResult.Message = e.Message;

                return baseTaskResult;
            }
        }

        public async Task<IBaseResult> RemoveUserRoles(int id, List<int> roleIds)
        {
            var baseTaskResult = new BaseResult();

            try
            {
                var user = await _context.Users
                    .Include(u => u.Roles)
                    .FirstOrDefaultAsync(predicate: u => u.Id == id);

                if (user == null)
                {
                    baseTaskResult.Message = "User not found";

                    return baseTaskResult;
                }

                var usersRoles = await _context.UsersRoles
                    .Where(ur => ur.UserId == id && roleIds.Contains(ur.RoleId))
                    .ToListAsync();

                _context.UsersRoles.RemoveRange(usersRoles);
                await _context.SaveChangesAsync();

                user.DateUpdated = DateTime.UtcNow;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                baseTaskResult.Success = true;

                return baseTaskResult;

            }
            catch (Exception e)
            {
                baseTaskResult.Message = e.Message;

                return baseTaskResult;
            }
        }

        public async Task<IBaseResult> ActivateUser(int id)
        {
            var baseTaskResult = new BaseResult();

            try
            {
                var user = await _context.Users
                    .Include(u => u.Games)
                    .ThenInclude(g => g.SudokuMatrix)
                    .FirstOrDefaultAsync(predicate: u => u.Id == id);

                if (user == null)
                {
                    baseTaskResult.Message = "User not found";

                    return baseTaskResult;
                }

                if (!user.IsActive)
                {
                    user.ActivateUser();
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();

                    user.DateUpdated = DateTime.UtcNow;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                }

                baseTaskResult.Success = true;

                return baseTaskResult;
            }
            catch (Exception e)
            {
                baseTaskResult.Message = e.Message;

                return baseTaskResult;
            }
        }

        public async Task<IBaseResult> DeactivateUser(int id)
        {
            var baseTaskResult = new BaseResult();

            try
            {
                var user = await _context.Users
                    .Include(u => u.Games)
                    .ThenInclude(g => g.SudokuMatrix)
                    .FirstOrDefaultAsync(predicate: u => u.Id == id);

                if (user == null)
                {
                    baseTaskResult.Message = "User not found";

                    return baseTaskResult;
                }

                if (user.IsActive)
                {
                    user.DeactiveUser();
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();

                    user.DateUpdated = DateTime.UtcNow;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                }

                baseTaskResult.Success = true;

                return baseTaskResult;
            }
            catch (Exception e)
            {
                baseTaskResult.Message = e.Message;

                return baseTaskResult;
            }
        }
    }
}

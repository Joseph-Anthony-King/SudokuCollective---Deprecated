using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.DataModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Interfaces.Repositories;
using SudokuCollective.Core.Models;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.DataModels;
using SudokuCollective.Data.Helpers;

namespace SudokuCollective.Data.Repositories
{
    public class AppsRepository<TEntity> : IAppsRepository<TEntity> where TEntity : App
    {
        #region Fields
        private readonly DatabaseContext context;
        #endregion

        #region Constructor
        public AppsRepository(DatabaseContext databaseContext)
        {
            context = databaseContext;
        }
        #endregion

        #region Methods
        async public Task<IRepositoryResponse> Create(TEntity entity)
        {
            var result = new RepositoryResponse();

            try
            {
                if (entity.Id != 0)
                {
                    result.Success = false;

                    return result;
                }

                context.Apps.Add(entity);

                foreach (var entry in context.ChangeTracker.Entries())
                {
                    if (entry.Entity is App app)
                    {
                        if (entity.Id == app.Id)
                        {
                            entry.State = EntityState.Added;
                        }
                        else
                        {
                            entry.State = EntityState.Modified;
                        }
                    }
                    else
                    {
                        var dbEntry = (IEntityBase)entry.Entity;

                        if (dbEntry.Id != 0)
                        {
                            entry.State = EntityState.Modified;
                        }
                        else
                        {
                            entry.State = EntityState.Added;
                        }
                    }
                }

                await context.SaveChangesAsync();

                // Add connection between the app and the user
                var userApp = new UserApp(
                    ((IApp)entity).OwnerId,
                    entity.Id);

                context.UsersApps.Add(userApp);

                // Ensure that the owner has admin priviledges, if not they will be promoted
                var addAdminRole = true;
                var newUserAdminRole = new UserRole();

                var user = await context
                    .Users
                    .Include(u => u.Roles)
                    .FirstOrDefaultAsync(predicate: u => u.Id == ((IApp)entity).OwnerId);

                foreach (var userRole in user.Roles)
                {
                    userRole.Role = await context
                        .Roles
                        .FirstOrDefaultAsync(roleDbSet => roleDbSet.Id == userRole.RoleId);

                    if (userRole.Role.RoleLevel == RoleLevel.ADMIN)
                    {
                        addAdminRole = false;
                    }
                }

                // Promote user to admin if user is not already
                if (addAdminRole)
                {
                    var adminRole = await context
                        .Roles
                        .FirstOrDefaultAsync(predicate: r => r.RoleLevel == RoleLevel.ADMIN);

                    newUserAdminRole = new UserRole(
                        user.Id,
                        adminRole.Id);

                    context.UsersRoles.Add(newUserAdminRole);
                }

                foreach (var entry in context.ChangeTracker.Entries())
                {
                    if (entry.Entity is UserApp ua)
                    {
                        if (userApp.Id == ua.Id)
                        {
                            entry.State = EntityState.Added;
                        }
                        else
                        {
                            entry.State = EntityState.Modified;
                        }
                    }
                    else if (entry.Entity is UserRole ur)
                    {
                        if (newUserAdminRole.Id == ur.Id)
                        {
                            entry.State = EntityState.Added;
                        }
                        else
                        {
                            entry.State = EntityState.Modified;
                        }
                    }
                    else
                    {
                        var dbEntry = (IEntityBase)entry.Entity;

                        if (dbEntry.Id != 0)
                        {
                            entry.State = EntityState.Modified;
                        }
                        else
                        {
                            entry.State = EntityState.Added;
                        }
                    }
                }

                await context.SaveChangesAsync();

                result.Object = entity;
                result.Success = true;

                return result;
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Exception = exp;

                return result;
            }
        }

        async public Task<IRepositoryResponse> GetById(int id, bool fullRecord = true)
        {
            var result = new RepositoryResponse();
            var query = new App();

            try
            {
                if (fullRecord)
                {
                    query = await context
                        .Apps
                        .FirstOrDefaultAsync(predicate: a => a.Id == id);

                    if (query == null)
                    {
                        result.Success = false;
                        result.Object = query;

                        return result;
                    }

                    query.Users = await context
                        .UsersApps
                        .Where(ua => ua.AppId == query.Id)
                        .ToListAsync();

                    foreach (var userApp in query.Users)
                    {
                        userApp.User = await context
                            .Users
                            .FirstOrDefaultAsync(predicate: u => u.Id == userApp.UserId);

                        userApp.User.Roles = await context
                            .UsersRoles
                            .Where(ur => ur.UserId == userApp.User.Id).ToListAsync();

                        foreach (var role in userApp.User.Roles)
                        {
                            role.Role = await context
                                .Roles
                                .FirstOrDefaultAsync(predicate: r => r.Id == role.RoleId);

                            role.Role.Users = null;
                        }

                        foreach (var game in userApp.User.Games)
                        {
                            if (await game.IsGameInActiveApp(context))
                            {
                                game.SudokuMatrix = await context
                                    .SudokuMatrices
                                    .FirstOrDefaultAsync(predicate: m => m.Id == game.SudokuMatrixId);

                                await game.SudokuMatrix.AttachSudokuCells(context);
                            }
                        }
                    }
                }
                else
                {
                    query = await context
                        .Apps
                        .FirstOrDefaultAsync(predicate: a => a.Id == id);

                    if (query == null)
                    {
                        result.Success = false;
                        result.Object = query;

                        return result;
                    }
                }

                result.Success = true;
                result.Object = query;

                return result;
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Exception = exp;

                return result;
            }
        }

        async public Task<IRepositoryResponse> GetByLicense(string license, bool fullRecord = true)
        {
            var result = new RepositoryResponse();
            var query = new App();

            try
            {
                if (fullRecord)
                {
                    query = await context
                        .Apps
                        .FirstOrDefaultAsync(
                            predicate: a => a.License.ToLower().Equals(license.ToLower()));

                    if (query == null)
                    {
                        result.Success = false;
                        result.Object = query;

                        return result;
                    }

                    query.Users = await context
                        .UsersApps
                        .Where(ua => ua.AppId == query.Id)
                        .ToListAsync();

                    foreach (var userApp in query.Users)
                    {
                        userApp.User = await context
                            .Users
                            .FirstOrDefaultAsync(predicate: u => u.Id == userApp.UserId);

                        userApp.User.Roles = await context
                            .UsersRoles
                            .Where(ur => ur.UserId == userApp.User.Id).ToListAsync();

                        foreach (var role in userApp.User.Roles)
                        {
                            role.Role = await context
                                .Roles
                                .FirstOrDefaultAsync(predicate: r => r.Id == role.RoleId);

                            role.Role.Users = null;
                        }

                        foreach (var game in userApp.User.Games)
                        {
                            if (await game.IsGameInActiveApp(context))
                            {
                                game.SudokuMatrix = await context
                                    .SudokuMatrices
                                    .FirstOrDefaultAsync(predicate: m => m.Id == game.SudokuMatrixId);

                                await game.SudokuMatrix.AttachSudokuCells(context);
                            }
                        }
                    }
                }
                else
                {
                    query = await context
                        .Apps
                        .FirstOrDefaultAsync(
                            predicate: a => a.License.ToLower().Equals(license.ToLower()));

                    if (query == null)
                    {
                        result.Success = false;
                        result.Object = query;

                        return result;
                    }
                }

                result.Success = true;
                result.Object = query;

                return result;
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Exception = exp;

                return result;
            }
        }

        async public Task<IRepositoryResponse> GetAll(bool fullRecord = true)
        {
            var result = new RepositoryResponse();
            var query = new List<App>();

            try
            {
                if (fullRecord)
                {
                    query = await context
                        .Apps
                        .ToListAsync();

                    foreach (var app in query)
                    {
                        app.Users = await context
                            .UsersApps
                            .Where(ua => ua.AppId == app.Id)
                            .ToListAsync();

                        foreach (var userApp in app.Users)
                        {
                            userApp.User = await context
                                .Users
                                .FirstOrDefaultAsync(predicate: u => u.Id == userApp.UserId);

                            userApp.User.Roles = await context
                                .UsersRoles
                                .Where(ur => ur.UserId == userApp.User.Id).ToListAsync();

                            foreach (var role in userApp.User.Roles)
                            {
                                role.Role = await context
                                    .Roles
                                    .FirstOrDefaultAsync(predicate: r => r.Id == role.RoleId);

                                role.Role.Users = null;
                            }

                            foreach (var game in userApp.User.Games)
                            {
                                if (await game.IsGameInActiveApp(context))
                                {
                                    game.SudokuMatrix = await context
                                        .SudokuMatrices
                                        .FirstOrDefaultAsync(predicate: m => m.Id == game.SudokuMatrixId);

                                    await game.SudokuMatrix.AttachSudokuCells(context);
                                }
                            }
                        }
                    }
                }
                else
                {
                    query = await context.Apps.ToListAsync();
                }

                result.Success = true;
                result.Objects = query
                    .ConvertAll(a => (IEntityBase)a)
                    .ToList();

                return result;
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Exception = exp;

                return result;
            }
        }

        async public Task<IRepositoryResponse> GetAppUsers(int id, bool fullRecord = true)
        {
            var result = new RepositoryResponse();
            var query = new List<User>();

            try
            {
                if (fullRecord)
                {
                    query = await context
                        .Users
                        .Include(u => u.Games)
                        .Include(u => u.Roles)
                        .Include(u => u.Apps)
                        .Where(u => u.Apps.Any(ua => ua.AppId == id))
                        .ToListAsync();

                    foreach (var user in query)
                    {
                        foreach (var role in user.Roles)
                        {
                            role.Role = await context
                                .Roles
                                .FirstOrDefaultAsync(predicate: r => r.Id == role.RoleId);

                            role.Role.Users = null;
                        }

                        user.Apps = new List<UserApp>();

                        foreach (var game in user.Games)
                        {
                            if (await game.IsGameInActiveApp(context))
                            {
                                game.SudokuMatrix = await context
                                    .SudokuMatrices
                                    .FirstOrDefaultAsync(predicate: m => m.Id == game.SudokuMatrixId);

                                await game.SudokuMatrix.AttachSudokuCells(context);
                            }
                        }
                    }
                }
                else
                {
                    query = await context
                        .Users
                        .Where(u => u.Apps.Any(ua => ua.AppId == id))
                        .ToListAsync();
                }

                if (query.Count > 0)
                {
                    result.Success = true;
                    result.Objects = query
                        .ConvertAll(u => (IEntityBase)u)
                        .ToList();
                }
                else
                {
                    result.Success = false;
                }

                return result;
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Exception = exp;

                return result;
            }
        }

        async public Task<IRepositoryResponse> Update(TEntity entity)
        {
            var result = new RepositoryResponse();

            try
            {
                context.Apps.Attach(entity);

                foreach (var entry in context.ChangeTracker.Entries())
                {
                    var dbEntry = (IEntityBase)entry.Entity;

                    if (dbEntry.Id != 0)
                    {
                        entry.State = EntityState.Modified;
                    }
                    else
                    {
                        entry.State = EntityState.Added;
                    }
                }

                await context.SaveChangesAsync();

                result.Success = true;
                result.Object = await context
                    .Apps
                    .FirstOrDefaultAsync(predicate: a => a.Id == entity.Id);

                ((App)result.Object).Users = await context
                    .UsersApps
                    .Where(ua => ua.AppId == ((App)result.Object).Id)
                    .ToListAsync();

                foreach (var userApp in ((App)result.Object).Users)
                {
                    userApp.User = await context
                        .Users
                        .FirstOrDefaultAsync(predicate: u => u.Id == userApp.UserId);

                    userApp.User.Roles = await context
                        .UsersRoles
                        .Where(ur => ur.UserId == userApp.User.Id).ToListAsync();

                    foreach (var role in userApp.User.Roles)
                    {
                        role.Role = await context
                            .Roles
                            .FirstOrDefaultAsync(predicate: r => r.Id == role.RoleId);

                        role.Role.Users = null;
                    }

                    foreach (var game in userApp.User.Games)
                    {
                        if (await game.IsGameInActiveApp(context))
                        {
                            game.SudokuMatrix = await context
                                .SudokuMatrices
                                .FirstOrDefaultAsync(predicate: m => m.Id == game.SudokuMatrixId);

                            await game.SudokuMatrix.AttachSudokuCells(context);
                        }
                    }
                }

                return result;
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Exception = exp;

                return result;
            }
        }

        async public Task<IRepositoryResponse> UpdateRange(List<TEntity> entities)
        {
            var result = new RepositoryResponse();

            try
            {
                context.Apps.UpdateRange(entities);

                foreach (var entry in context.ChangeTracker.Entries())
                {
                    var dbEntry = (IEntityBase)entry.Entity;

                    if (dbEntry.Id != 0)
                    {
                        entry.State = EntityState.Modified;
                    }
                    else
                    {
                        entry.State = EntityState.Added;
                    }
                }

                await context.SaveChangesAsync();

                result.Success = true;

                return result;
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Exception = exp;

                return result;
            }
        }

        async public Task<IRepositoryResponse> AddAppUser(int userId, string license)
        {
            var result = new RepositoryResponse();

            try
            {
                var user = await context
                    .Users
                    .FirstOrDefaultAsync(predicate: u => u.Id == userId);

                var app = await context
                    .Apps
                    .FirstOrDefaultAsync(predicate: a => a.License.Equals(license));

                if (user == null || app == null)
                {
                    result.Success = false;

                    return result;
                }

                var userApp = new UserApp(user.Id, app.Id);

                context.UsersApps.Add(userApp);

                foreach (var entry in context.ChangeTracker.Entries())
                {
                    if (entry.Entity is UserApp ua)
                    {
                        if (userApp.Id == ua.Id)
                        {
                            entry.State = EntityState.Added;
                        }
                        else
                        {
                            entry.State = EntityState.Modified;
                        }
                    }
                    else
                    {
                        var dbEntry = (IEntityBase)entry.Entity;

                        if (dbEntry.Id != 0)
                        {
                            entry.State = EntityState.Modified;
                        }
                        else
                        {
                            entry.State = EntityState.Added;
                        }
                    }
                }

                await context.SaveChangesAsync();

                result.Success = true;

                return result;
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Exception = exp;

                return result;
            }
        }

        async public Task<IRepositoryResponse> RemoveAppUser(int userId, string license)
        {
            var result = new RepositoryResponse();

            try
            {
                var user = await context
                    .Users
                    .Include(u => u.Games)
                    .FirstOrDefaultAsync(predicate: u => u.Id == userId);

                var app = await context
                    .Apps
                    .FirstOrDefaultAsync(predicate: a => a.License.Equals(license));

                if (user == null || app == null)
                {
                    result.Success = false;

                    return result;
                }

                var cellList = new List<int>();
                var matrixList = new List<int>();
                var gameList = new List<int>();
                var userAppList = new List<int>();

                user.Games = await context
                    .Games
                    .Where(g => g.User.Apps.Any(
                        ua => ua.App.License.ToLower().Equals(license.ToLower())))
                    .ToListAsync();

                foreach (var game in user.Games)
                {
                    game.SudokuMatrix = await context
                        .SudokuMatrices
                        .FirstOrDefaultAsync(predicate: m => m.Id == game.SudokuMatrixId);

                    await game.SudokuMatrix.AttachSudokuCells(context);
                }

                foreach (var game in user.Games)
                {
                    foreach (var cell in game.SudokuMatrix.SudokuCells)
                    {
                        context.SudokuCells.Remove(cell);
                        cellList.Add(cell.Id);
                    }

                    context.SudokuMatrices.Remove(game.SudokuMatrix);
                    matrixList.Add(game.SudokuMatrixId);
                    context.Games.Remove(game);
                    gameList.Add(game.Id);
                }

                var userApp = await context
                    .UsersApps
                    .FirstOrDefaultAsync(predicate: ua => ua.UserId == userId);

                context.UsersApps.Remove(userApp);
                userAppList.Add(userApp.Id);

                foreach (var entry in context.ChangeTracker.Entries())
                {
                    if (entry.Entity is SudokuCell cell)
                    {
                        if (cellList.Contains(cell.Id))
                        {
                            entry.State = EntityState.Deleted;
                        }
                        else
                        {
                            entry.State = EntityState.Modified;
                        }
                    }
                    else if (entry.Entity is SudokuMatrix matrix)
                    {
                        if (matrixList.Contains(matrix.Id))
                        {
                            entry.State = EntityState.Deleted;
                        }
                        else
                        {
                            entry.State = EntityState.Modified;
                        }
                    }
                    else if (entry.Entity is Game game)
                    {
                        if (gameList.Contains(game.Id))
                        {
                            entry.State = EntityState.Deleted;
                        }
                        else
                        {
                            entry.State = EntityState.Modified;
                        }
                    }
                    else if (entry.Entity is UserApp entity)
                    {
                        if (userAppList.Contains(entity.Id))
                        {
                            entry.State = EntityState.Deleted;
                        }
                        else
                        {
                            entry.State = EntityState.Modified;
                        }
                    }
                    else
                    {
                        entry.State = EntityState.Modified;
                    }
                }

                await context.SaveChangesAsync();

                result.Success = true;

                return result;
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Exception = exp;

                return result;
            }
        }

        async public Task<IRepositoryResponse> Delete(TEntity entity)
        {
            var result = new RepositoryResponse();

            try
            {
                if (await context.Apps.AnyAsync(predicate: a => a.Id == entity.Id))
                {
                    foreach (var userApp in entity.Users)
                    {
                        foreach (var game in userApp.User.Games)
                        {
                            foreach (var cell in game.SudokuMatrix.SudokuCells)
                            {
                                context.SudokuCells.Remove(cell);
                            }

                            context.Games.Remove(game);
                        }

                        context.UsersApps.Remove(userApp);
                    }

                    context.Apps.Remove(entity);
                    await context.SaveChangesAsync();

                    result.Success = true;

                    return result;
                }
                else
                {
                    result.Success = false;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Exception = exp;

                return result;
            }
        }

        async public Task<IRepositoryResponse> DeleteRange(List<TEntity> entities)
        {
            var result = new RepositoryResponse();

            try
            {
                foreach (var app in entities)
                {
                    foreach (var userApp in app.Users)
                    {
                        foreach (var game in userApp.User.Games)
                        {
                            foreach (var cell in game.SudokuMatrix.SudokuCells)
                            {
                                context.SudokuCells.Remove(cell);
                            }

                            context.Games.Remove(game);
                        }

                        context.UsersApps.Remove(userApp);
                    }

                    context.Apps.Remove(app);
                }

                await context.SaveChangesAsync();

                result.Success = true;

                return result;
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Exception = exp;

                return result;
            }
        }

        async public Task<IRepositoryResponse> Reset(TEntity entity)
        {
            var result = new RepositoryResponse();

            try
            {
                if (await context.Apps.AnyAsync(predicate: a => a.Id == entity.Id))
                {
                    foreach (var ua in entity.Users)
                    {
                        foreach (var game in ua.User.Games)
                        {
                            await game.SudokuMatrix.AttachSudokuCells(context);
                        }
                    }

                    foreach (var ua in entity.Users)
                    {
                        foreach (var game in ua.User.Games)
                        {
                            foreach (var cell in game.SudokuMatrix.SudokuCells)
                            {
                                context.SudokuCells.Remove(cell);
                            }

                            context.SudokuMatrices.Remove(game.SudokuMatrix);
                        }
                    }

                    foreach (var ua in entity.Users)
                    {
                        foreach (var game in ua.User.Games)
                        {
                            context.Games.Remove(game);
                        }
                    }

                    result.Success = true;

                    return result;
                }
                else
                {
                    result.Success = false;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Exception = exp;

                return result;
            }
        }

        async public Task<IRepositoryResponse> Activate(int id)
        {
            var result = new RepositoryResponse();

            try
            {
                if (await context.Apps.AnyAsync(predicate: a => a.Id == id))
                {
                    var app = await context.Apps.FindAsync(id);

                    if (app != null)
                    {
                        if (!app.IsActive)
                        {
                            app.ActivateApp();
                            context.Apps.Update(app);
                            await context.SaveChangesAsync();
                        }

                        result.Success = true;

                        return result;
                    }
                    else
                    {
                        result.Success = false;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Exception = exp;

                return result;
            }
        }

        async public Task<IRepositoryResponse> Deactivate(int id)
        {
            var result = new RepositoryResponse();

            try
            {
                if (await context.Apps.AnyAsync(predicate: a => a.Id == id))
                {
                    var app = await context.Apps.FindAsync(id);

                    if (app != null)
                    {
                        if (app.IsActive)
                        {
                            app.DeactivateApp();
                            context.Apps.Update(app);
                            await context.SaveChangesAsync();
                        }

                        result.Success = true;

                        return result;
                    }
                    else
                    {
                        result.Success = false;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Exception = exp;

                return result;
            }
        }

        async public Task<bool> HasEntity(int id)
        {
            var result = await context.Apps.AnyAsync(predicate: a => a.Id == id);

            return result;
        }

        async public Task<bool> IsAppLicenseValid(string license)
        {
            var result = await context
                .Apps
                .AnyAsync(
                    predicate: app => app.License.ToLower().Equals(license.ToLower()));

            return result;
        }

        async public Task<bool> IsUserRegisteredToApp(int id, string license, int userId)
        {
            var result = await context
                .Apps
                .AnyAsync(predicate:
                    a => a.Users.Any(ua => ua.UserId == userId)
                    && a.Id == id
                    && a.License.ToLower().Equals(license.ToLower()));

            return result;
        }

        async public Task<bool> IsUserOwnerOfApp(int id, string license, int userId)
        {
            var result = await context
                .Apps
                .AnyAsync(predicate:
                    a => a.License.ToLower().Equals(license.ToLower())
                    && a.OwnerId == userId
                    && a.Id == id);

            return result;
        }

        async public Task<string> GetLicense(int id)
        {
            var license = await context
                .Apps
                .Where(a => a.Id == id)
                .Select(a => a.License)
                .FirstOrDefaultAsync();

            return license;
        }
        #endregion
    }
}

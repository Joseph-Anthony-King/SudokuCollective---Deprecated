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
        public async Task<IRepositoryResponse> Add(TEntity entity)
        {
            var result = new RepositoryResponse();

            try
            {
                if (entity.Id != 0)
                {
                    result.Success = false;

                    return result;
                }

                context.Attach(entity);

                var user = await context
                    .Users
                    .FirstOrDefaultAsync(u => u.Id == entity.OwnerId);

                // Add connection between the app and the user
                var userApp = new UserApp
                {
                    User = user,
                    UserId = user.Id,
                    App = entity,
                    AppId = entity.Id
                };

                entity.Users.Add(userApp);

                context.Attach(userApp);

                foreach (var entry in context.ChangeTracker.Entries())
                {
                    if (entry.Entity is App app)
                    {
                        if (app.Id == entity.Id)
                        {
                            entry.State = EntityState.Added;
                        }
                        else
                        {
                            entry.State = EntityState.Modified;
                        }
                    }
                    else if (entry.Entity is UserApp ua)
                    {
                        if (ua.Id == userApp.Id)
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

                        if (dbEntry.Id == 0)
                        {
                            entry.State = EntityState.Added;
                        }
                        else
                        {
                            entry.State = EntityState.Modified;
                        }
                    }
                }

                await context.SaveChangesAsync();

                // Ensure that the owner has admin priviledges, if not they will be promoted
                var addAdminRole = true;
                var newUserAdminRole = new UserRole();

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
                        .FirstOrDefaultAsync(r => r.RoleLevel == RoleLevel.ADMIN);

                    newUserAdminRole = new UserRole
                    {
                        User = user,
                        UserId = user.Id,
                        Role = adminRole,
                        RoleId = adminRole.Id
                    };

                    var appAdmin = new AppAdmin
                    {
                        AppId = entity.Id,
                        UserId = user.Id
                    };

                    context.Attach(newUserAdminRole);

                    context.Attach(appAdmin);

                    foreach (var entry in context.ChangeTracker.Entries())
                    {
                        if (entry.Entity is UserApp ua)
                        {
                            if (ua.Id == newUserAdminRole.Id)
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

                            if (dbEntry.Id == 0)
                            {
                                entry.State = EntityState.Added;
                            }
                            else
                            {
                                entry.State = EntityState.Modified;
                            }
                        }
                    }

                    await context.SaveChangesAsync();
                }

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

        public async Task<IRepositoryResponse> GetById(int id, bool fullRecord = true)
        {
            var result = new RepositoryResponse();
            var query = new App();

            try
            {
                if (fullRecord)
                {
                    query = await context
                        .Apps
                        .Include(a => a.Users)
                            .ThenInclude(ua => ua.User)
                                .ThenInclude(u => u.Roles)
                                    .ThenInclude(ur => ur.Role)
                        .FirstOrDefaultAsync(a => a.Id == id);

                    if (query != null)
                    {
                        // Filter games by app
                        foreach (var userApp in query.Users)
                        {
                            userApp.User.Games = new List<Game>();

                            userApp.User.Games = await context
                                .Games
                                .Include(g => g.SudokuMatrix)
                                    .ThenInclude(g => g.Difficulty)
                                .Include(g => g.SudokuMatrix)
                                    .ThenInclude(m => m.SudokuCells)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.AppId == userApp.AppId && g.UserId == userApp.UserId)
                                .ToListAsync();
                        }
                    }
                }
                else
                {
                    query = await context
                        .Apps
                        .FirstOrDefaultAsync(a => a.Id == id);
                }

                if (query == null)
                {
                    result.Success = false;

                    return result;
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

        public async Task<IRepositoryResponse> GetByLicense(string license, bool fullRecord = true)
        {
            var result = new RepositoryResponse();
            var query = new App();

            try
            {
                if (fullRecord)
                {
                    query = await context
                        .Apps
                        .Include(a => a.Users)
                            .ThenInclude(ua => ua.User)
                                .ThenInclude(u => u.Roles)
                                    .ThenInclude(ur => ur.Role)
                        .FirstOrDefaultAsync(
                            a => a.License.ToLower().Equals(license.ToLower()));

                    if (query != null)
                    {
                        // Filter games by app
                        foreach (var userApp in query.Users)
                        {
                            userApp.User.Games = new List<Game>();

                            userApp.User.Games = await context
                                .Games
                                .Include(g => g.SudokuMatrix)
                                    .ThenInclude(g => g.Difficulty)
                                .Include(g => g.SudokuMatrix)
                                    .ThenInclude(m => m.SudokuCells)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.AppId == userApp.AppId && g.UserId == userApp.UserId)
                                .ToListAsync();
                        }
                    }
                }
                else
                {
                    query = await context
                        .Apps
                        .FirstOrDefaultAsync(a => a.License.ToLower().Equals(license.ToLower()));
                }

                if (query == null)
                {
                    result.Success = false;

                    return result;
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

        public async Task<IRepositoryResponse> GetAll(bool fullRecord = true)
        {
            var result = new RepositoryResponse();
            var query = new List<App>();

            try
            {
                if (fullRecord)
                {
                    query = await context
                        .Apps
                        .Include(a => a.Users)
                            .ThenInclude(ua => ua.User)
                                .ThenInclude(u => u.Roles)
                                    .ThenInclude(ur => ur.Role)
                        .OrderBy(a => a.Id)
                        .ToListAsync();

                    if (query.Count != 0)
                    {
                        // Filter games by app
                        foreach (var app in query)
                        {
                            foreach (var userApp in app.Users)
                            {
                                userApp.User.Games = new List<Game>();

                                userApp.User.Games = await context
                                    .Games
                                    .Include(g => g.SudokuMatrix)
                                        .ThenInclude(g => g.Difficulty)
                                    .Include(g => g.SudokuMatrix)
                                        .ThenInclude(m => m.SudokuCells)
                                    .Include(g => g.SudokuSolution)
                                    .Where(g => g.AppId == userApp.AppId && g.UserId == userApp.UserId)
                                    .ToListAsync();
                            }
                        }
                    }
                }
                else
                {
                    query = await context
                        .Apps
                        .OrderBy(a => a.Id)
                        .ToListAsync();
                }

                if (query == null)
                {
                    result.Success = false;

                    return result;
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

        public async Task<IRepositoryResponse> GetMyApps(int userId, bool fullRecord = true)
        {
            var result = new RepositoryResponse();
            var query = new List<App>();

            try
            {
                if (fullRecord)
                {
                    query = await context
                        .Apps
                        .Where(a => a.OwnerId == userId)
                        .Include(a => a.Users)
                            .ThenInclude(ua => ua.User)
                                .ThenInclude(u => u.Roles)
                                    .ThenInclude(ur => ur.Role)
                        .OrderBy(a => a.Id)
                        .ToListAsync();

                    if (query.Count != 0)
                    {
                        // Filter games by app
                        foreach (var app in query)
                        {
                            foreach (var userApp in app.Users)
                            {
                                userApp.User.Games = new List<Game>();

                                userApp.User.Games = await context
                                    .Games
                                    .Include(g => g.SudokuMatrix)
                                        .ThenInclude(g => g.Difficulty)
                                    .Include(g => g.SudokuMatrix)
                                        .ThenInclude(m => m.SudokuCells)
                                    .Include(g => g.SudokuSolution)
                                    .Where(g => g.AppId == userApp.AppId && g.UserId == userApp.UserId)
                                    .ToListAsync();
                            }
                        }
                    }
                }
                else
                {
                    query = await context
                        .Apps
                        .Where(a => a.OwnerId == userId)
                        .OrderBy(a => a.Id)
                        .ToListAsync();
                }

                if (query == null)
                {
                    result.Success = false;

                    return result;
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

        public async Task<IRepositoryResponse> GetAppUsers(int id, bool fullRecord = true)
        {
            var result = new RepositoryResponse();
            var query = new List<User>();

            try
            {
                if (fullRecord)
                {
                    query = await context
                        .Users
                        .Include(u => u.Apps)
                            .ThenInclude(ua => ua.App)
                        .Include(u => u.Roles)
                            .ThenInclude(ur => ur.Role)
                        .Include(u => u.Games)
                            .ThenInclude(g => g.SudokuSolution)
                        .Include(u => u.Games)
                            .ThenInclude(g => g.SudokuMatrix)
                                .ThenInclude(m => m.SudokuCells)
                        .Where(u => u.Apps.Any(ua => ua.AppId == id))
                        .OrderBy(u => u.Id)
                        .ToListAsync();

                    if (query.Count != 0)
                    {
                        foreach (var user in query)
                        {
                            // Filter games by app
                            user.Games = new List<Game>();

                            user.Games = await context
                                .Games
                                .Where(g => g.AppId == id && g.UserId == user.Id)
                                .ToListAsync();
                        }
                    }
                }
                else
                {
                    query = await context
                        .Users
                        .Where(u => u.Apps.Any(ua => ua.AppId == id))
                        .OrderBy(u => u.Id)
                        .ToListAsync();
                }

                if (query.Count == 0)
                {
                    result.Success = false;

                    return result;
                }

                result.Success = true;
                result.Objects = query
                    .ConvertAll(u => (IEntityBase)u)
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

        public async Task<IRepositoryResponse> Update(TEntity entity)
        {
            var result = new RepositoryResponse();

            try
            {
                if (entity.Id == 0)
                {
                    result.Success = false;

                    return result;
                }

                if (await context.Apps.AnyAsync(a => a.Id == entity.Id))
                {
                    entity.DateUpdated = DateTime.UtcNow;

                    context.Attach(entity);

                    foreach (var entry in context.ChangeTracker.Entries())
                    {
                        var dbEntry = (IEntityBase)entry.Entity;

                        if (dbEntry is UserApp)
                        {
                            entry.State = EntityState.Modified;
                        }
                        else if (dbEntry is UserRole)
                        {
                            entry.State = EntityState.Modified;
                        }
                        else
                        {
                            // Otherwise do nothing...
                        }
                    }

                    await context.SaveChangesAsync();

                    result.Success = true;
                    result.Object = entity;

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

        public async Task<IRepositoryResponse> UpdateRange(List<TEntity> entities)
        {
            var result = new RepositoryResponse();

            try
            {
                var dateUpdated = DateTime.UtcNow;

                foreach (var entity in entities)
                {
                    if (entity.Id == 0)
                    {
                        result.Success = false;

                        return result;
                    }

                    if (await context.Apps.AnyAsync(a => a.Id == entity.Id))
                    {
                        entity.DateUpdated = dateUpdated;
                    }
                    else
                    {
                        result.Success = false;

                        return result;
                    }
                }

                context.Apps.UpdateRange(entities);

                foreach (var entry in context.ChangeTracker.Entries())
                {
                    var dbEntry = (IEntityBase)entry.Entity;

                    if (dbEntry is UserApp)
                    {
                        entry.State = EntityState.Modified;
                    }
                    else if (dbEntry is UserRole)
                    {
                        entry.State = EntityState.Modified;
                    }
                    else
                    {
                        // Otherwise do nothing...
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

        public async Task<IRepositoryResponse> AddAppUser(int userId, string license)
        {
            var result = new RepositoryResponse();

            try
            {
                var user = await context
                    .Users
                    .FirstOrDefaultAsync(u => u.Id == userId);

                var app = await context
                    .Apps
                    .FirstOrDefaultAsync(
                        a => a.License.ToLower().Equals(license.ToLower()));

                if (user == null || app == null)
                {
                    result.Success = false;

                    return result;
                }

                var userApp = new UserApp
                {
                    User = user,
                    UserId = user.Id,
                    App = app,
                    AppId = app.Id
                };

                context.Attach(userApp);

                foreach (var entry in context.ChangeTracker.Entries())
                {
                    var dbEntry = (IEntityBase)entry.Entity;

                    if (dbEntry is UserApp ua)
                    {
                        if (ua.Id == userApp.Id)
                        {
                            entry.State = EntityState.Added;
                        }
                        else
                        {
                            entry.State = EntityState.Modified;
                        }
                    }
                    else if (dbEntry is UserRole)
                    {
                        entry.State = EntityState.Modified;
                    }
                    else
                    {
                        // Otherwise do nothing...
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

        public async Task<IRepositoryResponse> RemoveAppUser(int userId, string license)
        {
            var result = new RepositoryResponse();

            try
            {
                var app = await context
                    .Apps
                    .FirstOrDefaultAsync(
                        a => a.License.ToLower().Equals(license.ToLower()));

                var user = await context
                    .Users
                    .FirstOrDefaultAsync(
                        u => u.Id == userId && 
                        u.Apps.Any(ua => ua.AppId == app.Id));

                if (user == null || app == null)
                {
                    result.Success = false;

                    return result;
                }

                user.Games = new List<Game>();

                user.Games = await context
                    .Games
                    .Include(g => g.SudokuMatrix)
                        .ThenInclude(g => g.Difficulty)
                    .Include(g => g.SudokuMatrix)
                        .ThenInclude(m => m.SudokuCells)
                    .Where(g => g.AppId == app.Id)
                    .ToListAsync();

                foreach (var game in user.Games)
                {
                    if (game.AppId == app.Id)
                    {
                        context.Remove(game);
                    }
                }

                foreach (var entry in context.ChangeTracker.Entries())
                {
                    if (entry.Entity is UserApp userApp)
                    {
                        if (user.Apps.Any(ua => ua.Id == userApp.Id))
                        {
                            entry.State = EntityState.Deleted;
                        }
                        else
                        {
                            entry.State = EntityState.Modified;
                        }
                    }
                    else if (entry.Entity is UserRole)
                    {
                        entry.State = EntityState.Modified;
                    }
                    else
                    {

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

        public async Task<IRepositoryResponse> Delete(TEntity entity)
        {
            var result = new RepositoryResponse();

            try
            {
                if (await context.Apps.AnyAsync(a => a.Id == entity.Id))
                {
                    context.Remove(entity);

                    var games = await context
                        .Games
                        .Include(g => g.SudokuMatrix)
                            .ThenInclude(g => g.Difficulty)
                        .Include(g => g.SudokuMatrix)
                            .ThenInclude(m => m.SudokuCells)
                        .Where(g => g.AppId == entity.Id)
                        .ToListAsync();

                    context.RemoveRange(games);

                    foreach (var entry in context.ChangeTracker.Entries())
                    {
                        var dbEntry = (IEntityBase)entry.Entity;

                        if (dbEntry is UserApp userApp)
                        {
                            if (userApp.AppId == entity.Id)
                            {
                                entry.State = EntityState.Deleted;
                            }
                            else
                            {
                                entry.State = EntityState.Modified;
                            }
                        }
                        else if (dbEntry is UserRole)
                        {
                            entry.State = EntityState.Modified;
                        }
                        else if (dbEntry is SudokuSolution)
                        {
                            entry.State = EntityState.Modified;
                        }
                        else
                        {
                            // Otherwise do nothing...
                        }
                    }

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

        public async Task<IRepositoryResponse> DeleteRange(List<TEntity> entities)
        {
            var result = new RepositoryResponse();

            try
            {
                foreach (var entity in entities)
                {
                    if (entity.Id == 0)
                    {
                        result.Success = false;

                        return result;
                    }

                    if (await context.Apps.AnyAsync(a => a.Id == entity.Id))
                    {
                        context.Remove(entity);

                        var games = await context
                            .Games
                            .Include(g => g.SudokuMatrix)
                                .ThenInclude(g => g.Difficulty)
                            .Include(g => g.SudokuMatrix)
                                .ThenInclude(m => m.SudokuCells)
                            .Where(g => g.AppId == entity.Id)
                            .ToListAsync();

                        context.RemoveRange(games);
                    }
                    else
                    {
                        result.Success = false;

                        return result;
                    }
                }

                foreach (var entity in entities)
                {
                    foreach (var entry in context.ChangeTracker.Entries())
                    {
                        var dbEntry = (IEntityBase)entry.Entity;

                        if (dbEntry is UserApp userApp)
                        {
                            if (userApp.AppId == entity.Id)
                            {
                                entry.State = EntityState.Deleted;
                            }
                            else
                            {
                                entry.State = EntityState.Modified;
                            }
                        }
                        else if (dbEntry is UserRole)
                        {
                            entry.State = EntityState.Modified;
                        }
                        else if (dbEntry is SudokuSolution)
                        {
                            entry.State = EntityState.Modified;
                        }
                        else
                        {
                            // Otherwise do nothing...
                        }
                    }

                    await context.SaveChangesAsync();
                }

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

        public async Task<IRepositoryResponse> Reset(TEntity entity)
        {
            var result = new RepositoryResponse();

            try
            {
                if (await context.Apps.AnyAsync(a => a.Id == entity.Id))
                {
                    var games = await context
                        .Games
                        .Include(g => g.SudokuMatrix)
                            .ThenInclude(g => g.Difficulty)
                        .Include(g => g.SudokuMatrix)
                            .ThenInclude(m => m.SudokuCells)
                        .Where(g => g.AppId == entity.Id)
                        .ToListAsync();

                    context.RemoveRange(games);

                    foreach (var entry in context.ChangeTracker.Entries())
                    {
                        var dbEntry = (IEntityBase)entry.Entity;

                        if (dbEntry is UserApp userApp)
                        {
                            entry.State = EntityState.Modified;
                        }
                        else if (dbEntry is UserRole)
                        {
                            entry.State = EntityState.Modified;
                        }
                        else if (dbEntry is SudokuSolution)
                        {
                            entry.State = EntityState.Modified;
                        }
                        else
                        {
                            // Otherwise do nothing...
                        }
                    }

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

        public async Task<IRepositoryResponse> Activate(int id)
        {
            var result = new RepositoryResponse();

            try
            {
                if (await context.Apps.AnyAsync(a => a.Id == id))
                {
                    var app = await context.Apps.FindAsync(id);

                    if (app != null)
                    {
                        if (!app.IsActive)
                        {
                            app.ActivateApp();

                            context.Attach(app);

                            foreach (var entry in context.ChangeTracker.Entries())
                            {
                                var dbEntry = (IEntityBase)entry.Entity;

                                if (dbEntry is UserApp)
                                {
                                    entry.State = EntityState.Modified;
                                }
                                else if (dbEntry is UserRole)
                                {
                                    entry.State = EntityState.Modified;
                                }
                                else
                                {
                                    // Otherwise do nothing...
                                }
                            }

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

        public async Task<IRepositoryResponse> Deactivate(int id)
        {
            var result = new RepositoryResponse();

            try
            {
                if (await context.Apps.AnyAsync(a => a.Id == id))
                {
                    var app = await context.Apps.FindAsync(id);

                    if (app != null)
                    {
                        if (app.IsActive)
                        {
                            app.DeactivateApp();

                            context.Attach(app);

                            foreach (var entry in context.ChangeTracker.Entries())
                            {
                                var dbEntry = (IEntityBase)entry.Entity;

                                if (dbEntry is UserApp)
                                {
                                    entry.State = EntityState.Modified;
                                }
                                else if (dbEntry is UserRole)
                                {
                                    entry.State = EntityState.Modified;
                                }
                                else
                                {
                                    // Otherwise do nothing...
                                }
                            }

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

        public async Task<bool> HasEntity(int id)
        {
            return await context.Apps.AnyAsync(a => a.Id == id);
        }

        public async Task<bool> IsAppLicenseValid(string license)
        {
            var result = await context
                .Apps
                .AnyAsync(
                    app => app.License.ToLower().Equals(license.ToLower()));

            return result;
        }

        public async Task<bool> IsUserRegisteredToApp(int id, string license, int userId)
        {
            var result = await context
                .Apps
                .AnyAsync(
                    a => a.Users.Any(ua => ua.UserId == userId)
                    && a.Id == id
                    && a.License.ToLower().Equals(license.ToLower()));

            return result;
        }

        public async Task<bool> IsUserOwnerOfApp(int id, string license, int userId)
        {
            var result = await context
                .Apps
                .AnyAsync(
                    a => a.License.ToLower().Equals(license.ToLower())
                    && a.OwnerId == userId
                    && a.Id == id);

            return result;
        }

        public async Task<string> GetLicense(int id)
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

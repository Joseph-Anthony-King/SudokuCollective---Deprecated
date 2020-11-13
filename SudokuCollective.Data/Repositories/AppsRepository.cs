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
        private readonly DbSet<App> dbSet;
        private readonly DbSet<User> userDbSet;
        private readonly DbSet<UserApp> userAppDbSet;
        private readonly DbSet<Role> roleDbSet;
        private readonly DbSet<UserRole> userRoleDbSet;
        private readonly DbSet<Game> gameDbSet;
        private readonly DbSet<SudokuMatrix> matrixDbSet;
        private readonly DbSet<SudokuCell> cellDbSet;
        #endregion

        #region Constructor
        public AppsRepository(DatabaseContext databaseContext)
        {
            context = databaseContext;
            dbSet = context.Set<App>();
            userDbSet = context.Set<User>();
            userAppDbSet = context.Set<UserApp>();
            roleDbSet = context.Set<Role>();
            userRoleDbSet = context.Set<UserRole>();
            gameDbSet = context.Set<Game>();
            matrixDbSet = context.Set<SudokuMatrix>();
            cellDbSet = context.Set<SudokuCell>();
        }
        #endregion

        #region Methods
        async public Task<IRepositoryResponse> Create(TEntity entity)
        {
            var result = new RepositoryResponse();

            try
            {
                dbSet.Add(entity);

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

                // Add connection between the app and the user
                var userApp = new UserApp(
                    ((IApp)entity).OwnerId,
                    entity.Id);

                userAppDbSet.Add(userApp);

                // Ensure that the owner has admin priviledges, if not they will be promoted
                var addAdminRole = true;

                var user = await userDbSet
                    .Include(u => u.Roles)
                    .FirstOrDefaultAsync(predicate: u => u.Id == ((IApp)entity).OwnerId);

                foreach (var userRole in user.Roles)
                {
                    userRole.Role = await roleDbSet
                        .FirstOrDefaultAsync(roleDbSet => roleDbSet.Id == userRole.RoleId);

                    if (userRole.Role.RoleLevel == RoleLevel.ADMIN)
                    {
                        addAdminRole = false;
                    }
                }

                // Promote user to admin if user is not already
                if (addAdminRole)
                {
                    var adminRole = await roleDbSet
                        .FirstOrDefaultAsync(predicate: r => r.RoleLevel == RoleLevel.ADMIN);

                    var newUserAdminRole = new UserRole(
                        user.Id,
                        adminRole.Id);

                    userRoleDbSet.Add(newUserAdminRole);
                }

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

                result.Object = entity;
                result.Success = true;

                return result;
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Exception = e;

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
                    query = await dbSet
                        .FirstOrDefaultAsync(predicate: a => a.Id == id);

                    if (query == null)
                    {
                        result.Success = false;
                        result.Object = query;

                        return result;
                    }

                    query.Users = await userAppDbSet
                        .Where(ua => ua.AppId == query.Id)
                        .ToListAsync();

                    foreach (var userApp in query.Users)
                    {
                        userApp.User = await userDbSet
                            .FirstOrDefaultAsync(predicate: u => u.Id == userApp.UserId);
                        userApp.User.Roles = await userRoleDbSet
                            .Where(ur => ur.UserId == userApp.User.Id).ToListAsync();

                        foreach (var role in userApp.User.Roles)
                        {
                            role.Role = await roleDbSet
                                .FirstOrDefaultAsync(predicate: r => r.Id == role.RoleId);

                            role.Role.Users = null;
                        }

                        foreach (var game in userApp.User.Games)
                        {
                            if (await game.IsGameInActiveApp(context))
                            {
                                game.SudokuMatrix = await matrixDbSet
                                    .FirstOrDefaultAsync(predicate: m => m.Id == game.SudokuMatrixId);

                                await game.SudokuMatrix.AttachSudokuCells(context);
                            }
                        }
                    }
                }
                else
                {
                    query = await dbSet
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
            catch (Exception e)
            {
                result.Success = false;
                result.Exception = e;

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
                    query = await dbSet
                        .FirstOrDefaultAsync(
                            predicate: a => a.License.ToLower().Equals(license.ToLower()));

                    if (query == null)
                    {
                        result.Success = false;
                        result.Object = query;

                        return result;
                    }

                    query.Users = await userAppDbSet
                        .Where(ua => ua.AppId == query.Id)
                        .ToListAsync();

                    foreach (var userApp in query.Users)
                    {
                        userApp.User = await userDbSet
                            .FirstOrDefaultAsync(predicate: u => u.Id == userApp.UserId);
                        userApp.User.Roles = await userRoleDbSet
                            .Where(ur => ur.UserId == userApp.User.Id).ToListAsync();

                        foreach (var role in userApp.User.Roles)
                        {
                            role.Role = await roleDbSet
                                .FirstOrDefaultAsync(predicate: r => r.Id == role.RoleId);

                            role.Role.Users = null;
                        }

                        foreach (var game in userApp.User.Games)
                        {
                            if (await game.IsGameInActiveApp(context))
                            {
                                game.SudokuMatrix = await matrixDbSet
                                    .FirstOrDefaultAsync(predicate: m => m.Id == game.SudokuMatrixId);

                                await game.SudokuMatrix.AttachSudokuCells(context);
                            }
                        }
                    }
                }
                else
                {
                    query = await dbSet
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
            catch (Exception e)
            {
                result.Success = false;
                result.Exception = e;

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
                    query = await dbSet
                        .ToListAsync();

                    foreach (var app in query)
                    {
                        app.Users = await userAppDbSet
                            .Where(ua => ua.AppId == app.Id)
                            .ToListAsync();

                        foreach (var userApp in app.Users)
                        {
                            userApp.User = await userDbSet
                                .FirstOrDefaultAsync(predicate: u => u.Id == userApp.UserId);
                            userApp.User.Roles = await userRoleDbSet
                                .Where(ur => ur.UserId == userApp.User.Id).ToListAsync();

                            foreach (var role in userApp.User.Roles)
                            {
                                role.Role = await roleDbSet
                                    .FirstOrDefaultAsync(predicate: r => r.Id == role.RoleId);

                                role.Role.Users = null;
                            }

                            foreach (var game in userApp.User.Games)
                            {
                                if (await game.IsGameInActiveApp(context))
                                {
                                    game.SudokuMatrix = await matrixDbSet
                                        .FirstOrDefaultAsync(predicate: m => m.Id == game.SudokuMatrixId);

                                    await game.SudokuMatrix.AttachSudokuCells(context);
                                }
                            }
                        }
                    }
                }
                else
                {
                    query = await dbSet.ToListAsync();
                }

                foreach (var app in query)
                {
                    if (app.Users != null)
                    {
                        foreach (var userApp in app.Users)
                        {
                            userApp.User.Apps = new List<UserApp>();
                        }
                    }
                }

                result.Success = true;
                result.Objects = query
                    .ConvertAll(a => (IEntityBase)a)
                    .ToList();

                return result;
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Exception = e;

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
                    query = await userDbSet
                        .Include(u => u.Games)
                        .Include(u => u.Roles)
                        .Include(u => u.Apps)
                        .Where(u => u.Apps.Any(ua => ua.AppId == id))
                        .ToListAsync();

                    foreach (var user in query)
                    {
                        foreach (var role in user.Roles)
                        {
                            role.Role = await roleDbSet
                                .FirstOrDefaultAsync(predicate: r => r.Id == role.RoleId);

                            role.Role.Users = null;
                        }

                        user.Apps = new List<UserApp>();

                        foreach (var game in user.Games)
                        {
                            if (await game.IsGameInActiveApp(context))
                            {
                                game.SudokuMatrix = await matrixDbSet
                                    .FirstOrDefaultAsync(predicate: m => m.Id == game.SudokuMatrixId);

                                await game.SudokuMatrix.AttachSudokuCells(context);
                            }
                        }
                    }
                }
                else
                {
                    query = await userDbSet
                        .Where(u => u.Apps.Any(ua => ua.AppId == id))
                        .ToListAsync();

                    foreach (var user in query)
                    {
                        user.Roles = new List<UserRole>();
                        user.Apps = new List<UserApp>();
                        user.Games = new List<Game>();
                    }
                }

                result.Success = true;
                result.Objects = query
                    .ConvertAll(u => (IEntityBase)u)
                    .ToList();


                return result;
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Exception = e;

                return result;
            }
        }

        async public Task<IRepositoryResponse> Update(TEntity entity)
        {
            var result = new RepositoryResponse();

            try
            {
                dbSet.Attach(entity);

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
                result.Object = await dbSet
                        .FirstOrDefaultAsync(predicate: a => a.Id == entity.Id);

                ((App)result.Object).Users = await userAppDbSet
                    .Where(ua => ua.AppId == ((App)result.Object).Id)
                    .ToListAsync();

                foreach (var userApp in ((App)result.Object).Users)
                {
                    userApp.User = await userDbSet
                        .FirstOrDefaultAsync(predicate: u => u.Id == userApp.UserId);
                    userApp.User.Roles = await userRoleDbSet
                        .Where(ur => ur.UserId == userApp.User.Id).ToListAsync();

                    foreach (var role in userApp.User.Roles)
                    {
                        role.Role = await roleDbSet
                            .FirstOrDefaultAsync(predicate: r => r.Id == role.RoleId);

                        role.Role.Users = null;
                    }

                    foreach (var game in userApp.User.Games)
                    {
                        if (await game.IsGameInActiveApp(context))
                        {
                            game.SudokuMatrix = await matrixDbSet
                                .FirstOrDefaultAsync(predicate: m => m.Id == game.SudokuMatrixId);

                            await game.SudokuMatrix.AttachSudokuCells(context);
                        }
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Exception = e;

                return result;
            }
        }

        async public Task<IRepositoryResponse> UpdateRange(List<TEntity> entities)
        {
            var result = new RepositoryResponse();

            try
            {
                dbSet.UpdateRange(entities);
                await context.SaveChangesAsync();

                result.Success = true;

                return result;
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Exception = e;

                return result;
            }
        }

        async public Task<IRepositoryResponse> AddAppUser(int userId, string license)
        {
            var result = new RepositoryResponse();

            try
            {
                var user = await userDbSet
                    .FirstOrDefaultAsync(predicate: u => u.Id == userId);

                var app = await dbSet
                    .FirstOrDefaultAsync(predicate: a => a.License.Equals(license));

                userAppDbSet.Add(new UserApp(user.Id, app.Id));

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
            catch (Exception e)
            {
                result.Success = false;
                result.Exception = e;

                return result;
            }
        }

        async public Task<IRepositoryResponse> RemoveAppUser(int userId, string license)
        {
            var result = new RepositoryResponse();

            try
            {
                var cellList = new List<int>();
                var matrixList = new List<int>();
                var gameList = new List<int>();
                var userAppList = new List<int>();

                var user = await userDbSet
                    .Include(u => u.Games)
                    .FirstOrDefaultAsync(predicate: u => u.Id == userId);

                user.Games = await gameDbSet
                    .Where(g => g.User.Apps.Any(
                        ua => ua.App.License.ToLower().Equals(license.ToLower())))
                    .ToListAsync();

                foreach (var game in user.Games)
                {
                    game.SudokuMatrix = await matrixDbSet
                        .FirstOrDefaultAsync(predicate: m => m.Id == game.SudokuMatrixId);

                    await game.SudokuMatrix.AttachSudokuCells(context);
                }

                var app = await dbSet
                    .FirstOrDefaultAsync(predicate: a => a.License.Equals(license));

                foreach (var game in user.Games)
                {
                    foreach (var cell in game.SudokuMatrix.SudokuCells)
                    {
                        cellDbSet.Remove(cell);
                        cellList.Add(cell.Id);
                    }

                    matrixDbSet.Remove(game.SudokuMatrix);
                    matrixList.Add(game.SudokuMatrixId);
                    gameDbSet.Remove(game);
                    gameList.Add(game.Id);
                }

                var userApp = await userAppDbSet.FirstOrDefaultAsync(predicate: ua => ua.UserId == userId);

                userAppDbSet.Remove(userApp);
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
            catch (Exception e)
            {
                result.Success = false;
                result.Exception = e;

                return result;
            }
        }

        async public Task<IRepositoryResponse> Delete(TEntity entity)
        {
            var result = new RepositoryResponse();

            try
            {
                foreach (var userApp in entity.Users)
                {
                    foreach (var game in userApp.User.Games)
                    {
                        foreach (var cell in game.SudokuMatrix.SudokuCells)
                        {
                            cellDbSet.Remove(cell);
                        }

                        gameDbSet.Remove(game);
                    }

                    userAppDbSet.Remove(userApp);
                }

                dbSet.Remove(entity);
                await context.SaveChangesAsync();

                result.Success = true;

                return result;
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Exception = e;

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
                                cellDbSet.Remove(cell);
                            }

                            gameDbSet.Remove(game);
                        }

                        userAppDbSet.Remove(userApp);
                    }

                    dbSet.Remove(app);
                }

                await context.SaveChangesAsync();

                result.Success = true;

                return result;
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Exception = e;

                return result;
            }
        }

        async public Task<IRepositoryResponse> ResetApp(TEntity entity)
        {
            var result = new RepositoryResponse();

            try
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
                            cellDbSet.Remove(cell);
                        }

                        matrixDbSet.Remove(game.SudokuMatrix);
                    }
                }

                foreach (var ua in entity.Users)
                {
                    foreach (var game in ua.User.Games)
                    {
                        gameDbSet.Remove(game);
                    }
                }

                result.Success = true;

                return result;
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Exception = e;

                return result;
            }
        }

        async public Task<IRepositoryResponse> ActivateApp(int id)
        {
            var result = new RepositoryResponse();

            try
            {
                var app = await dbSet.FindAsync(id);

                if (app != null)
                {
                    if (!app.IsActive)
                    {
                        app.ActivateApp();
                        dbSet.Update(app);
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
            catch (Exception e)
            {
                result.Success = false;
                result.Exception = e;

                return result;
            }
        }

        async public Task<IRepositoryResponse> DeactivateApp(int id)
        {
            var result = new RepositoryResponse();

            try
            {
                var app = await dbSet.FindAsync(id);

                if (app != null)
                {
                    if (app.IsActive)
                    {
                        app.DeactivateApp();
                        dbSet.Update(app);
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
            catch (Exception e)
            {
                result.Success = false;
                result.Exception = e;

                return result;
            }
        }

        async public Task<bool> HasEntity(int id)
        {
            var result = await dbSet.AnyAsync(predicate: a => a.Id == id);

            return result;
        }

        async public Task<bool> IsAppLicenseValid(string license)
        {
            var result = await dbSet.AnyAsync(
                predicate: app => app.License.ToLower().Equals(license.ToLower()));

            return result;
        }

        async public Task<string> GetLicense(int id)
        {
            var license = await dbSet
                .Where(a => a.Id == id)
                .Select(a => a.License)
                .FirstOrDefaultAsync();

            return license;
        }

        async public Task<bool> IsUserRegisteredToApp(int id, string license, int userId)
        {
            var result = await dbSet
                .AnyAsync(predicate:
                    a => a.Users.Any(ua => ua.UserId == userId)
                    && a.Id == id
                    && a.License.ToLower().Equals(license.ToLower()));

            return result;
        }

        async public Task<bool> IsUserOwnerOfApp(int id, string license, int userId)
        {
            var result = await dbSet
                .AnyAsync(predicate:
                    a => a.License.ToLower().Equals(license.ToLower())
                    && a.OwnerId == userId
                    && a.Id == id);

            return result;
        }
        #endregion
    }
}

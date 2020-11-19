using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using SudokuCollective.Core.Interfaces.DataModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Interfaces.Repositories;
using SudokuCollective.Core.Models;
using SudokuCollective.Data.Helpers;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.DataModels;

namespace SudokuCollective.Data.Repositories
{
    public class GamesRepository<TEntity> : IGamesRepository<TEntity> where TEntity : Game
    {
        #region Fields
        private readonly DatabaseContext context;
        private readonly DbSet<Game> dbSet;
        private readonly DbSet<SudokuMatrix> matrixDbSet;
        private readonly DbSet<SudokuCell> cellDbSet;
        private readonly DbSet<SudokuSolution> solutionDbSet;
        private readonly DbSet<App> appDbSet;
        private readonly DbSet<Role> roleDbSet;
        private readonly DbSet<User> userDbSet;
        private readonly DbSet<UserApp> userAppDbSet;
        private readonly DbSet<UserRole> userRoleDbSet;
        #endregion

        #region Constructor
        public GamesRepository(DatabaseContext databaseContext)
        {
            context = databaseContext;
            dbSet = context.Set<Game>();
            matrixDbSet = context.Set<SudokuMatrix>();
            cellDbSet = context.Set<SudokuCell>();
            solutionDbSet = context.Set<SudokuSolution>();
            appDbSet = context.Set<App>();
            roleDbSet = context.Set<Role>();
            userDbSet = context.Set<User>();
            userAppDbSet = context.Set<UserApp>();
            userRoleDbSet = context.Set<UserRole>();
        }
        #endregion

        #region Methods
        async public Task<IRepositoryResponse> Create(TEntity entity)
        {
            var result = new RepositoryResponse();

            try
            {
                if (entity.Id == 0)
                {
                    dbSet.Add(entity);

                    var apps = new List<App>();
                    var roles = new List<Role>();

                    foreach (var userApp in entity.User.Apps)
                    {
                        apps.Add(userApp.App);
                    }

                    foreach (var userRole in entity.User.Roles)
                    {
                        roles.Add(userRole.Role);
                    }

                    context.ChangeTracker.TrackGraph(entity,
                        e => {

                            var dbEntry = (IEntityBase)e.Entry.Entity;

                            if (dbEntry.Id != 0)
                            {
                                e.Entry.State = EntityState.Modified;
                            }
                            else
                            {
                                e.Entry.State = EntityState.Added;
                            }
                        });

                    await context.SaveChangesAsync();

                    foreach (var app in apps)
                    {
                        userAppDbSet.Add(new UserApp(entity.UserId, app.Id));
                    }

                    foreach (var role in roles)
                    {
                        userRoleDbSet.Add(new UserRole(entity.UserId, role.Id));
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

        async public Task<IRepositoryResponse> GetById(int id, bool fullRecord = true)
        {
            var result = new RepositoryResponse();
            var query = new Game();

            try
            {
                if (fullRecord)
                {
                    query = await dbSet
                        .Include(g => g.User)
                            .ThenInclude(u => u.Apps)
                        .Include(g => g.User)
                            .ThenInclude(u => u.Roles)
                        .Include(g => g.SudokuMatrix)
                            .ThenInclude(m => m.SudokuCells)
                        .Include(g => g.SudokuSolution)
                        .FirstOrDefaultAsync(predicate: g => g.Id == id);

                    if (query == null)
                    {
                        result.Success = false;
                        result.Object = query;

                        return result;
                    }

                    foreach (var app in query.User.Apps)
                    {
                        app.App = await appDbSet.FirstOrDefaultAsync(predicate: a => a.Id == app.AppId);
                    }

                    foreach (var role in query.User.Roles)
                    {
                        role.Role = await roleDbSet.FirstOrDefaultAsync(predicate: r => r.Id == role.RoleId);
                    }
                }
                else
                {
                    query = await dbSet
                        .Include(g => g.SudokuMatrix)
                        .FirstOrDefaultAsync(predicate: g => g.Id == id);

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
            var query = new List<Game>();

            try
            {
                if (fullRecord)
                {
                    query = await dbSet
                        .Include(g => g.User)
                            .ThenInclude(u => u.Roles)
                        .Include(g => g.SudokuMatrix)
                        .Include(g => g.SudokuSolution)
                        .ToListAsync();

                    foreach (var game in query)
                    {
                        await game.SudokuMatrix.AttachSudokuCells(context);
                    }
                }
                else
                {
                    query = await dbSet.ToListAsync();
                }

                result.Success = true;
                result.Objects = query
                    .ConvertAll(g => (IEntityBase)g)
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

        async public Task<IRepositoryResponse> Update(TEntity entity)
        {
            var result = new RepositoryResponse();

            try
            {
                if (await dbSet.AnyAsync(predicate: g => g.Id == entity.Id))
                {
                    entity.DateUpdated = DateTime.UtcNow;

                    context.Games.Update(entity);

                    context.ChangeTracker.TrackGraph(entity,
                        e => {

                            var dbEntry = (IEntityBase)e.Entry.Entity;

                            if (dbEntry.Id != 0)
                            {
                                e.Entry.State = EntityState.Modified;
                            }
                            else
                            {
                                e.Entry.State = EntityState.Added;
                            }
                        });

                    var apps = new List<App>();
                    var roles = new List<Role>();

                    foreach (var userApp in entity.User.Apps)
                    {
                        apps.Add(userApp.App);
                    }

                    foreach (var userRole in entity.User.Roles)
                    {
                        roles.Add(userRole.Role);
                    }

                    await context.SaveChangesAsync();

                    foreach (var app in apps)
                    {
                        userAppDbSet.Add(new UserApp(entity.UserId, app.Id));
                    }

                    foreach (var role in roles)
                    {
                        userRoleDbSet.Add(new UserRole(entity.UserId, role.Id));
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

        async public Task<IRepositoryResponse> UpdateRange(List<TEntity> entities)
        {
            var result = new RepositoryResponse();

            try
            {
                foreach (var entity in entities)
                {
                    if (await dbSet.AnyAsync(predicate: g => g.Id == entity.Id))
                    {
                        entity.DateUpdated = DateTime.UtcNow;

                        context.Games.Update(entity);

                        context.ChangeTracker.TrackGraph(entity,
                            e => {

                                var dbEntry = (IEntityBase)e.Entry.Entity;

                                if (dbEntry.Id != 0)
                                {
                                    e.Entry.State = EntityState.Modified;
                                }
                                else
                                {
                                    e.Entry.State = EntityState.Added;
                                }
                            });

                        var apps = new List<App>();
                        var roles = new List<Role>();

                        foreach (var userApp in entity.User.Apps)
                        {
                            apps.Add(userApp.App);
                        }

                        foreach (var userRole in entity.User.Roles)
                        {
                            roles.Add(userRole.Role);
                        }

                        await context.SaveChangesAsync();

                        foreach (var app in apps)
                        {
                            userAppDbSet.Add(new UserApp(entity.UserId, app.Id));
                        }

                        foreach (var role in roles)
                        {
                            userRoleDbSet.Add(new UserRole(entity.UserId, role.Id));
                        }

                        await context.SaveChangesAsync();

                        result.Success = true;
                        result.Object = entity;

                        return result;
                    }
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

        async public Task<IRepositoryResponse> Delete(TEntity entity)
        {
            var result = new RepositoryResponse();

            try
            {
                if (await dbSet.AnyAsync(predicate: g => g.Id == entity.Id))
                {
                    foreach (var cell in entity.SudokuMatrix.SudokuCells)
                    {
                        cellDbSet.Remove(cell);
                    }

                    matrixDbSet.Remove(entity.SudokuMatrix);

                    solutionDbSet.Remove(entity.SudokuSolution);

                    dbSet.Remove(entity);

                    var apps = new List<App>();
                    var roles = new List<Role>();

                    foreach (var userApp in entity.User.Apps)
                    {
                        apps.Add(userApp.App);
                    }

                    foreach (var userRole in entity.User.Roles)
                    {
                        roles.Add(userRole.Role);
                    }

                    await context.SaveChangesAsync();

                    foreach (var app in apps)
                    {
                        userAppDbSet.Add(new UserApp(entity.UserId, app.Id));
                    }

                    foreach (var role in roles)
                    {
                        userRoleDbSet.Add(new UserRole(entity.UserId, role.Id));
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

        async public Task<IRepositoryResponse> DeleteRange(List<TEntity> entities)
        {
            var result = new RepositoryResponse();

            try
            {
                foreach (var entity in entities)
                {
                    if (await dbSet.AnyAsync(predicate: g => g.Id == entity.Id))
                    {
                        foreach (var cell in entity.SudokuMatrix.SudokuCells)
                        {
                            cellDbSet.Remove(cell);
                        }

                        matrixDbSet.Remove(entity.SudokuMatrix);

                        solutionDbSet.Remove(entity.SudokuSolution);

                        dbSet.Remove(entity);

                        var apps = new List<App>();
                        var roles = new List<Role>();

                        foreach (var userApp in entity.User.Apps)
                        {
                            apps.Add(userApp.App);
                        }

                        foreach (var userRole in entity.User.Roles)
                        {
                            roles.Add(userRole.Role);
                        }

                        await context.SaveChangesAsync();

                        foreach (var app in apps)
                        {
                            userAppDbSet.Add(new UserApp(entity.UserId, app.Id));
                        }

                        foreach (var role in roles)
                        {
                            userRoleDbSet.Add(new UserRole(entity.UserId, role.Id));
                        }

                        await context.SaveChangesAsync();

                        result.Success = true;

                        return result;
                    }
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

        async public Task<bool> HasEntity(int id)
        {
            var result = await dbSet.AnyAsync(predicate: g => g.Id == id);

            return result;
        }

        async public Task<IRepositoryResponse> GetGame(int id, int appid, bool fullRecord = true)
        {
            var result = new RepositoryResponse();

            try
            {
                var query = new Game();

                if (fullRecord)
                {
                    query = await dbSet
                        .Include(g => g.User)
                            .ThenInclude(u => u.Roles)
                        .Include(g => g.SudokuMatrix)
                        .Include(g => g.SudokuSolution)
                        .FirstOrDefaultAsync(predicate: g => g.Id == id && g.AppId == appid);

                    await query.SudokuMatrix.AttachSudokuCells(context);
                }
                else
                {
                    query = await dbSet
                        .FirstOrDefaultAsync(predicate: g => g.Id == id && g.AppId == appid);
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

        async public Task<IRepositoryResponse> GetGames(int appid, bool fullRecord = true)
        {
            var result = new RepositoryResponse();

            try
            {
                var query = new List<Game>();

                if (fullRecord)
                {
                    query = await dbSet
                        .Include(g => g.User)
                            .ThenInclude(u => u.Roles)
                        .Include(g => g.SudokuMatrix)
                        .Include(g => g.SudokuSolution)
                        .Where(g => g.AppId == appid)
                        .ToListAsync();

                    foreach (var game in query)
                    {

                        await game.SudokuMatrix.AttachSudokuCells(context);
                    }
                }
                else
                {
                    query = await dbSet
                        .Where(g => g.AppId == appid)
                        .ToListAsync();
                }

                result.Success = true;
                result.Objects = query.ConvertAll(g => (IEntityBase)g);

                return result;
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Exception = exp;

                return result;
            }
        }

        async public Task<IRepositoryResponse> GetMyGame(int userid, int gameid, int appid, bool fullRecord = true)
        {
            var result = new RepositoryResponse();

            try
            {
                var query = new Game();

                if (fullRecord)
                {
                    query = await dbSet
                        .Include(g => g.User)
                            .ThenInclude(u => u.Roles)
                        .Include(g => g.SudokuMatrix)
                            .ThenInclude(m => m.Difficulty)
                        .Include(g => g.SudokuSolution)
                        .FirstOrDefaultAsync(predicate:
                            g => g.Id == gameid
                            && g.AppId == appid
                            && g.UserId == userid);

                    await query.SudokuMatrix.AttachSudokuCells(context);
                }
                else
                {
                    query = await dbSet
                        .FirstOrDefaultAsync(predicate:
                            g => g.Id == gameid
                            && g.AppId == appid
                            && g.UserId == userid);
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

        async public Task<IRepositoryResponse> GetMyGames(int userid, int appid, bool fullRecord = true)
        {
            var result = new RepositoryResponse();

            try
            {
                if (await userDbSet.AnyAsync(predicate: u => u.Id == userid))
                {
                    if (await appDbSet.AnyAsync(predicate: a => a.Id == appid))
                    {
                        var query = new List<Game>();

                        if (fullRecord)
                        {
                            query = await dbSet
                                .Include(g => g.User)
                                    .ThenInclude(u => u.Roles)
                                .Include(g => g.SudokuMatrix)
                                .Include(g => g.SudokuSolution)
                                .Where(g => g.AppId == appid && g.UserId == userid)
                                .ToListAsync();

                            foreach (var game in query)
                            {
                                await game.SudokuMatrix.AttachSudokuCells(context);

                                game.User = null;
                            }
                        }
                        else
                        {
                            query = await dbSet
                                .Where(g => g.AppId == appid && g.UserId == userid)
                                .ToListAsync();
                        }

                        result.Success = true;
                        result.Objects = query.ConvertAll(g => (IEntityBase)g);

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

        async public Task<IRepositoryResponse> DeleteMyGame(int userid, int gameid, int appid)
        {
            var result = new RepositoryResponse();

            try
            {
                if (await userDbSet.AnyAsync(predicate: u => u.Id == userid) && 
                    await dbSet.AnyAsync(predicate: g => g.Id == gameid) &&
                    await appDbSet.AnyAsync(predicate: a => a.Id == appid))
                {
                    var query = new Game();

                    query = await dbSet
                        .Include(g => g.SudokuMatrix)
                        .Include(g => g.SudokuSolution)
                        .Include(g => g.User)
                        .Include(g => g.User)
                            .ThenInclude(u => u.Apps)
                        .Include(g => g.User)
                            .ThenInclude(u => u.Roles)
                        .FirstOrDefaultAsync(predicate:
                            g => g.Id == gameid
                            && g.AppId == appid
                            && g.UserId == userid);

                    await query.SudokuMatrix.AttachSudokuCells(context);

                    foreach (var userApp in query.User.Apps)
                    {
                        userApp.App = await appDbSet
                            .FirstOrDefaultAsync(predicate: a => a.Id == userApp.AppId);
                    }

                    foreach (var userRole in query.User.Roles)
                    {
                        userRole.Role = await roleDbSet
                            .FirstOrDefaultAsync(predicate: r => r.Id == userRole.RoleId);
                    }
                    foreach (var cell in query.SudokuMatrix.SudokuCells)
                    {
                        cellDbSet.Remove(cell);
                    }

                    matrixDbSet.Remove(query.SudokuMatrix);

                    solutionDbSet.Remove(query.SudokuSolution);

                    dbSet.Remove(query);

                    var apps = new List<App>();
                    var roles = new List<Role>();

                    foreach (var userApp in query.User.Apps)
                    {
                        apps.Add(userApp.App);
                    }

                    foreach (var userRole in query.User.Roles)
                    {
                        roles.Add(userRole.Role);
                    }

                    await context.SaveChangesAsync();

                    foreach (var app in apps)
                    {
                        userAppDbSet.Add(new UserApp(query.UserId, app.Id));
                    }

                    foreach (var role in roles)
                    {
                        userRoleDbSet.Add(new UserRole(query.UserId, role.Id));
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
        #endregion
    }
}

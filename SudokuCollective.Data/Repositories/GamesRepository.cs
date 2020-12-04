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
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.DataModels;

namespace SudokuCollective.Data.Repositories
{
    public class GamesRepository<TEntity> : IGamesRepository<TEntity> where TEntity : Game
    {
        #region Fields
        private readonly DatabaseContext context;
        #endregion

        #region Constructor
        public GamesRepository(DatabaseContext databaseContext)
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
                    query = await context
                        .Games
                        .FirstOrDefaultAsync(g => g.Id == id);

                    if (query == null)
                    {
                        result.Success = false;

                        return result;
                    }

                    foreach (var app in query.User.Apps)
                    {
                        app.App = null;
                        app.User = null;
                    }

                    foreach (var role in query.User.Roles)
                    {
                        role.Role.Users = new List<UserRole>();
                        role.User = null;
                    }

                    query.User.Games = new List<Game>();
                }
                else
                {
                    query = await context
                        .Games
                        .FirstOrDefaultAsync(g => g.Id == id);

                    if (query == null)
                    {
                        result.Success = false;

                        return result;
                    }

                    query.SudokuMatrix = null;
                    query.SudokuSolution = null;
                    query.User = null;
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
                    query = await context
                        .Games
                        .ToListAsync();

                    if (query.Count == 0)
                    {
                        result.Success = false;

                        return result;
                    }

                    foreach (var game in query)
                    {
                        foreach (var app in game.User.Apps)
                        {
                            app.App = null;
                            app.User = null;
                        }

                        foreach (var role in game.User.Roles)
                        {
                            role.Role.Users = new List<UserRole>();
                            role.User = null;
                        }

                        game.User.Games = new List<Game>();
                    }
                }
                else
                {
                    query = await context.Games.ToListAsync();

                    if (query.Count == 0)
                    {
                        result.Success = false;

                        return result;
                    }

                    foreach (var game in query)
                    {
                        game.SudokuMatrix = null;
                        game.SudokuSolution = null;
                        game.User = null;
                    }
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
                if (entity.Id == 0)
                {
                    result.Success = false;

                    return result;
                }

                if (await context.Games.AnyAsync(g => g.Id == entity.Id))
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

        async public Task<IRepositoryResponse> UpdateRange(List<TEntity> entities)
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

                    if (await context.Games.AnyAsync(g => g.Id == entity.Id))
                    {
                        context.Attach(entity);
                    }
                    else
                    {
                        result.Success = false;

                        return result;
                    }
                }

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

        async public Task<IRepositoryResponse> Delete(TEntity entity)
        {
            var result = new RepositoryResponse();

            try
            {
                if (await context.Games.AnyAsync(g => g.Id == entity.Id))
                {
                    context.Remove(entity);

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
                    if (entity.Id == 0)
                    {
                        result.Success = false;

                        return result;
                    }

                    if (await context.Games.AnyAsync(g => g.Id == entity.Id))
                    {
                        context.Remove(entity);
                    }
                    else
                    {
                        result.Success = false;

                        return result;
                    }
                }

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
            return await context.Games.AnyAsync(g => g.Id == id);
        }

        async public Task<IRepositoryResponse> GetAppGame(int id, int appid, bool fullRecord = true)
        {
            var result = new RepositoryResponse();
            var query = new Game();

            try
            {
                if (id == 0 || appid == 0)
                {
                    result.Success = false;

                    return result;
                }

                if (fullRecord)
                {
                    query = await context
                        .Games
                        .FirstOrDefaultAsync(g => g.Id == id && g.AppId == appid);

                    if (query == null)
                    {
                        result.Success = false;

                        return result;
                    }

                    foreach (var app in query.User.Apps)
                    {
                        app.App = null;
                        app.User = null;
                    }

                    foreach (var role in query.User.Roles)
                    {
                        role.Role.Users = new List<UserRole>();
                        role.User = null;
                    }

                    query.User.Games = new List<Game>();
                }
                else
                {
                    query = await context
                        .Games
                        .FirstOrDefaultAsync(g => g.Id == id && g.AppId == appid);

                    if (query == null)
                    {
                        result.Success = false;

                        return result;
                    }

                    query.SudokuMatrix = null;
                    query.SudokuSolution = null;
                    query.User = null;
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

        async public Task<IRepositoryResponse> GetAppGames(int appid, bool fullRecord = true)
        {
            var result = new RepositoryResponse();

            try
            {
                if (appid == 0)
                {
                    result.Success = false;

                    return result;
                }

                var query = new List<Game>();

                if (fullRecord)
                {
                    query = await context
                        .Games
                        .Where(g => g.AppId == appid)
                        .ToListAsync();

                    if (query.Count == 0)
                    {
                        result.Success = false;

                        return result;
                    }

                    foreach (var game in query)
                    {
                        foreach (var app in game.User.Apps)
                        {
                            app.App = null;
                            app.User = null;
                        }

                        foreach (var role in game.User.Roles)
                        {
                            role.Role.Users = new List<UserRole>();
                            role.User = null;
                        }

                        game.User.Games = new List<Game>();
                    }
                }
                else
                {
                    query = await context
                        .Games
                        .Where(g => g.AppId == appid)
                        .ToListAsync();

                    if (query.Count == 0)
                    {
                        result.Success = false;

                        return result;
                    }

                    foreach (var game in query)
                    {
                        game.SudokuMatrix = null;
                        game.SudokuSolution = null;
                        game.User = null;
                    }
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
                if (userid == 0 || gameid == 0 || appid == 0)
                {
                    result.Success = false;

                    return result;
                }

                var query = new Game();

                if (fullRecord)
                {
                    query = await context
                        .Games
                        .FirstOrDefaultAsync(predicate:
                            g => g.Id == gameid
                            && g.AppId == appid
                            && g.UserId == userid);

                    if (query == null)
                    {
                        result.Success = false;

                        return result;
                    }

                    foreach (var app in query.User.Apps)
                    {
                        app.App = null;
                        app.User = null;
                    }

                    foreach (var role in query.User.Roles)
                    {
                        role.Role.Users = new List<UserRole>();
                        role.User = null;
                    }

                    query.User.Games = new List<Game>();
                }
                else
                {
                    query = await context
                        .Games
                        .FirstOrDefaultAsync(predicate:
                            g => g.Id == gameid
                            && g.AppId == appid
                            && g.UserId == userid);

                    if (query == null)
                    {
                        result.Success = false;

                        return result;
                    }

                    query.SudokuMatrix = null;
                    query.SudokuSolution = null;
                    query.User = null;
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
                if (userid == 0 || appid == 0)
                {
                    result.Success = false;

                    return result;
                }

                if (await context.Users.AnyAsync(u => u.Id == userid))
                {
                    if (await context.Apps.AnyAsync(a => a.Id == appid))
                    {
                        var query = new List<Game>();

                        if (fullRecord)
                        {
                            query = await context
                                .Games
                                .Where(g => g.AppId == appid && g.UserId == userid)
                                .ToListAsync();

                            if (query.Count == 0)
                            {
                                result.Success = false;

                                return result;
                            }

                            foreach (var game in query)
                            {
                                foreach (var app in game.User.Apps)
                                {
                                    app.App = null;
                                    app.User = null;
                                }

                                foreach (var role in game.User.Roles)
                                {
                                    role.Role.Users = new List<UserRole>();
                                    role.User = null;
                                }

                                game.User.Games = new List<Game>();
                            }
                        }
                        else
                        {
                            query = await context
                                .Games
                                .Where(g => g.AppId == appid && g.UserId == userid)
                                .ToListAsync();

                            if (query.Count == 0)
                            {
                                result.Success = false;

                                return result;
                            }

                            foreach (var game in query)
                            {
                                game.SudokuMatrix = null;
                                game.SudokuSolution = null;
                                game.User = null;
                            }
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
                if (userid == 0 || gameid == 0 || appid == 0)
                {
                    result.Success = false;

                    return result;
                }

                if (await context.Users.AnyAsync(u => u.Id == userid) && 
                    await context.Games.AnyAsync(g => g.Id == gameid) &&
                    await context.Apps.AnyAsync(a => a.Id == appid))
                {
                    var query = new Game();

                    query = await context
                        .Games
                        .FirstOrDefaultAsync(predicate:
                            g => g.Id == gameid
                            && g.AppId == appid
                            && g.UserId == userid);

                    context.Remove(query);

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

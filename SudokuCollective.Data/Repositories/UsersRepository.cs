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
using SudokuCollective.Data.Helpers;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.DataModels;

namespace SudokuCollective.Data.Repositories
{
    public class UsersRepository<TEntity> : IUsersRepository<TEntity> where TEntity : User
    {
        #region Fields
        private readonly DatabaseContext context;
        #endregion

        #region Constructors
        public UsersRepository(DatabaseContext databaseContext)
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

                var role = await context                    
                    .Roles
                    .FirstOrDefaultAsync(
                        predicate: r => r.RoleLevel == RoleLevel.USER);

                var userRoles = new List<UserRole> 
                {
                    new UserRole
                    {
                        UserId = entity.Id,
                        User = entity,
                        RoleId = role.Id,
                        Role = role
                    } 
                };

                if (entity.Apps[0].AppId == 1)
                {
                    var adminRole = await context
                        .Roles
                        .FirstOrDefaultAsync(
                            predicate: r => r.RoleLevel == RoleLevel.ADMIN);

                    userRoles.Add(
                        new UserRole
                        {
                            UserId = entity.Id,
                            User = entity,
                            RoleId = adminRole.Id,
                            Role = adminRole
                        });
                }

                entity.Roles = userRoles;

                context.ChangeTracker.TrackGraph(entity,
                    e =>
                    {

                        var dbEntry = (IEntityBase)e.Entry.Entity;

                        if (dbEntry.Id != 0)
                        {
                            e.Entry.State = EntityState.Added;
                        }
                        else
                        {
                            e.Entry.State = EntityState.Modified;
                        }
                    });

                var emailConfirmation = new EmailConfirmation(
                    entity.Id,
                    entity.Apps[0].App.Id);

                context.Entry(emailConfirmation).State = EntityState.Added;

                await context.SaveChangesAsync();

                result.Object = entity;
                result.Success = true;
                result.Token = emailConfirmation.Token;

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
            var query = new User();

            try
            {
                if (fullRecord)
                {
                    query = await context
                        .Users
                        .Include(u => u.Games)
                        .Include(u => u.Roles)
                            .ThenInclude(ur => ur.Role)
                        .Include(u => u.Apps)
                            .ThenInclude(ua => ua.App)
                        .FirstOrDefaultAsync(predicate: u => u.Id == id);

                    if (query == null)
                    {
                        result.Success = false;
                        result.Object = query;

                        return result;
                    }

                    foreach (var role in query.Roles)
                    {
                        role.Role = await context
                            .Roles
                            .FirstOrDefaultAsync(predicate: r => r.Id == role.RoleId);

                        role.Role.Users = new List<UserRole>();
                    }

                    foreach (var app in query.Apps)
                    {
                        app.App = await context
                            .Apps
                            .FirstOrDefaultAsync(predicate: a => a.Id == app.AppId && a.IsActive);

                        app.App.Users = new List<UserApp>();
                    }

                    foreach (var game in query.Games)
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
                else
                {
                    query = await context
                        .Users
                        .FirstOrDefaultAsync(predicate: u => u.Id == id);
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

        async public Task<IRepositoryResponse> GetByUserName(string username, bool fullRecord = true)
        {
            var result = new RepositoryResponse();
            var query = new User();

            try
            {
                if (fullRecord)
                {
                    query = await context
                        .Users
                        .Include(u => u.Games)
                        .Include(u => u.Roles)
                            .ThenInclude(ur => ur.Role)
                        .Include(u => u.Apps)
                            .ThenInclude(ua => ua.App)
                        .FirstOrDefaultAsync(
                            predicate: u => u.UserName.ToLower().Equals(username.ToLower()));

                    if (query == null)
                    {
                        result.Success = false;
                        result.Object = query;

                        return result;
                    }

                    foreach (var role in query.Roles)
                    {
                        role.Role = await context
                            .Roles
                            .FirstOrDefaultAsync(predicate: r => r.Id == role.RoleId);

                        role.Role.Users = new List<UserRole>();
                    }

                    foreach (var app in query.Apps)
                    {
                        app.App = await context
                            .Apps
                            .FirstOrDefaultAsync(predicate: a => a.Id == app.AppId && a.IsActive);

                        app.App.Users = new List<UserApp>();
                    }

                    foreach (var game in query.Games)
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
                else
                {
                    query = await context
                        .Users
                        .FirstOrDefaultAsync(
                            predicate: u => u.UserName.ToLower().Equals(username.ToLower()));
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

        async public Task<IRepositoryResponse> GetByEmail(string email, bool fullRecord = true)
        {
            var result = new RepositoryResponse();
            var query = new User();

            try
            {
                if (fullRecord)
                {
                    query = await context
                        .Users
                        .Include(u => u.Games)
                        .Include(u => u.Roles)
                            .ThenInclude(ur => ur.Role)
                        .Include(u => u.Apps)
                            .ThenInclude(ua => ua.App)
                        .FirstOrDefaultAsync(
                            predicate: u => u.Email.ToLower().Equals(email.ToLower()));

                    if (query == null)
                    {
                        result.Success = false;
                        result.Object = query;

                        return result;
                    }

                    foreach (var role in query.Roles)
                    {
                        role.Role = await context
                            .Roles
                            .FirstOrDefaultAsync(predicate: r => r.Id == role.RoleId);

                        role.Role.Users = new List<UserRole>();
                    }

                    foreach (var app in query.Apps)
                    {
                        app.App = await context
                            .Apps
                            .FirstOrDefaultAsync(predicate: a => a.Id == app.AppId && a.IsActive);

                        app.App.Users = new List<UserApp>();
                    }

                    foreach (var game in query.Games)
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
                else
                {
                    query = await context
                        .Users
                        .FirstOrDefaultAsync(predicate: u => u.Email.ToLower().Equals(email.ToLower()));
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
            var query = new List<User>();

            try
            {
                if (fullRecord)
                {
                    query = await context
                        .Users
                        .Include(u => u.Games)
                        .Include(u => u.Roles)
                            .ThenInclude(ur => ur.Role)
                        .Include(u => u.Apps)
                            .ThenInclude(ua => ua.App)
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

                        foreach (var app in user.Apps)
                        {
                            app.App = await context
                                .Apps
                                .FirstOrDefaultAsync(predicate: a => a.Id == app.AppId && a.IsActive);

                            app.App.Users = null;
                        }

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
                    query = await context.Users.ToListAsync();
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

        async public Task<IRepositoryResponse> Update(TEntity entity)
        {
            var result = new RepositoryResponse();

            try
            {
                context.Attach(entity);

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
                    .Users
                    .Include(u => u.Games)
                    .Include(u => u.Roles)
                    .Include(u => u.Apps)
                    .FirstOrDefaultAsync(
                        predicate: u => u.Id == entity.Id);

                foreach (var role in ((User)result.Object).Roles)
                {
                    role.Role = await context
                        .Roles
                        .FirstOrDefaultAsync(predicate: r => r.Id == role.RoleId);

                    role.Role.Users = null;
                }

                foreach (var app in ((User)result.Object).Apps)
                {
                    app.App = await context
                        .Apps
                        .FirstOrDefaultAsync(predicate: a => a.Id == app.AppId && a.IsActive);

                    app.App.Users = null;
                }

                foreach (var game in ((User)result.Object).Games)
                {
                    if (await game.IsGameInActiveApp(context))
                    {
                        game.SudokuMatrix = await context
                            .SudokuMatrices
                            .FirstOrDefaultAsync(predicate: m => m.Id == game.SudokuMatrixId);

                        await game.SudokuMatrix.AttachSudokuCells(context);
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
                foreach (var entity in entities)
                {
                    context.Attach(entity);
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

                var entityIds = new List<int>();

                foreach (var entity in entities)
                {
                    entityIds.Add(entity.Id);
                }

                var users = await context
                    .Users
                    .Where(u => entityIds.Contains(u.Id))
                    .Include(u => u.Games)
                    .Include(u => u.Roles)
                    .Include(u => u.Apps)
                    .ToListAsync();

                result.Success = true;
                result.Objects = users.ConvertAll(u => (IEntityBase)u);

                foreach (var role in ((User)result.Object).Roles)
                {
                    role.Role = await context
                        .Roles
                        .FirstOrDefaultAsync(predicate: r => r.Id == role.RoleId);

                    role.Role.Users = null;
                }

                foreach (var app in ((User)result.Object).Apps)
                {
                    app.App = await context
                        .Apps
                        .FirstOrDefaultAsync(predicate: a => a.Id == app.AppId && a.IsActive);

                    app.App.Users = null;
                }

                foreach (var game in ((User)result.Object).Games)
                {
                    if (await game.IsGameInActiveApp(context))
                    {
                        game.SudokuMatrix = await context
                            .SudokuMatrices
                            .FirstOrDefaultAsync(predicate: m => m.Id == game.SudokuMatrixId);

                        await game.SudokuMatrix.AttachSudokuCells(context);
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

        async public Task<IRepositoryResponse> Delete(TEntity entity)
        {
            var result = new RepositoryResponse();

            try
            {
                var cellList = new List<int>();
                var matrixList = new List<int>();
                var gameList = new List<int>();
                var userRoleList = new List<int>();
                var userAppList = new List<int>();

                foreach (var game in entity.Games)
                {
                    await game.SudokuMatrix.AttachSudokuCells(context);

                    foreach (var cell in game.SudokuMatrix.SudokuCells)
                    {
                        cellList.Add(cell.Id);
                        context.SudokuCells.Attach(cell);
                    }

                    matrixList.Add(game.SudokuMatrixId);
                    gameList.Add(game.Id);
                    context.Games.Attach(game);
                }

                foreach (var role in entity.Roles)
                {
                    userRoleList.Add(role.Id);
                    context.UsersRoles.Attach(role);
                }

                foreach (var app in entity.Apps)
                {
                    userAppList.Add(app.Id);
                    context.UsersApps.Attach(app);
                }

                context.Users.Attach(entity);

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
                    else if (entry.Entity is UserRole userRole)
                    {
                        if (userRoleList.Contains(userRole.Id))
                        {
                            entry.State = EntityState.Deleted;
                        }
                        else
                        {
                            entry.State = EntityState.Modified;
                        }
                    }
                    else if (entry.Entity is UserApp userApp)
                    {
                        if (userAppList.Contains(userApp.Id))
                        {
                            entry.State = EntityState.Deleted;
                        }
                        else
                        {
                            entry.State = EntityState.Modified;
                        }
                    }
                    else if (entry.Entity is User user)
                    {
                        if (user.Id == entity.Id)
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

        async public Task<IRepositoryResponse> DeleteRange(List<TEntity> entities)
        {
            var result = new RepositoryResponse();

            try
            {
                foreach (var entity in entities)
                {
                    var cellList = new List<int>();
                    var matrixList = new List<int>();
                    var gameList = new List<int>();
                    var userRoleList = new List<int>();
                    var userAppList = new List<int>();

                    foreach (var game in entity.Games)
                    {
                        await game.SudokuMatrix.AttachSudokuCells(context);

                        foreach (var cell in game.SudokuMatrix.SudokuCells)
                        {
                            cellList.Add(cell.Id);
                            context.SudokuCells.Attach(cell);
                        }

                        matrixList.Add(game.SudokuMatrixId);
                        gameList.Add(game.Id);
                        context.Games.Attach(game);
                    }

                    foreach (var role in entity.Roles)
                    {
                        userRoleList.Add(role.Id);
                        context.UsersRoles.Attach(role);
                    }

                    foreach (var app in entity.Apps)
                    {
                        userAppList.Add(app.Id);
                        context.UsersApps.Attach(app);
                    }

                    context.Users.Attach(entity);

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
                        else if (entry.Entity is UserRole userRole)
                        {
                            if (userRoleList.Contains(userRole.Id))
                            {
                                entry.State = EntityState.Deleted;
                            }
                            else
                            {
                                entry.State = EntityState.Modified;
                            }
                        }
                        else if (entry.Entity is UserApp userApp)
                        {
                            if (userAppList.Contains(userApp.Id))
                            {
                                entry.State = EntityState.Deleted;
                            }
                            else
                            {
                                entry.State = EntityState.Modified;
                            }
                        }
                        else if (entry.Entity is User user)
                        {
                            if (user.Id == entity.Id)
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

        async public Task<bool> HasEntity(int id)
        {
            var result = await context.Users.AnyAsync(predicate: u => u.Id == id);

            return result;
        }

        async public Task<IRepositoryResponse> AddRoles(int userId, List<int> roles)
        {
            var result = new RepositoryResponse();

            try
            {
                if (await context.Users.AnyAsync(u => u.Id == userId))
                {
                    if (await context.Users.AnyAsync(u => u.Id == userId))
                    {
                        foreach (var roleId in roles)
                        {
                            if (await context.Roles.AnyAsync(r => r.Id == roleId))
                            {
                                var userRole = new UserRole(userId, roleId);

                                context.UsersRoles.Add(userRole);
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

        async public Task<IRepositoryResponse> RemoveRoles(int userId, List<int> roles)
        {
            var result = new RepositoryResponse();

            try
            {
                if (await context.Users.AnyAsync(u => u.Id == userId))
                {
                    foreach (var roleId in roles)
                    {
                        if (await context.Roles.AnyAsync(r => r.Id == roleId))
                        {
                            var userRole = await context
                                .UsersRoles
                                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

                            context.UsersRoles.Attach(userRole);
                        }
                    }

                    foreach (var entry in context.ChangeTracker.Entries())
                    {
                        if (entry.Entity is UserRole dbEntry)
                        {
                            if (dbEntry.UserId == userId && roles.Contains(dbEntry.RoleId))
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

        async public Task<IRepositoryResponse> ConfirmEmail(string code)
        {
            var result = new RepositoryResponse();

            try
            {
                if (await context.EmailConfirmations.AnyAsync(ec => ec.Token.ToLower().Equals(code.ToLower())))
                {
                    var emailConfirmed = await context
                        .EmailConfirmations
                        .FirstOrDefaultAsync(predicate: ec => ec.Token.ToLower().Equals(code.ToLower()));

                    if (await context.Users.AnyAsync(u => u.Id == emailConfirmed.UserId))
                    {
                        var user = await context
                            .Users
                            .Include(u => u.Apps)
                            .Include(u => u.Roles)
                            .Include(u => u.Games)
                            .Include(u => u.Games)
                                .ThenInclude(g => g.SudokuMatrix)
                            .Include(u => u.Games)
                                .ThenInclude(g => g.SudokuSolution)
                            .FirstOrDefaultAsync(predicate: u => u.Id == emailConfirmed.UserId);

                        foreach (var userApp in user.Apps)
                        {
                            userApp.App = await context
                                .Apps
                                .FirstOrDefaultAsync(predicate: a => a.Id == userApp.AppId);
                        }

                        foreach (var userRole in user.Roles)
                        {
                            userRole.Role = await context
                                .Roles
                                .FirstOrDefaultAsync(predicate: r => r.Id == userRole.RoleId);
                        }

                        foreach (var game in user.Games)
                        {
                            await game.SudokuMatrix.AttachSudokuCells(context);
                        }

                        user.DateUpdated = DateTime.UtcNow;
                        user.EmailConfirmed = true;

                        context.Attach(user);

                        context.ChangeTracker.TrackGraph(user,
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

                        var emailConfirmation = await context
                            .EmailConfirmations
                            .FirstOrDefaultAsync(ec => ec.Token.ToLower().Equals(code.ToLower()));

                        context.EmailConfirmations.Remove(emailConfirmation);

                        await context.SaveChangesAsync();

                        user.Apps = user
                            .Apps
                            .Where(ua => ua.App.Id == emailConfirmation.AppId)
                            .ToList();

                        result.Success = true;
                        result.Object = user;

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

        async public Task<bool> ActivateUser(int id)
        {
            try
            {
                if (await context.Users.AnyAsync(u => u.Id == id))
                {
                    var user = await context.Users.FirstOrDefaultAsync(predicate: u => u.Id == id);

                    user.ActivateUser();

                    context.Users.Update(user);

                    await context.SaveChangesAsync();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        async public Task<bool> DeactivateUser(int id)
        {
            try
            {
                if (await context.Users.AnyAsync(u => u.Id == id))
                {
                    var user = await context.Users.FirstOrDefaultAsync(predicate: u => u.Id == id);

                    user.DeactiveUser();

                    context.Users.Update(user);

                    await context.SaveChangesAsync();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        async public Task<bool> PromoteUserToAdmin(int id)
        {
            try
            {
                var user = await context
                    .Users
                    .FirstOrDefaultAsync(predicate: u => u.Id == id);

                var role = await context
                    .Roles
                    .FirstOrDefaultAsync(predicate: r => r.RoleLevel == RoleLevel.ADMIN);

                var userRole = new UserRole { 
                    UserId = user.Id, 
                    User = user, 
                    RoleId = role.Id, 
                    Role = role };

                context.UsersRoles.Add(userRole);

                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        async public Task<bool> IsUserRegistered(int id)
        {
            if (id == 0)
            {
                return false;
            }
            else
            {
                return await context.Users.AnyAsync(predicate: u => u.Id == id);
            }
        }

        async public Task<bool> IsUserNameUnique(string username)
        {
            var names = await context.Users.Select(u => u.UserName).ToListAsync();

            var result = true;

            foreach (var name in names)
            {
                if (name.ToLower().Equals(username.ToLower()))
                {
                    result = false;
                }
            }

            return result;
        }

        async public Task<bool> IsEmailUnique(string email)
        {
            var emails = await context.Users.Select(u => u.Email).ToListAsync();

            var result = true;

            foreach (var e in emails)
            {
                if (e.ToLower().Equals(email.ToLower()))
                {
                    result = false;
                }
            }

            return result;
        }
        #endregion
    }
}

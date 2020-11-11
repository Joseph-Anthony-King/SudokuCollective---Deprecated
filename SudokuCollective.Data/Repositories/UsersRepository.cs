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
        private readonly DbSet<User> dbSet;
        private readonly DbSet<UserRole> userRoleDbSet;
        private readonly DbSet<Role> roleDbSet;
        private readonly DbSet<UserApp> userAppDbSet;
        private readonly DbSet<App> appDbSet;
        private readonly DbSet<Game> gameDbSet;
        private readonly DbSet<SudokuMatrix> matrixDbSet;
        private readonly DbSet<SudokuCell> cellDbSet;
        #endregion

        #region Constructors
        public UsersRepository(DatabaseContext databaseContext)
        {
            context = databaseContext;
            dbSet = context.Set<User>();
            userRoleDbSet = context.Set<UserRole>();
            roleDbSet = context.Set<Role>();
            userAppDbSet = context.Set<UserApp>();
            appDbSet = context.Set<App>(); 
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

                var role = await roleDbSet
                    .FirstOrDefaultAsync(
                        predicate: r => r.RoleLevel == RoleLevel.USER);

                var userRoles = new List<UserRole>();

                userRoles.Add(
                    new UserRole
                    {
                        UserId = entity.Id,
                        User = entity,
                        RoleId = role.Id,
                        Role = role
                    });

                if (entity.Apps[0].AppId == 1)
                {
                    var adminRole = await roleDbSet
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

                entity.Roles.AddRange(userRoles);

                userRoleDbSet.AddRange(userRoles);

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

                entity.Apps = new List<UserApp>();

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
            var query = new User();

            try
            {
                if (fullRecord)
                {
                    query = await dbSet
                        .Include(u => u.Games)
                        .Include(u => u.Roles)
                            .ThenInclude(ur => ur.Role)
                        .Include(u => u.Apps)
                            .ThenInclude(ua => ua.App)
                        .FirstOrDefaultAsync(predicate: u => u.Id == id);

                    foreach (var role in query.Roles)
                    {
                        role.Role = await roleDbSet
                            .FirstOrDefaultAsync(predicate: r => r.Id == role.RoleId);

                        role.Role.Users = new List<UserRole>();
                    }

                    foreach (var app in query.Apps)
                    {
                        app.App = await appDbSet
                            .FirstOrDefaultAsync(predicate: a => a.Id == app.AppId && a.IsActive);

                        app.App.Users = new List<UserApp>();
                    }

                    foreach (var game in query.Games)
                    {
                        if (await game.IsGameInActiveApp(context))
                        {
                            game.SudokuMatrix = await matrixDbSet
                                .FirstOrDefaultAsync(predicate: m => m.Id == game.SudokuMatrixId);

                            await game.SudokuMatrix.AttachSudokuCells(context);
                        }
                    }
                }
                else
                {
                    query = await dbSet
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
                    query = await dbSet
                        .Include(u => u.Games)
                        .Include(u => u.Roles)
                            .ThenInclude(ur => ur.Role)
                        .Include(u => u.Apps)
                            .ThenInclude(ua => ua.App)
                        .FirstOrDefaultAsync(
                            predicate: u => u.UserName.ToLower().Equals(username.ToLower()));

                    foreach (var role in query.Roles)
                    {
                        role.Role = await roleDbSet
                            .FirstOrDefaultAsync(predicate: r => r.Id == role.RoleId);

                        role.Role.Users = new List<UserRole>();
                    }

                    foreach (var app in query.Apps)
                    {
                        app.App = await appDbSet
                            .FirstOrDefaultAsync(predicate: a => a.Id == app.AppId && a.IsActive);

                        app.App.Users = new List<UserApp>();
                    }

                    foreach (var game in query.Games)
                    {
                        if (await game.IsGameInActiveApp(context))
                        {
                            game.SudokuMatrix = await matrixDbSet
                                .FirstOrDefaultAsync(predicate: m => m.Id == game.SudokuMatrixId);

                            await game.SudokuMatrix.AttachSudokuCells(context);
                        }
                    }
                }
                else
                {
                    query = await dbSet
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
                    query = await dbSet
                        .Include(u => u.Games)
                        .Include(u => u.Roles)
                            .ThenInclude(ur => ur.Role)
                        .Include(u => u.Apps)
                            .ThenInclude(ua => ua.App)
                        .FirstOrDefaultAsync(
                            predicate: u => u.Email.ToLower().Equals(email.ToLower()));

                    foreach (var role in query.Roles)
                    {
                        role.Role = await roleDbSet
                            .FirstOrDefaultAsync(predicate: r => r.Id == role.RoleId);

                        role.Role.Users = new List<UserRole>();
                    }

                    foreach (var app in query.Apps)
                    {
                        app.App = await appDbSet
                            .FirstOrDefaultAsync(predicate: a => a.Id == app.AppId && a.IsActive);

                        app.App.Users = new List<UserApp>();
                    }

                    foreach (var game in query.Games)
                    {
                        if (await game.IsGameInActiveApp(context))
                        {
                            game.SudokuMatrix = await matrixDbSet
                                .FirstOrDefaultAsync(predicate: m => m.Id == game.SudokuMatrixId);

                            await game.SudokuMatrix.AttachSudokuCells(context);
                        }
                    }
                }
                else
                {
                    query = await dbSet
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
                    query = await dbSet
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
                            role.Role = await roleDbSet
                                .FirstOrDefaultAsync(predicate: r => r.Id == role.RoleId);

                            role.Role.Users = null;
                        }

                        foreach (var app in user.Apps)
                        {
                            app.App = await appDbSet
                                .FirstOrDefaultAsync(predicate: a => a.Id == app.AppId && a.IsActive);

                            app.App.Users = null;
                        }

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
                    query = await dbSet.ToListAsync();
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
                    .Include(u => u.Games)
                    .Include(u => u.Roles)
                    .Include(u => u.Apps)
                    .FirstOrDefaultAsync(
                        predicate: u => u.Id == entity.Id);

                foreach (var role in ((User)result.Object).Roles)
                {
                    role.Role = await roleDbSet
                        .FirstOrDefaultAsync(predicate: r => r.Id == role.RoleId);

                    role.Role.Users = null;
                }

                foreach (var app in ((User)result.Object).Apps)
                {
                    app.App = await appDbSet
                        .FirstOrDefaultAsync(predicate: a => a.Id == app.AppId && a.IsActive);

                    app.App.Users = null;
                }

                foreach (var game in ((User)result.Object).Games)
                {
                    if (await game.IsGameInActiveApp(context))
                    {
                        game.SudokuMatrix = await matrixDbSet
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
                    dbSet.Attach(entity);
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

                var users = await dbSet
                    .Where(u => entityIds.Contains(u.Id))
                    .Include(u => u.Games)
                    .Include(u => u.Roles)
                    .Include(u => u.Apps)
                    .ToListAsync();

                result.Success = true;
                result.Objects = users.ConvertAll(u => (IEntityBase)u);

                foreach (var role in ((User)result.Object).Roles)
                {
                    role.Role = await roleDbSet
                        .FirstOrDefaultAsync(predicate: r => r.Id == role.RoleId);

                    role.Role.Users = null;
                }

                foreach (var app in ((User)result.Object).Apps)
                {
                    app.App = await appDbSet
                        .FirstOrDefaultAsync(predicate: a => a.Id == app.AppId && a.IsActive);

                    app.App.Users = null;
                }

                foreach (var game in ((User)result.Object).Games)
                {
                    if (await game.IsGameInActiveApp(context))
                    {
                        game.SudokuMatrix = await matrixDbSet
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
                        cellDbSet.Attach(cell);
                    }

                    matrixList.Add(game.SudokuMatrixId);
                    gameList.Add(game.Id);
                    gameDbSet.Attach(game);
                }

                foreach (var role in entity.Roles)
                {
                    userRoleList.Add(role.Id);
                    userRoleDbSet.Attach(role);
                }

                foreach (var app in entity.Apps)
                {
                    userAppList.Add(app.Id);
                    userAppDbSet.Attach(app);
                }

                dbSet.Attach(entity);

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
                            cellDbSet.Attach(cell);
                        }

                        matrixList.Add(game.SudokuMatrixId);
                        gameList.Add(game.Id);
                        gameDbSet.Attach(game);
                    }

                    foreach (var role in entity.Roles)
                    {
                        userRoleList.Add(role.Id);
                        userRoleDbSet.Attach(role);
                    }

                    foreach (var app in entity.Apps)
                    {
                        userAppList.Add(app.Id);
                        userAppDbSet.Attach(app);
                    }

                    dbSet.Attach(entity);

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
            var result = await dbSet.AnyAsync(predicate: u => u.Id == id);

            return result;
        }

        async public Task<IRepositoryResponse> AddRoles(int userId, List<int> roles)
        {
            var result = new RepositoryResponse();

            try
            {
                foreach (var roleId in roles)
                {
                    var userRole = new UserRole(userId, roleId);

                    userRoleDbSet.Add(userRole);
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

        async public Task<IRepositoryResponse> RemoveRoles(int userid, List<int> roles)
        {
            var result = new RepositoryResponse();

            try
            {
                var userRoles = await userRoleDbSet
                    .Where(ur => ur.UserId == userid && roles.Contains(ur.RoleId))
                    .ToListAsync();

                userRoleDbSet.AttachRange(userRoles);

                foreach (var entry in context.ChangeTracker.Entries())
                {
                    if (entry.Entity is UserRole dbEntry)
                    {
                        if (dbEntry.UserId == userid && roles.Contains(dbEntry.RoleId))
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

        async public Task<bool> ActivateUser(int id)
        {
            try
            {
                var user = await dbSet.FirstOrDefaultAsync(predicate: u => u.Id == id);

                user.ActivateUser();

                dbSet.Update(user);

                await context.SaveChangesAsync();

                return true;
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
                var user = await dbSet.FirstOrDefaultAsync(predicate: u => u.Id == id);

                user.DeactiveUser();

                dbSet.Update(user);

                await context.SaveChangesAsync();

                return true;
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
                var user = await dbSet.FirstOrDefaultAsync(predicate: u => u.Id == id);
                var role = await roleDbSet.FirstOrDefaultAsync(predicate: r => r.RoleLevel == RoleLevel.ADMIN);

                var userRole = new UserRole { 
                    UserId = user.Id, 
                    User = user, 
                    RoleId = role.Id, 
                    Role = role };

                userRoleDbSet.Add(userRole);

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
            return await dbSet.AnyAsync(predicate: u => u.Id == id);
        }

        async public Task<bool> IsUserNameUnique(string username)
        {
            return await dbSet.AnyAsync(
                predicate: u => !u.UserName.ToLower().Equals(username.ToLower()));
        }

        async public Task<bool> IsEmailUnique(string email)
        {
            return await dbSet.AnyAsync(
                predicate: u => !u.Email.ToLower().Equals(email.ToLower()));
        }
        #endregion
    }
}

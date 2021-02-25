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

                var role = await context                    
                    .Roles
                    .FirstOrDefaultAsync(r => r.RoleLevel == RoleLevel.USER);

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

                if (entity.Apps.FirstOrDefault().AppId == 1)
                {
                    var adminRole = await context
                        .Roles
                        .FirstOrDefaultAsync(r => r.RoleLevel == RoleLevel.ADMIN);

                    userRoles.Add(
                        new UserRole
                        {
                            UserId = entity.Id,
                            User = entity,
                            RoleId = adminRole.Id,
                            Role = adminRole
                        });
                }

                entity.Roles = userRoles
                    .OrderBy(ur => ur.RoleId)
                    .ToList();

                context.AddRange(entity.Roles);

                foreach (var entry in context.ChangeTracker.Entries())
                {
                    var dbEntry = (IEntityBase)entry.Entity;

                    if (dbEntry is UserApp userApp)
                    {
                        if (entity.Apps.Any(ua => ua.Id == userApp.Id))
                        {
                            entry.State = EntityState.Added;
                        }
                        else
                        {
                            entry.State = EntityState.Modified;
                        }
                    }
                    else if (dbEntry is UserRole userRole)
                    {
                        if (userRole.Role == null)
                        {
                            userRole.Role = await context
                                .Roles
                                .FirstOrDefaultAsync(r => r.Id == userRole.RoleId);
                        }

                        if (entity.Roles.Any(ur => ur.Id == userRole.Id))
                        {
                            entry.State = EntityState.Added;
                        }
                        else
                        {
                            entry.State = EntityState.Modified;
                        }
                    }
                    else if (dbEntry is AppAdmin userAdmin)
                    {
                        if (userAdmin.AppId == entity.Apps.FirstOrDefault().AppId 
                            && userAdmin.UserId == entity.Id)
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

        public async Task<IRepositoryResponse> GetById(int id, bool fullRecord = true)
        {
            var result = new RepositoryResponse();
            var query = new User();

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
                        .FirstOrDefaultAsync(u => u.Id == id);
                }
                else
                {
                    query = await context
                        .Users
                        .FirstOrDefaultAsync(u => u.Id == id);
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

        public async Task<IRepositoryResponse> GetByUserName(string username, bool fullRecord = true)
        {
            var result = new RepositoryResponse();
            var query = new User();

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
                        .FirstOrDefaultAsync(
                            u => u.UserName.ToLower().Equals(username.ToLower()));
                }
                else
                {
                    query = await context
                        .Users
                        .FirstOrDefaultAsync(
                            u => u.UserName.ToLower().Equals(username.ToLower()));
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

        public async Task<IRepositoryResponse> GetByEmail(string email, bool fullRecord = true)
        {
            var result = new RepositoryResponse();
            var query = new User();

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
                        .FirstOrDefaultAsync(
                            u => u.Email.ToLower().Equals(email.ToLower()));
                }
                else
                {
                    query = await context
                        .Users
                        .FirstOrDefaultAsync(u => u.Email.ToLower().Equals(email.ToLower()));
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
                        .OrderBy(u => u.Id)
                        .ToListAsync();
                }
                else
                {
                    query = await context
                        .Users
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

                if (await context.Users.AnyAsync(u => u.Id == entity.Id))
                {
                    entity.DateUpdated = DateTime.UtcNow;

                    context.Attach(entity);

                    foreach (var entry in context.ChangeTracker.Entries())
                    {
                        var dbEntry = (IEntityBase)entry.Entity;

                        if (dbEntry is UserApp)
                        {
                            if (dbEntry.Id == 0)
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
                            if (dbEntry.Id == 0)
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

            var dateUpdated = DateTime.UtcNow;

            try
            {
                foreach (var entity in entities)
                {
                    if (entity.Id == 0)
                    {
                        result.Success = false;

                        return result;
                    }

                    if (await context.Users.AnyAsync(u => u.Id == entity.Id))
                    {
                        entity.DateUpdated = dateUpdated;

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
                result.Objects = entities.ConvertAll(a => (IEntityBase)a);

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
                if (entity.Id == 0)
                {
                    result.Success = false;

                    return result;
                }

                if (await context.Users.AnyAsync(u => u.Id == entity.Id))
                {
                    context.Remove(entity);

                    var games = await context
                        .Games
                        .Where(g => g.UserId == entity.Id)
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
                        else if (dbEntry is UserRole userRole)
                        {
                            if (userRole.UserId == entity.Id)
                            {
                                entry.State = EntityState.Deleted;
                            }
                            else
                            {
                                entry.State = EntityState.Modified;
                            }
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

                    if (await context.Users.AnyAsync(u => u.Id == entity.Id))
                    {
                        context.Remove(entity);

                        var games = await context
                            .Games
                            .Where(g => g.UserId == entity.Id)
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
                        else if (dbEntry is UserRole userRole)
                        {
                            if (userRole.UserId == entity.Id)
                            {
                                entry.State = EntityState.Deleted;
                            }
                            else
                            {
                                entry.State = EntityState.Modified;
                            }
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

        public async Task<bool> HasEntity(int id)
        {
            return await context.Users.AnyAsync(u => u.Id == id);
        }

        public async Task<IRepositoryResponse> AddRole(int userId, int roleId)
        {
            var result = new RepositoryResponse();

            try
            {
                if (await context.Users.AnyAsync(u => u.Id == userId) && await context.Roles.AnyAsync(r => r.Id == roleId))
                {
                    if (await context.Users.AnyAsync(u => u.Id == userId && !u.Roles.Any(ur => ur.RoleId == roleId)))
                    {
                        var user = await context
                            .Users
                            .FirstOrDefaultAsync(u => u.Id == userId);

                        var role = await context
                            .Roles
                            .FirstOrDefaultAsync(r => r.Id == roleId);

                        var userRole = new UserRole
                        {
                            User = user,
                            UserId = user.Id,
                            Role = role,
                            RoleId = role.Id
                        };

                        context.Attach(userRole);

                        foreach (var entry in context.ChangeTracker.Entries())
                        {
                            var dbEntry = (IEntityBase)entry.Entity;

                            if (dbEntry is UserRole ur)
                            {
                                if (ur.Id == userRole.Id)
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
                                entry.State = EntityState.Modified;
                            }
                        }

                        await context.SaveChangesAsync();

                        result.Success = true;
                        result.Object = userRole;

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

        public async Task<IRepositoryResponse> AddRoles(int userId, List<int> roleIds)
        {
            var result = new RepositoryResponse();

            try
            {
                if (await context.Users.AnyAsync(u => u.Id == userId))
                {
                    var user = await context
                        .Users
                        .FirstOrDefaultAsync(u => u.Id == userId);

                    var newUserRoleIds = new List<int>();

                    foreach (var roleId in roleIds)
                    {
                        if (await context.Roles.AnyAsync(r => r.Id == roleId))
                        {
                            var role = await context.Roles.FirstOrDefaultAsync(r => r.Id == roleId);

                            var userRole = new UserRole
                            {
                                UserId = user.Id,
                                User = user,
                                RoleId = role.Id,
                                Role = role
                            };

                            context.Attach(userRole);

                            result.Objects.Add(userRole);

                            newUserRoleIds.Add(userRole.Id);
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

                        if (dbEntry is UserRole ur)
                        {
                            if (newUserRoleIds.Contains(ur.Id))
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

        public async Task<IRepositoryResponse> RemoveRole(int userId, int roleId)
        {
            var result = new RepositoryResponse();

            try
            {
                if (await context.Users
                    .AnyAsync(u => u.Id == userId && u.Roles.Any(ur => ur.RoleId == roleId)))
                {
                    var userRole = await context
                        .UsersRoles
                        .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

                    foreach (var entry in context.ChangeTracker.Entries())
                    {
                        var dbEntry = (IEntityBase)entry.Entity;

                        if (dbEntry is UserRole ur)
                        {
                            if (ur.Id == userRole.Id)
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

        public async Task<IRepositoryResponse> RemoveRoles(int userId, List<int> roleIds)
        {
            var result = new RepositoryResponse();

            try
            {
                if (await context.Users.AnyAsync(u => u.Id == userId))
                {
                    foreach (var roleId in roleIds)
                    {
                        if (await context
                            .UsersRoles
                            .AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId))
                        {
                            // Role exists so we continue...
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

                        if (dbEntry is UserRole userRole)
                        {
                            if (userRole.UserId == userId 
                                && roleIds.Contains(userRole.RoleId))
                            {
                                entry.State = EntityState.Deleted;

                                result.Objects.Add(userRole);
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

        public async Task<IRepositoryResponse> ConfirmEmail(IEmailConfirmation emailConfirmation)
        {
            var result = new RepositoryResponse();

            try
            {
                if (await context
                    .EmailConfirmations
                    .AnyAsync(ec => ec.Id == emailConfirmation.Id))
                {
                    if (await context.Users.AnyAsync(u => u.Id == emailConfirmation.UserId))
                    {
                        var user = await context
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
                            .FirstOrDefaultAsync(u => u.Id == emailConfirmation.UserId);

                        user.DateUpdated = DateTime.UtcNow;
                        user.EmailConfirmed = true;

                        context.Attach(user);

                        foreach (var entry in context.ChangeTracker.Entries())
                        {
                            var dbEntry = (IEntityBase)entry.Entity;

                            if (dbEntry is UserApp)
                            {
                                entry.State = EntityState.Modified;
                            }

                            if (dbEntry is UserRole)
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

        public async Task<IRepositoryResponse> UpdateUserEmail(IEmailConfirmation emailConfirmation)
        {
            var result = new RepositoryResponse();

            try
            {
                if (await context
                    .EmailConfirmations
                    .AnyAsync(ec => ec.Id == emailConfirmation.Id))
                {
                    if (await context.Users.AnyAsync(u => u.Id == emailConfirmation.UserId))
                    {
                        var user = await context
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
                            .FirstOrDefaultAsync(u => u.Id == emailConfirmation.UserId);

                        user.Email = emailConfirmation.NewEmailAddress;
                        user.ReceivedRequestToUpdateEmail = false;
                        user.DateUpdated = DateTime.UtcNow;

                        context.Attach(user);

                        foreach (var entry in context.ChangeTracker.Entries())
                        {
                            var dbEntry = (IEntityBase)entry.Entity;

                            if (dbEntry is UserApp)
                            {
                                entry.State = EntityState.Modified;
                            }

                            if (dbEntry is UserRole)
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

        public async Task<bool> ActivateUser(int id)
        {
            try
            {
                if (await context.Users.AnyAsync(u => u.Id == id))
                {
                    var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);

                    user.ActivateUser();

                    context.Attach(user);

                    foreach (var entry in context.ChangeTracker.Entries())
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

        public async Task<bool> DeactivateUser(int id)
        {
            try
            {
                if (await context.Users.AnyAsync(u => u.Id == id))
                {
                    var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);

                    user.DeactiveUser();

                    context.Attach(user);

                    foreach (var entry in context.ChangeTracker.Entries())
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

        public async Task<bool> PromoteUserToAdmin(int id)
        {
            try
            {
                var user = await context
                    .Users
                    .FirstOrDefaultAsync(u => u.Id == id);

                var role = await context
                    .Roles
                    .FirstOrDefaultAsync(r => r.RoleLevel == RoleLevel.ADMIN);

                var userRole = new UserRole { 
                    UserId = user.Id, 
                    User = user, 
                    RoleId = role.Id, 
                    Role = role };

                context.Attach(userRole);

                foreach (var entry in context.ChangeTracker.Entries())
                {
                    var dbEntry = (IEntityBase)entry.Entity;

                    if (dbEntry is UserRole ur)
                    {
                        if (ur.Id == userRole.Id)
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
                        entry.State = EntityState.Modified;
                    }
                }
                
                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> IsUserRegistered(int id)
        {
            if (id == 0)
            {
                return false;
            }
            else
            {
                return await context.Users.AnyAsync(u => u.Id == id);
            }
        }

        public async Task<bool> IsUserNameUnique(string username)
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

        public async Task<bool> IsEmailUnique(string email)
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

       public async Task<bool> IsUpdatedUserNameUnique(int userId, string username)
        {
            var names = await context
                .Users
                .Where(u => u.Id != userId)
                .Select(u => u.UserName)
                .ToListAsync();

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

        public async Task<bool> IsUpdatedEmailUnique(int userId, string email)
        {
            var emails = await context
                .Users
                .Where(u => u.Id != userId)
                .Select(u => u.Email)
                .ToListAsync();

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

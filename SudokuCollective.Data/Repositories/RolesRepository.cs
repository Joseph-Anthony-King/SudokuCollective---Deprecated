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
    public class RolesRepository<TEntity> : IRolesRepository<TEntity> where TEntity : Role
    {
        #region Fields
        private readonly DatabaseContext context;
        #endregion

        #region Constructor
        public RolesRepository(DatabaseContext databaseContext)
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
                if (await context.Roles.AnyAsync(r => r.RoleLevel == entity.RoleLevel))
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

        public async Task<IRepositoryResponse> GetById(int id, bool fullRecord = true)
        {
            var result = new RepositoryResponse();
            var query = new Role();

            try
            {
                if (fullRecord)
                {
                    query = await context
                        .Roles
                        .Include(r => r.Users)
                            .ThenInclude(ur => ur.User)
                        .FirstOrDefaultAsync(r => r.Id == id);
                }
                else
                {
                    query = await context
                        .Roles
                        .FirstOrDefaultAsync(r => r.Id == id);
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
            var query = new List<Role>();

            try
            {
                if (fullRecord)
                {
                    query = await context
                        .Roles
                        .Include(r => r.Users)
                            .ThenInclude(ur => ur.User)
                        .Where(r =>
                            r.RoleLevel != RoleLevel.NULL)
                        .OrderBy(r => r.RoleLevel)
                        .ToListAsync();
                }
                else
                {
                    query = await context
                        .Roles
                        .Where(r =>
                            r.RoleLevel != RoleLevel.NULL)
                        .OrderBy(r => r.RoleLevel)
                        .ToListAsync();
                }

                if (query.Count == 0)
                {
                    result.Success = false;

                    return result;
                }

                result.Success = true;
                result.Objects = query
                    .ConvertAll(r => (IEntityBase)r)
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

                if (await context.Roles.AnyAsync(r => r.Id == entity.Id))
                {
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
                foreach (var entity in entities)
                {
                    if (entity.Id == 0)
                    {
                        result.Success = false;

                        return result;
                    }

                    if (await context.Roles.AnyAsync(d => d.Id == entity.Id))
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

        public async Task<IRepositoryResponse> Delete(TEntity entity)
        {
            var result = new RepositoryResponse();

            try
            {
                if (await context.Roles.AnyAsync(d => d.Id == entity.Id))
                {
                    context.Remove(entity);

                    foreach (var entry in context.ChangeTracker.Entries())
                    {
                        var dbEntry = (IEntityBase)entry.Entity;

                        if (dbEntry is UserApp)
                        {
                            entry.State = EntityState.Modified;
                        }
                        else if (dbEntry is UserRole userRole)
                        {
                            if (userRole.RoleId == entity.Id)
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

        public async Task<IRepositoryResponse> DeleteRange(List<TEntity> entities)
        {
            var result = new RepositoryResponse();

            try
            {
                var roleIds = new List<int>();

                foreach (var entity in entities)
                {
                    if (entity.Id == 0)
                    {
                        result.Success = false;

                        return result;
                    }

                    if (await context.Roles.AnyAsync(d => d.Id == entity.Id))
                    {
                        context.Remove(entity);
                        roleIds.Add(entity.Id);
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
                    else if (dbEntry is UserRole userRole)
                    {
                        if (roleIds.Contains(userRole.RoleId))
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

        public async Task<bool> HasEntity(int id)
        {
            var result = await context.Roles.AnyAsync(r => r.Id == id);

            return result;
        }

        public async Task<bool> HasRoleLevel(RoleLevel level)
        {
            var result = await context.Roles.AnyAsync(r => r.RoleLevel == level);

            return result;
        }

        public async Task<bool> IsListValid(List<int> ids)
        {
            var result = true;

            foreach (var id in ids)
            {
                var isIdValid = await context.Roles.AnyAsync(r => r.Id == id);

                if (!isIdValid)
                {
                    result = false;
                }
            }

            return result;
        }
        #endregion
    }
}

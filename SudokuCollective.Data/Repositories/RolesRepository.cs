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
        async public Task<IRepositoryResponse> Create(TEntity entity)
        {
            var result = new RepositoryResponse();

            try
            {
                if (await context.Roles.AnyAsync(r => r.RoleLevel == entity.RoleLevel))
                {
                    result.Success = false;

                    return result;
                }

                context.Roles.Add(entity);

                foreach (var entry in context.ChangeTracker.Entries())
                {
                    if (entry.Entity is Role role)
                    {
                        if (entity.Id == role.Id)
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
            var query = new Role();

            try
            {
                if (fullRecord)
                {
                    query = await context
                        .Roles
                        .Include(r => r.Users)
                            .ThenInclude(ua => ua.User)
                        .FirstOrDefaultAsync(predicate: r => r.Id == id);
                }
                else
                {
                    query = await context
                        .Roles
                        .FirstOrDefaultAsync(predicate: r => r.Id == id);
                }

                if (query == null)
                {
                    result.Success = false;
                    result.Object = query;

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

        async public Task<IRepositoryResponse> GetAll(bool fullRecord = true)
        {
            var result = new RepositoryResponse();
            var query = new List<Role>();

            try
            {
                if (fullRecord)
                {
                    query = await context
                        .Roles
                        .Where(r =>
                            r.RoleLevel != RoleLevel.NULL)
                        .Include(r => r.Users)
                            .ThenInclude(ua => ua.User)
                        .ToListAsync();
                }
                else
                {
                    query = await context
                        .Roles
                        .Where(r =>
                            r.RoleLevel != RoleLevel.NULL)
                        .ToListAsync();
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

        async public Task<IRepositoryResponse> Update(TEntity entity)
        {
            var result = new RepositoryResponse();

            try
            {
                context.Roles.Update(entity);

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

        async public Task<IRepositoryResponse> UpdateRange(List<TEntity> entities)
        {
            var result = new RepositoryResponse();

            try
            {
                context.Roles.UpdateRange(entities);

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

        async public Task<IRepositoryResponse> Delete(TEntity entity)
        {
            var result = new RepositoryResponse();
            try
            {
                context.Roles.Remove(entity);

                foreach (var entry in context.ChangeTracker.Entries())
                {
                    if (entry.Entity is Difficulty difficulty)
                    {
                        if (entity.Id == difficulty.Id)
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
                context.Roles.RemoveRange(entities);

                var rolesForDeletion = new List<int>();

                foreach (var entity in entities)
                {
                    rolesForDeletion.Add(entity.Id);
                }

                foreach (var entry in context.ChangeTracker.Entries())
                {
                    if (entry.Entity is Role role)
                    {
                        if (rolesForDeletion.Contains(role.Id))
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

        async public Task<bool> HasEntity(int id)
        {
            var result = await context.Roles.AnyAsync(predicate: r => r.Id == id);

            return result;
        }

        async public Task<bool> HasRoleLevel(RoleLevel level)
        {
            var result = await context.Roles.AnyAsync(predicate: r => r.RoleLevel == level);

            return result;
        }

        async public Task<bool> IsListValid(List<int> ids)
        {
            var result = true;

            foreach (var id in ids)
            {
                var isIdValid = await context.Roles.AnyAsync(predicate: r => r.Id == id);

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

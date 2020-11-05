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
        private readonly DbSet<Role> dbSet;
        #endregion

        #region Constructor
        public RolesRepository(DatabaseContext databaseContext)
        {
            context = databaseContext;
            dbSet = context.Set<Role>();
        }
        #endregion

        #region Methods
        async public Task<IRepositoryResponse> Create(TEntity entity)
        {
            var result = new RepositoryResponse();

            try
            {
                dbSet.Add(entity);
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

        async public Task<IRepositoryResponse> GetById(int id, bool fullRecord = false)
        {
            var result = new RepositoryResponse();
            var query = new Role();

            try
            {
                if (fullRecord)
                {
                    query = await dbSet
                        .Include(r => r.Users)
                            .ThenInclude(ua => ua.User)
                        .FirstOrDefaultAsync(predicate: r => r.Id == id);
                }
                else
                {
                    query = await dbSet
                        .FirstOrDefaultAsync(predicate: r => r.Id == id);
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

        async public Task<IRepositoryResponse> GetAll(bool fullRecord = false)
        {
            var result = new RepositoryResponse();
            var query = new List<Role>();

            try
            {
                if (fullRecord)
                {
                    query = await dbSet
                        .Include(r => r.Users)
                            .ThenInclude(ua => ua.User)
                        .ToListAsync();
                }
                else
                {
                    query = await dbSet.ToListAsync();
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
                dbSet.Update(entity);
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
                dbSet.Remove(entity);
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
                dbSet.RemoveRange(entities);
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
            var result = await dbSet.AnyAsync(predicate: r => r.Id == id);

            return result;
        }

        async public Task<bool> HasRoleLevel(RoleLevel level)
        {
            var result = await dbSet.AnyAsync(predicate: r => r.RoleLevel == level);

            return result;
        }

        async public Task<bool> IsListValid(List<int> ids)
        {
            var result = true;

            foreach (var id in ids)
            {
                var isIdValid = await dbSet.AnyAsync(predicate: r => r.Id == id);

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

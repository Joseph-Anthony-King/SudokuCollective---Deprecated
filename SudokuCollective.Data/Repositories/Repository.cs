using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SudokuCollective.Core.Interfaces.DataModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Interfaces.Repositories;
using SudokuCollective.Core.Models;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.DataModels;

namespace SudokuCollective.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {
        private readonly DatabaseContext context;
        private readonly DbSet<TEntity> dbSet;

        public Repository (DatabaseContext databaseContext)
        {
            context = databaseContext;
            dbSet = context.Set<TEntity>();
        }

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

            try
            {
                var query = await dbSet
                    .FirstOrDefaultAsync(predicate: x => x.Id == id);

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

            try
            {
                var query = await dbSet.ToListAsync();

                result.Success = true;
                result.Objects = query
                    .ConvertAll(x => (IEntityBase)x);

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
                dbSet.Update(entity);
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
                dbSet.RemoveRange(entities);
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

        async public Task<bool> HasEntity(int id)
        {
            var result = await dbSet.AnyAsync(predicate: x => x.Id == id);

            return result;
        }
    }
}

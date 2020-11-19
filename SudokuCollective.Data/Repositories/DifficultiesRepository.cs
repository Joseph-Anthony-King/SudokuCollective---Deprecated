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
    public class DifficultiesRepository<TEntity> : IDifficultiesRepository<TEntity> where TEntity : Difficulty
    {
        #region Fields
        private readonly DatabaseContext context;
        private readonly DbSet<Difficulty> dbSet;
        #endregion

        #region Constructor
        public DifficultiesRepository(DatabaseContext databaseContext)
        {
            context = databaseContext;
            dbSet = context.Set<Difficulty>();
        }
        #endregion

        #region Methods
        async public Task<IRepositoryResponse> Create(TEntity entity)
        {
            var result = new RepositoryResponse();

            try
            {
                if (await dbSet.AnyAsync(d => d.DifficultyLevel == entity.DifficultyLevel))
                {
                    result.Success = false;

                    return result;
                }

                dbSet.Add(entity);

                foreach (var entry in context.ChangeTracker.Entries())
                {
                    if (entry.Entity is Difficulty difficulty)
                    {
                        if (entity.Id == difficulty.Id)
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
            var query = new Difficulty();

            try
            {
                if (fullRecord)
                {
                    query = await dbSet
                        .Include(d => d.Matrices)
                        .FirstOrDefaultAsync(predicate: d => d.Id == id);
                }
                else
                {
                    query = await dbSet
                        .FirstOrDefaultAsync(predicate: d => d.Id == id);
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
            var query = new List<Difficulty>();

            try
            {
                if (fullRecord)
                {
                    query = await dbSet
                        .Where(d => 
                            d.DifficultyLevel != DifficultyLevel.NULL 
                            && d.DifficultyLevel != DifficultyLevel.TEST)
                        .Include(d => d.Matrices)
                        .ToListAsync();
                }
                else
                {
                    query = await dbSet
                        .Where(d =>
                            d.DifficultyLevel != DifficultyLevel.NULL
                            && d.DifficultyLevel != DifficultyLevel.TEST)
                        .ToListAsync();
                }

                result.Success = true;
                result.Objects = query
                    .ConvertAll(d => (IEntityBase)d)
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
                dbSet.UpdateRange(entities);

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
                if (await dbSet.AnyAsync(u => u.Id == entity.Id))
                {
                    dbSet.Remove(entity);

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
                dbSet.RemoveRange(entities);

                var difficultiesForDeletion = new List<int>();

                foreach (var entity in entities)
                {
                    difficultiesForDeletion.Add(entity.Id);
                }

                foreach (var entry in context.ChangeTracker.Entries())
                {
                    if (entry.Entity is Difficulty difficulty)
                    {
                        if (difficultiesForDeletion.Contains(difficulty.Id))
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
            var result = await dbSet.AnyAsync(predicate: d => d.Id == id);

            return result;
        }

        async public Task<bool> HasDifficultyLevel(DifficultyLevel level)
        {
            var result = await dbSet.AnyAsync(predicate: d => d.DifficultyLevel == level);

            return result;
        }
        #endregion
    }
}

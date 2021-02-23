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
        #endregion

        #region Constructor
        public DifficultiesRepository(DatabaseContext databaseContext)
        {
            context = databaseContext;
        }
        #endregion

        #region Methods
        async public Task<IRepositoryResponse> Add(TEntity entity)
        {
            var result = new RepositoryResponse();

            try
            {
                if (await context.Difficulties.AnyAsync(d => d.DifficultyLevel == entity.DifficultyLevel))
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
            var query = new Difficulty();

            try
            {
                if (fullRecord)
                {
                    query = await context
                        .Difficulties
                        .Include(d => d.Matrices)
                            .ThenInclude(m => m.SudokuCells)
                        .Include(d => d.Matrices)
                            .ThenInclude(m => m.Game)
                        .FirstOrDefaultAsync(d => d.Id == id);
                }
                else
                {
                    query = await context
                        .Difficulties
                        .FirstOrDefaultAsync(d => d.Id == id);
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

        async public Task<IRepositoryResponse> GetAll(bool fullRecord = true)
        {
            var result = new RepositoryResponse();
            var query = new List<Difficulty>();

            try
            {
                if (fullRecord)
                {
                    query = await context
                        .Difficulties
                        .Include(d => d.Matrices)
                            .ThenInclude(m => m.SudokuCells)
                        .Include(d => d.Matrices)
                            .ThenInclude(m => m.Game)
                        .Where(d => 
                            d.DifficultyLevel != DifficultyLevel.NULL 
                            && d.DifficultyLevel != DifficultyLevel.TEST)
                        .OrderBy(d => d.DifficultyLevel)
                        .ToListAsync();
                }
                else
                {
                    query = await context
                        .Difficulties
                        .Where(d =>
                            d.DifficultyLevel != DifficultyLevel.NULL
                            && d.DifficultyLevel != DifficultyLevel.TEST)
                        .OrderBy(d => d.DifficultyLevel)
                        .ToListAsync();
                }

                if (query.Count == 0)
                {
                    result.Success = false;

                    return result;
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
                if (await context.Difficulties.AnyAsync(d => d.Id == entity.Id))
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

                    if (await context.Difficulties.AnyAsync(d => d.Id == entity.Id))
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
                if (await context.Difficulties.AnyAsync(d => d.Id == entity.Id))
                {
                    context.Remove(entity);

                    if (entity.Matrices.Count == 0)
                    {
                        var games = await context
                            .Games
                            .Include(g => g.SudokuMatrix)
                                .ThenInclude(m => m.SudokuCells)
                            .ToListAsync();

                        foreach (var game in games)
                        {
                            if (game.SudokuMatrix.DifficultyId == entity.Id)
                            {
                                context.Remove(game);
                            }
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

                    if (await context.Difficulties.AnyAsync(d => d.Id == entity.Id))
                    {
                        context.Remove(entity);

                        if (entity.Matrices.Count == 0)
                        {
                            var games = await context
                                .Games
                                .Include(g => g.SudokuMatrix)
                                    .ThenInclude(m => m.SudokuCells)
                                .ToListAsync();

                            foreach (var game in games)
                            {
                                if (game.SudokuMatrix.DifficultyId == entity.Id)
                                {
                                    context.Remove(game);
                                }
                            }
                        }
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
            catch (Exception exp)
            {
                result.Success = false;
                result.Exception = exp;

                return result;
            }
        }

        async public Task<bool> HasEntity(int id)
        {
            var result = await context.Difficulties.AnyAsync(d => d.Id == id);

            return result;
        }

        async public Task<bool> HasDifficultyLevel(DifficultyLevel level)
        {
            var result = await context.Difficulties.AnyAsync(d => d.DifficultyLevel == level);

            return result;
        }
        #endregion
    }
}

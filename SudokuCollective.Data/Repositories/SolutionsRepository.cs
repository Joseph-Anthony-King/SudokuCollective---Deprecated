using System;
using System.Collections.Generic;
using System.Linq;
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
    public class SolutionsRepository<TEntity> : ISolutionsRepository<TEntity> where TEntity : SudokuSolution
    {
        #region Fields
        private readonly DatabaseContext context;
        private readonly DbSet<SudokuSolution> dbSet;
        #endregion

        #region Constructor
        public SolutionsRepository(DatabaseContext databaseContext)
        {
            context = databaseContext;
            dbSet = context.Set<SudokuSolution>();
        }
        #endregion

        #region Methods
        async public Task<IRepositoryResponse> Create(TEntity entity)
        {
            var result = new RepositoryResponse();

            try
            {
                dbSet.Add(entity);

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
            var query = new SudokuSolution();

            try
            {
                if (fullRecord)
                {
                    query = await dbSet
                        .Include(s => s.Game)
                        .ThenInclude(g => g.User)
                        .FirstOrDefaultAsync(predicate: s => s.Id == id);
                }
                else
                {
                    query = await dbSet
                        .FirstOrDefaultAsync(predicate: s => s.Id == id);
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
            var query = new List<SudokuSolution>();

            try
            {
                if (fullRecord)
                {
                    query = await dbSet
                        .Include(s => s.Game)
                        .ThenInclude(g => g.User)
                        .ToListAsync();
                }
                else
                {
                    query = await dbSet.ToListAsync();
                }

                result.Success = true;
                result.Objects = query
                    .ConvertAll(s => (IEntityBase)s)
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

        async public Task<IRepositoryResponse> AddSolutions(List<ISudokuSolution> solutions)
        {
            var result = new RepositoryResponse();

            try
            {
                dbSet.AddRange(solutions.ConvertAll(s => (SudokuSolution)s));

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

        async public Task<IRepositoryResponse> GetSolvedSolutions()
        {
            var result = new RepositoryResponse();
            var query = new List<SudokuSolution>();

            try
            {
                query = await dbSet
                    .Where(s => s.DateSolved > DateTime.MinValue)
                    .ToListAsync();

                result.Success = true;
                result.Objects = query
                    .ConvertAll(s => (IEntityBase)s)
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
        #endregion
    }
}

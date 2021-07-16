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
        private readonly DatabaseContext _context;
        #endregion

        #region Constructor
        public SolutionsRepository(DatabaseContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        public async Task<IRepositoryResponse> Add(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var result = new RepositoryResponse();

            if (entity.Id != 0)
            {
                result.Success = false;

                return result;
            }

            try
            {
                _context.Attach(entity);

                foreach (var entry in _context.ChangeTracker.Entries())
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

                await _context.SaveChangesAsync();

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

        public async Task<IRepositoryResponse> Get(int id)
        {
            var result = new RepositoryResponse();

            if (id == 0)
            {
                result.Success = false;

                return result;
            }

            try
            {
                var query = await _context
                    .SudokuSolutions
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (query == null)
                {
                    result.Success = false;
                }
                else
                {
                    result.Success = true;
                    result.Object = query;
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

        public async Task<IRepositoryResponse> GetAll()
        {
            var result = new RepositoryResponse();

            try
            {
                var query = await _context
                    .SudokuSolutions
                    .ToListAsync();

                if (query.Count == 0)
                {
                    result.Success = false;
                }
                else
                {
                    result.Success = true;
                    result.Objects = query
                        .ConvertAll(s => (IEntityBase)s)
                        .ToList();
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

        public async Task<IRepositoryResponse> AddSolutions(List<ISudokuSolution> solutions)
        {
            if (solutions == null) throw new ArgumentNullException(nameof(solutions));

            var result = new RepositoryResponse();

            try
            {
                _context.AddRange(solutions.ConvertAll(s => (SudokuSolution)s));

                foreach (var entry in _context.ChangeTracker.Entries())
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

                await _context.SaveChangesAsync();

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

        public async Task<IRepositoryResponse> GetSolvedSolutions()
        {
            var result = new RepositoryResponse();

            try
            {
                var query = await _context
                    .SudokuSolutions
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

        public Task<IRepositoryResponse> Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<IRepositoryResponse> UpdateRange(List<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public Task<IRepositoryResponse> Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<IRepositoryResponse> DeleteRange(List<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasEntity(int id)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

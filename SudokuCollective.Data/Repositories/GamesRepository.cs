using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SudokuCollective.Core.Interfaces.DataModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Interfaces.Repositories;
using SudokuCollective.Core.Models;
using SudokuCollective.Data.Helpers;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.DataModels;

namespace SudokuCollective.Data.Repositories
{
    public class GamesRepository<TEntity> : IGamesRepository<TEntity> where TEntity : Game
    {
        #region Fields
        private readonly DatabaseContext context;
        private readonly DbSet<Game> dbSet;
        private readonly DbSet<SudokuMatrix> matrixDbSet;
        private readonly DbSet<SudokuCell> cellDbSet;
        #endregion

        #region Constructor
        public GamesRepository(DatabaseContext databaseContext)
        {
            context = databaseContext;
            dbSet = context.Set<Game>();
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
            var query = new Game();

            try
            {
                if (fullRecord)
                {
                    query = await dbSet
                        .Include(g => g.User)
                            .ThenInclude(u => u.Roles)
                        .Include(g => g.SudokuMatrix)
                        .Include(g => g.SudokuSolution)
                        .FirstOrDefaultAsync(predicate: g => g.Id == id);

                    await query.SudokuMatrix.AttachSudokuCells(context);
                }
                else
                {
                    query = await dbSet
                        .FirstOrDefaultAsync(predicate: g => g.Id == id);
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
            var query = new List<Game>();

            try
            {
                if (fullRecord)
                {
                    query = await dbSet
                        .Include(g => g.User)
                            .ThenInclude(u => u.Roles)
                        .Include(g => g.SudokuMatrix)
                        .Include(g => g.SudokuSolution)
                        .ToListAsync();

                    foreach (var game in query)
                    {
                        await game.SudokuMatrix.AttachSudokuCells(context);
                    }
                }
                else
                {
                    query = await dbSet.ToListAsync();
                }

                result.Success = true;
                result.Objects = query
                    .ConvertAll(g => (IEntityBase)g)
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
                foreach (var cell in entity.SudokuMatrix.SudokuCells)
                {
                    cellDbSet.Update(cell);
                }

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
                foreach (var cell in entity.SudokuMatrix.SudokuCells)
                {
                    cellDbSet.Remove(cell);
                }

                matrixDbSet.Remove(entity.SudokuMatrix);

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
            var result = await dbSet.AnyAsync(predicate: g => g.Id == id);

            return result;
        }

        async public Task<IRepositoryResponse> GetGame(int id, int appid, bool fullRecord = false)
        {
            var result = new RepositoryResponse();

            try
            {
                var query = new Game();

                if (fullRecord)
                {
                    query = await dbSet
                        .Include(g => g.User)
                            .ThenInclude(u => u.Roles)
                        .Include(g => g.SudokuMatrix)
                        .Include(g => g.SudokuSolution)
                        .FirstOrDefaultAsync(predicate: g => g.Id == id && g.AppId == appid);

                    await query.SudokuMatrix.AttachSudokuCells(context);
                }
                else
                {
                    query = await dbSet
                        .FirstOrDefaultAsync(predicate: g => g.Id == id && g.AppId == appid);
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

        async public Task<IRepositoryResponse> GetGames(int appid, bool fullRecord = false)
        {
            var result = new RepositoryResponse();

            try
            {
                var query = new List<Game>();

                if (fullRecord)
                {
                    query = await dbSet
                        .Include(g => g.User)
                            .ThenInclude(u => u.Roles)
                        .Include(g => g.SudokuMatrix)
                        .Include(g => g.SudokuSolution)
                        .Where(g => g.AppId == appid)
                        .ToListAsync();

                    foreach (var game in query)
                    {

                        await game.SudokuMatrix.AttachSudokuCells(context);
                    }
                }
                else
                {
                    query = await dbSet
                        .Where(g => g.AppId == appid)
                        .ToListAsync();
                }

                result.Success = true;
                result.Objects = query.ConvertAll(g => (IEntityBase)g);

                return result;
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Exception = exp;

                return result;
            }
        }

        async public Task<IRepositoryResponse> GetMyGame(int userid, int gameid, int appid, bool fullRecord = false)
        {
            var result = new RepositoryResponse();

            try
            {
                var query = new Game();

                if (fullRecord)
                {
                    query = await dbSet
                        .Include(g => g.User)
                            .ThenInclude(u => u.Roles)
                        .Include(g => g.SudokuMatrix)
                        .Include(g => g.SudokuSolution)
                        .FirstOrDefaultAsync(predicate:
                            g => g.Id == gameid
                            && g.AppId == appid
                            && g.UserId == userid);

                    await query.SudokuMatrix.AttachSudokuCells(context);
                }
                else
                {
                    query = await dbSet
                        .FirstOrDefaultAsync(predicate:
                            g => g.Id == gameid
                            && g.AppId == appid
                            && g.UserId == userid);
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

        async public Task<IRepositoryResponse> GetMyGames(int userid, int appid, bool fullRecord = false)
        {
            var result = new RepositoryResponse();

            try
            {
                var query = new List<Game>();

                if (fullRecord)
                {
                    query = await dbSet
                        .Include(g => g.User)
                            .ThenInclude(u => u.Roles)
                        .Include(g => g.SudokuMatrix)
                        .Include(g => g.SudokuSolution)
                        .Where(g => g.AppId == appid && g.UserId == userid)
                        .ToListAsync();

                    foreach (var game in query)
                    {

                        await game.SudokuMatrix.AttachSudokuCells(context);
                    }
                }
                else
                {
                    query = await dbSet
                        .Where(g => g.AppId == appid && g.UserId == userid)
                        .ToListAsync();
                }

                result.Success = true;
                result.Objects = query.ConvertAll(g => (IEntityBase)g);

                return result;
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Exception = exp;

                return result;
            }
        }

        async public Task<IRepositoryResponse> DeleteMyGame(int userid, int gameid, int appid)
        {
            var result = new RepositoryResponse();

            try
            {
                var query = new Game();

                query = await dbSet
                    .Include(g => g.User)
                        .ThenInclude(u => u.Roles)
                    .Include(g => g.SudokuMatrix)
                    .Include(g => g.SudokuSolution)
                    .FirstOrDefaultAsync(predicate:
                        g => g.Id == gameid
                        && g.AppId == appid
                        && g.UserId == userid);

                await query.SudokuMatrix.AttachSudokuCells(context);

                foreach (var cell in query.SudokuMatrix.SudokuCells)
                {
                    cellDbSet.Remove(cell);
                }

                matrixDbSet.Remove(query.SudokuMatrix);

                dbSet.Remove(query);

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
        #endregion
    }
}

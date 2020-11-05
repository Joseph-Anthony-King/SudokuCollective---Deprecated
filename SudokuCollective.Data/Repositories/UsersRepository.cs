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
using SudokuCollective.Data.Helpers;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.DataModels;

namespace SudokuCollective.Data.Repositories
{
    public class UsersRepository<TEntity> : IUsersRepository<TEntity> where TEntity : User
    {
        #region Fields
        private readonly DatabaseContext context;
        private readonly DbSet<User> dbSet;
        private readonly DbSet<UserRole> userRoleDbSet;
        private readonly DbSet<Role> roleDbSet;
        private readonly DbSet<UserApp> userAppDbSet;
        private readonly DbSet<Game> gameDbSet;
        private readonly DbSet<SudokuCell> cellDbSet;
        #endregion

        #region Constructors
        public UsersRepository(DatabaseContext databaseContext)
        {
            context = databaseContext;
            dbSet = context.Set<User>();
            userRoleDbSet = context.Set<UserRole>();
            roleDbSet = context.Set<Role>();
            userAppDbSet = context.Set<UserApp>();
            gameDbSet = context.Set<Game>();
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

                var user = await dbSet.FirstOrDefaultAsync(
                    predicate: u => u.Email.ToLower().Equals(entity.Email.ToLower()));

                var defaultRole = await userRoleDbSet
                    .FirstOrDefaultAsync(
                        predicate: ur => ur.Role.RoleLevel == RoleLevel.USER);

                var userRole = new UserRole(user.Id, defaultRole.Id);

                user.Roles.Add(userRole);

                await context.SaveChangesAsync();

                result.Object = user;
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
            var query = new User();

            try
            {
                if (fullRecord)
                {
                    query = await dbSet
                        .Include(u => u.Games)
                        .Include(u => u.Roles)
                        .Include(u => u.Apps)
                        .FirstOrDefaultAsync(predicate: u => u.Id == id);

                    foreach (var role in query.Roles)
                    {
                        role.Role = await roleDbSet
                            .FirstOrDefaultAsync(r => r.Id == role.RoleId);

                        role.Role.Users = null;
                    }

                    foreach (var game in query.Games)
                    {
                        await game.SudokuMatrix.AttachSudokuCells(context);
                    }
                }
                else
                {
                    query = await dbSet
                        .FirstOrDefaultAsync(predicate: u => u.Id == id);
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

        async public Task<IRepositoryResponse> GetByUserName(string username, bool fullRecord = false)
        {
            var result = new RepositoryResponse();
            var query = new User();

            try
            {
                if (fullRecord)
                {
                    query = await dbSet
                        .Include(u => u.Games)
                        .Include(u => u.Roles)
                        .Include(u => u.Apps)
                        .FirstOrDefaultAsync(
                            predicate: u => u.UserName.ToLower().Equals(username.ToLower()));

                    foreach (var role in query.Roles)
                    {
                        role.Role = await roleDbSet
                            .FirstOrDefaultAsync(r => r.Id == role.RoleId);

                        role.Role.Users = null;
                    }

                    foreach (var game in query.Games)
                    {
                        await game.SudokuMatrix.AttachSudokuCells(context);
                    }
                }
                else
                {
                    query = await dbSet
                        .FirstOrDefaultAsync(
                            predicate: u => u.UserName.ToLower().Equals(username.ToLower()));
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

        async public Task<IRepositoryResponse> GetByEmail(string email, bool fullRecord = false)
        {
            var result = new RepositoryResponse();
            var query = new User();

            try
            {
                if (fullRecord)
                {
                    query = await dbSet
                        .Include(u => u.Games)
                        .Include(u => u.Roles)
                        .Include(u => u.Apps)
                        .FirstOrDefaultAsync(
                            predicate: u => u.Email.ToLower().Equals(email.ToLower()));

                    foreach (var game in query.Games)
                    {
                        await game.SudokuMatrix.AttachSudokuCells(context);
                    }
                }
                else
                {
                    query = await dbSet
                        .FirstOrDefaultAsync(predicate: u => u.Email.ToLower().Equals(email.ToLower()));
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
            var query = new List<User>();

            try
            {
                if (fullRecord)
                {
                    query = await dbSet
                        .Include(u => u.Games)
                        .Include(u => u.Roles)
                        .Include(u => u.Apps)
                        .ToListAsync();

                    foreach (var user in query)
                    {
                        foreach (var game in user.Games)
                        {
                            await game.SudokuMatrix.AttachSudokuCells(context);
                        }
                    }
                }
                else
                {
                    query = await dbSet.ToListAsync();
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

        async public Task<IRepositoryResponse> Update(TEntity entity)
        {
            var result = new RepositoryResponse();

            try
            {
                dbSet.Update(entity);
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
                foreach (var game in entity.Games)
                {
                    await game.SudokuMatrix.AttachSudokuCells(context);

                    foreach (var cell in game.SudokuMatrix.SudokuCells)
                    {
                        cellDbSet.Remove(cell);
                    }

                    gameDbSet.Remove(game);
                }

                foreach (var app in entity.Apps)
                {
                    userAppDbSet.Remove(app);
                }

                foreach (var role in entity.Roles)
                {
                    userRoleDbSet.Remove(role);
                }

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
            var result = await dbSet.AnyAsync(predicate: u => u.Id == id);

            return result;
        }

        async public Task<IRepositoryResponse> AddRoles(int userId, List<int> roles)
        {
            var result = new RepositoryResponse();

            try
            {
                foreach (var roleId in roles)
                {
                   var userRole = new UserRole(userId, roleId);

                    userRoleDbSet.Add(userRole);
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

        async public Task<IRepositoryResponse> RemoveRoles(int userid, List<int> roles)
        {
            var result = new RepositoryResponse();

            try
            {
                foreach (var roleId in roles)
                {
                    var userRole = new UserRole(userid, roleId);

                    userRoleDbSet.Remove(userRole);
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

        async public Task<bool> ActivateUser(int id)
        {
            try
            {
                var user = await dbSet.FirstOrDefaultAsync(predicate: u => u.Id == id);

                user.ActivateUser();

                dbSet.Update(user);

                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        async public Task<bool> DeactivateUser(int id)
        {
            try
            {
                var user = await dbSet.FirstOrDefaultAsync(predicate: u => u.Id == id);

                user.DeactiveUser();

                dbSet.Update(user);

                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        async public Task<bool> PromoteUserToAdmin(int id)
        {
            try
            {
                var user = await dbSet.FirstOrDefaultAsync(predicate: u => u.Id == id);
                var role = await roleDbSet.FirstOrDefaultAsync(predicate: r => r.RoleLevel == RoleLevel.ADMIN);

                var userRole = new UserRole(user.Id, role.Id);

                userRoleDbSet.Add(userRole);

                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        async public Task<bool> IsUserRegistered(int id)
        {
            return await dbSet.AnyAsync(predicate: u => u.Id == id);
        }

        async public Task<bool> IsUserNameUnique(string username)
        {
            return await dbSet.AnyAsync(
                predicate: u => !u.UserName.ToLower().Equals(username.ToLower()));
        }

        async public Task<bool> IsEmailUnique(string email)
        {
            return await dbSet.AnyAsync(
                predicate: u => !u.Email.ToLower().Equals(email.ToLower()));
        }
        #endregion
    }
}

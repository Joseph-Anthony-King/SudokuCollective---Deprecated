using System.Collections.Generic;
using System.Threading.Tasks;
using SudokuCollective.Core.Interfaces.DataModels;
using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Core.Interfaces.Repositories
{
    public interface IUsersRepository<TEntity> : IRepository<TEntity> where TEntity : IUser
    {
        Task<IRepositoryResponse> GetByUserName(string username, bool fullRecord = true);
        Task<IRepositoryResponse> GetByEmail(string email, bool fullRecord = true);
        Task<IRepositoryResponse> AddRoles(int userid, List<int> roles);
        Task<IRepositoryResponse> RemoveRoles(int userid, List<int> roles);
        Task<bool> ActivateUser(int id);
        Task<bool> DeactivateUser(int id);
        Task<bool> PromoteUserToAdmin(int id);
        Task<bool> IsUserRegistered(int id);
        Task<bool> IsUserNameUnique(string username);
        Task<bool> IsEmailUnique(string email);
    }
}

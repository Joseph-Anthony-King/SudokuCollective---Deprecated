using System.Threading.Tasks;
using SudokuCollective.Core.Interfaces.DataModels;
using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Core.Interfaces.Repositories
{
    public interface IAppsRepository<TEntity> : IRepository<TEntity> where TEntity : IApp
    {
        Task<IRepositoryResponse> GetByLicense(string license, bool fullRecord = true);
        Task<IRepositoryResponse> GetAppUsers(int id, bool fullRecord = true);
        Task<IRepositoryResponse> GetMyApps(int ownerId, bool fullRecord = true);
        Task<IRepositoryResponse> AddAppUser(int userId, string license);
        Task<IRepositoryResponse> RemoveAppUser(int userId, string license);
        Task<IRepositoryResponse> Reset(TEntity entity);
        Task<IRepositoryResponse> Activate(int id);
        Task<IRepositoryResponse> Deactivate(int id);
        Task<bool> IsAppLicenseValid(string license);
        Task<bool> IsUserRegisteredToApp(int id, string license, int userId);
        Task<bool> IsUserOwnerOfApp(int id, string license, int userId);
        Task<string> GetLicense(int id);
    }
}

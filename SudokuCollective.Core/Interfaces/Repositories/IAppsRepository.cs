using System.Threading.Tasks;
using SudokuCollective.Core.Interfaces.DataModels;
using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Core.Interfaces.Repositories
{
    public interface IAppsRepository<TEntity> : IRepository<TEntity> where TEntity : IApp
    {
        Task<IRepositoryResponse> GetByLicense(string license, bool fullRecord = false);
        Task<IRepositoryResponse> GetAppUsers(int id, bool fullRecord = false);
        Task<IRepositoryResponse> AddAppUser(int userId, string license);
        Task<IRepositoryResponse> RemoveAppUser(int userId, string license);
        Task<IRepositoryResponse> ResetApp(TEntity entity);
        Task<IRepositoryResponse> ActivateApp(int id);
        Task<IRepositoryResponse> DeactivateApp(int id);
        Task<bool> IsAppLicenseValid(string license);
        Task<string> GetLicense(int id);
        Task<bool> IsUserRegisteredToApp(int id, string license, int userId);
        Task<bool> IsUserOwnerOfApp(int id, string license, int userId);
    }
}

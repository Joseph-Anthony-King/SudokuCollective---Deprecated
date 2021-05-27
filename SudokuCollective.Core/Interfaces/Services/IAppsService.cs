using System.Threading.Tasks;
using SudokuCollective.Core.Interfaces.APIModels.PageModels;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;

namespace SudokuCollective.Core.Interfaces.Services
{
    public interface IAppsService : IService
    {
        Task<IAppResult> GetApp(int id, int requestorId, bool fullRecord = true);
        Task<IAppsResult> GetApps(IPaginator paginator, int requestorId, bool fullRecord = true);
        Task<IAppsResult> GetMyApps(int ownerId, IPaginator paginator, bool fullRecord = true);
        Task<IAppResult> CreateApp(ILicenseRequest request);
        Task<IAppResult> GetAppByLicense(string license, int requestorId, bool fullRecord = true);
        Task<ILicenseResult> GetLicense(int id);
        Task<IUsersResult> GetAppUsers(int id, int requestorId, IPaginator paginator, bool appUsers = true, bool fullRecord = true);
        Task<IAppResult> UpdateApp(int id, IAppRequest request);
        Task<IBaseResult> AddAppUser(int appId, int userId);
        Task<IBaseResult> RemoveAppUser(int appId, int userId);
        Task<IBaseResult> DeleteOrResetApp(int id, bool isReset = false);
        Task<IBaseResult> ActivateApp(int id);
        Task<IBaseResult> DeactivateApp(int id);
        Task<IUserResult> ActivateAdminPrivileges(int appId, int userId);
        Task<IUserResult> DeactivateAdminPrivileges(int appId, int userId);
        Task<bool> IsRequestValidOnThisLicense(int id, string license, int userId);
        Task<bool> IsOwnerOfThisLicense(int id, string license, int userId);
    }
}

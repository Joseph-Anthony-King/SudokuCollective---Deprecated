using System.Threading.Tasks;
using SudokuCollective.Core.Interfaces.APIModels.PageModels;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;

namespace SudokuCollective.Core.Interfaces.Services
{
    public interface IAppsService : IService
    {
        Task<IAppResult> GetApp(int id, bool fullRecord = true);
        Task<IAppsResult> GetApps(IPageListModel pageListModel, bool fullRecord = true);
        Task<IAppResult> CreateApp(ILicenseRequest request);
        Task<IAppResult> GetAppByLicense(string license, bool fullRecord = true);
        Task<ILicenseResult> GetLicense(int id);
        Task<IUsersResult> GetAppUsers(int id, int requestorId, IPageListModel pageListModel, bool fullRecord = true);
        Task<IAppResult> UpdateApp(IAppRequest request);
        Task<IBaseResult> AddAppUser(int id, IBaseRequest request);
        Task<IBaseResult> RemoveAppUser(int id, IBaseRequest request);
        Task<IBaseResult> DeleteOrResetApp(int id, bool isReset = false);
        Task<IBaseResult> ActivateApp(int id);
        Task<IBaseResult> DeactivateApp(int id);
        Task<bool> IsRequestValidOnThisLicense(int id, string license, int userId);
        Task<bool> IsOwnerOfThisLicense(int id, string license, int userId);
    }
}

using System.Threading.Tasks;
using SudokuCollective.Core.Interfaces.APIModels.PageModels;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;

namespace SudokuCollective.Core.Interfaces.Services
{
    public interface IAppsService
    {
        Task<IAppResult> GetApp(int id, bool fullRecord = false);
        Task<IAppsResult> GetApps(IPageListModel pageListModel, bool fullRecord = false);
        Task<IAppResult> CreateApp(ILicenseRequest licenseRequestRO);
        Task<IAppResult> GetAppByLicense(string license, bool fullRecord = false);
        Task<ILicenseResult> GetLicense(int id);
        Task<IUsersResult> GetAppUsers(int id, IPageListModel pageListModel, bool fullRecord = false);
        Task<IBaseResult> UpdateApp(IAppRequest updateAppRO);
        Task<IBaseResult> AddAppUser(int id, IBaseRequest baseRequestRO);
        Task<IBaseResult> RemoveAppUser(int id, IBaseRequest baseRequestRO);
        Task<IBaseResult> DeleteOrResetApp(int id, bool isReset = false);
        Task<IBaseResult> ActivateApp(int id);
        Task<IBaseResult> DeactivateApp(int id);
        Task<bool> IsRequestValidOnThisLicense(int id, string license, int userId);
        Task<bool> IsOwnerOfThisLicense(int id, string license, int userId);
    }
}

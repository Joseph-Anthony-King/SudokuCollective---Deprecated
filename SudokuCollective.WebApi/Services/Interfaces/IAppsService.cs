using System.Threading.Tasks;
using SudokuCollective.WebApi.Models.PageModels;
using SudokuCollective.WebApi.Models.RequestModels;
using SudokuCollective.WebApi.Models.RequestModels.AppRequests;
using SudokuCollective.WebApi.Models.ResultModels;
using SudokuCollective.WebApi.Models.ResultModels.AppRequests;
using SudokuCollective.WebApi.Models.ResultModels.UserRequests;

namespace SudokuCollective.WebApi.Services.Interfaces {

    public interface IAppsService {

        Task<AppResult> GetApp(int id, bool fullRecord = false);
        Task<AppsResult> GetApps(PageListModel pageListModel, bool fullRecord = false);
        Task<AppResult> CreateApp(LicenseRequest licenseRequestRO);
        Task<AppResult> GetAppByLicense(string license, bool fullRecord = false);
        Task<LicenseResult> GetLicense(int id);
        Task<UsersResult> GetAppUsers(BaseRequest baseRequest, bool fullRecord = false);
        Task<BaseResult> UpdateApp(AppRequest updateAppRO);
        Task<BaseResult> AddAppUser(int id, BaseRequest baseRequestRO);
        Task<BaseResult> RemoveAppUser(int id, BaseRequest baseRequestRO);
        Task<BaseResult> DeleteOrResetApp(int id, bool isReset = false);
        Task<BaseResult> ActivateApp(int id);
        Task<BaseResult> DeactivateApp(int id);
        Task<bool> IsRequestValidOnThisLicense(string license, int userId, int appId);
        Task<bool> IsOwnerOfThisLicense(string license, int userId, int appId);
    }
}

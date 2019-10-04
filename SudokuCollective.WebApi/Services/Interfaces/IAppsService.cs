using System.Threading.Tasks;
using SudokuCollective.WebApi.Models;
using SudokuCollective.WebApi.Models.RequestModels;
using SudokuCollective.WebApi.Models.RequestModels.AppRequests;
using SudokuCollective.WebApi.Models.TaskModels;
using SudokuCollective.WebApi.Models.TaskModels.AppRequests;
using SudokuCollective.WebApi.Models.TaskModels.UserRequests;

namespace SudokuCollective.WebApi.Services.Interfaces {

    public interface IAppsService {

        Task<AppTaskResult> GetApp(int id, bool fullRecord = false);
        Task<AppListTaskResult> GetApps(PageListModel pageListModel, bool fullRecord = false);
        Task<AppTaskResult> CreateApp(LicenseRequestRO licenseRequestRO);
        Task<AppTaskResult> GetAppByLicense(string license, bool fullRecord = false);
        Task<LicenseTaskResult> GetLicense(int id);
        Task<UserListTaskResult> GetAppUsers(BaseRequestRO baseRequest, bool fullRecord = false);
        Task<BaseTaskResult> UpdateApp(UpdateAppRO updateAppRO);
        Task<BaseTaskResult> AddAppUser(int id, BaseRequestRO baseRequestRO);
        Task<BaseTaskResult> RemoveAppUser(int id, BaseRequestRO baseRequestRO);
        Task<BaseTaskResult> DeleteApp(int id);
        Task<BaseTaskResult> ActivateApp(int id);
        Task<BaseTaskResult> DeactivateApp(int id);
        Task<bool> IsRequestValidOnThisLicense(string license, int userId);
    }
}

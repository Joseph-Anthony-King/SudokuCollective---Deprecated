using System.Threading.Tasks;
using SudokuCollective.WebApi.Models.RequestModels;
using SudokuCollective.WebApi.Models.RequestModels.AppRequests;
using SudokuCollective.WebApi.Models.TaskModels;
using SudokuCollective.WebApi.Models.TaskModels.AppRequests;
using SudokuCollective.WebApi.Models.TaskModels.UserRequests;

namespace SudokuCollective.WebApi.Services.Interfaces {

    public interface IAppsService {

        Task<AppTaskResult> GetApp(int id, bool fullRecord = true);
        Task<AppListTaskResult> GetApps(bool fullRecord = true);
        Task<AppTaskResult> CreateApp(LicenseRequestRO licenseRequestRO);
        Task<AppTaskResult> GetAppByLicense(string license, bool fullRecord = true);
        Task<LicenseTaskResult> GetLicense(int id);
        Task<UserListTaskResult> GetAppUsers(BaseRequestRO baseRequest, bool fullRecord = true);
        Task<BaseTaskResult> UpdateApp(UpdateAppRO updateAppRO);
        Task<BaseTaskResult> AddAppUser(int id, BaseRequestRO baseRequestRO);
        Task<BaseTaskResult> RemoveAppUser(int id, BaseRequestRO baseRequestRO);
        Task<BaseTaskResult> DeleteApp(int id);
        Task<BaseTaskResult> ActivateApp(int id);
        Task<BaseTaskResult> DeactivateApp(int id);
        Task<bool> IsRequestValidOnThisLicense(string license, int userId);
    }
}

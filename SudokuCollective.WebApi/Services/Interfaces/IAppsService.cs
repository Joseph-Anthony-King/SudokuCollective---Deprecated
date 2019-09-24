using System.Threading.Tasks;
using SudokuCollective.Models;
using SudokuCollective.WebApi.Models.RequestModels;
using SudokuCollective.WebApi.Models.TaskModels.AppRequests;

namespace SudokuCollective.WebApi.Services.Interfaces {

    public interface IAppsService {

        Task<AppTaskResult> GetApp(int id, bool fullRecord = true);
        Task<AppListTaskResult> GetApps(bool fullRecord = true);
        Task<AppTaskResult> CreateApp(LicenseRequestRO licenseRequestRO);
        Task<bool> UpdateApp(int id, App app);
        Task<bool> DeleteApp(int id);
        Task<LicenseTaskResult> GetLicense(int id);
        bool ValidLicense(string license);
    }
}

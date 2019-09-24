using System.Threading.Tasks;
using SudokuCollective.Models;
using SudokuCollective.WebApi.Models.TaskModels.AppRequests;

namespace SudokuCollective.WebApi.Services.Interfaces {

    public interface IAppsService {

        Task<AppTaskResult> GetApp(int id, bool fullRecord = true);
        Task<AppListTaskResult> GetApps(bool fullRecord = true);
        Task<AppTaskResult> CreateApp(string name, int ownerId, 
            string devUrl, string liveUrl);
        Task<bool> UpdateApp(int id, App app);
        Task<bool> DeleteApp(int id);
        bool ValidLicense(string license);
    }
}

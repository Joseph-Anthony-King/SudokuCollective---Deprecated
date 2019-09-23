using System.Threading.Tasks;
using SudokuCollective.Models;
using SudokuCollective.Models.Enums;
using SudokuCollective.WebApi.Models.TaskModels.RoleRequests;

namespace SudokuCollective.WebApi.Services.Interfaces
{

    public interface IRolesService {

        Task<RoleTaskResult> GetRole(int id, bool fullRecord = true);
        Task<RoleListTaskResult> GetRoles(bool fullRecord = true);
        Task<RoleTaskResult> CreateRole(string name, RoleLevel roleLevel);
        Task<bool> UpdateRole(int id, Role difficulty);
        Task<bool> DeleteRole(int id);
    }
}

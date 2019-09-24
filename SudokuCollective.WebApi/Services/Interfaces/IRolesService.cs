using System.Threading.Tasks;
using SudokuCollective.Models.Enums;
using SudokuCollective.WebApi.Models.RequestModels.RoleRequests;
using SudokuCollective.WebApi.Models.TaskModels.RoleRequests;

namespace SudokuCollective.WebApi.Services.Interfaces {

    public interface IRolesService {

        Task<RoleTaskResult> GetRole(int id, bool fullRecord = true);
        Task<RoleListTaskResult> GetRoles(bool fullRecord = true);
        Task<RoleTaskResult> CreateRole(string name, RoleLevel roleLevel);
        Task<bool> UpdateRole(int id, UpdateRoleRO updateRoleRO);
        Task<bool> DeleteRole(int id);
    }
}

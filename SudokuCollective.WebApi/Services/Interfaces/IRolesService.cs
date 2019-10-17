using System.Threading.Tasks;
using SudokuCollective.Domain.Enums;
using SudokuCollective.WebApi.Models.RequestModels.RoleRequests;
using SudokuCollective.WebApi.Models.ResultModels;
using SudokuCollective.WebApi.Models.ResultModels.RoleRequests;

namespace SudokuCollective.WebApi.Services.Interfaces {

    public interface IRolesService {

        Task<RoleResult> GetRole(int id, bool fullRecord = false);
        Task<RolesResult> GetRoles(bool fullRecord = false);
        Task<RoleResult> CreateRole(string name, RoleLevel roleLevel);
        Task<BaseResult> UpdateRole(int id, UpdateRoleRequest updateRoleRO);
        Task<BaseResult> DeleteRole(int id);
    }
}

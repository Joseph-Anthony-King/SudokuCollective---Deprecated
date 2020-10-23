using System.Threading.Tasks;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;

namespace SudokuCollective.Core.Interfaces.Services
{
    public interface IRolesService
    {
        Task<IRoleResult> GetRole(int id, bool fullRecord = false);
        Task<IRolesResult> GetRoles(bool fullRecord = false);
        Task<IRoleResult> CreateRole(string name, RoleLevel roleLevel);
        Task<IBaseResult> UpdateRole(int id, IUpdateRoleRequest updateRoleRO);
        Task<IBaseResult> DeleteRole(int id);
    }
}

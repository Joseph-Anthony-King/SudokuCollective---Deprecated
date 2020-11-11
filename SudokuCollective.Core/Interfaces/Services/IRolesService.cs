using System.Threading.Tasks;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;

namespace SudokuCollective.Core.Interfaces.Services
{
    public interface IRolesService : IService
    {
        Task<IRoleResult> GetRole(int id, bool fullRecord = true);
        Task<IRolesResult> GetRoles(bool fullRecord = true);
        Task<IRoleResult> CreateRole(string name, RoleLevel roleLevel);
        Task<IBaseResult> UpdateRole(int id, IUpdateRoleRequest updateRoleRO);
        Task<IBaseResult> DeleteRole(int id);
    }
}

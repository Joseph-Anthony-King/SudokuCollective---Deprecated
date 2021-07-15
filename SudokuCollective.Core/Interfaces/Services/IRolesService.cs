using System.Threading.Tasks;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;

namespace SudokuCollective.Core.Interfaces.Services
{
    public interface IRolesService : IService
    {
        Task<IRoleResult> Create(string name, RoleLevel roleLevel);
        Task<IRoleResult> Get(int id, bool fullRecord = true);
        Task<IBaseResult> Update(int id, IUpdateRoleRequest updateRoleRO);
        Task<IBaseResult> Delete(int id);
        Task<IRolesResult> GetRoles(bool fullRecord = true);
    }
}

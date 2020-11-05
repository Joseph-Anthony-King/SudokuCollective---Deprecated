using System.Collections.Generic;
using System.Threading.Tasks;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Core.Interfaces.Repositories
{
    public interface IRolesRepository<TEntity> : IRepository<TEntity> where TEntity : IRole
    {
        Task<bool> HasRoleLevel(RoleLevel level);
        Task<bool> IsListValid(List<int> ids);
    }
}

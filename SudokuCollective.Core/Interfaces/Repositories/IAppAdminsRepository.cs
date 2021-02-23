using System.Threading.Tasks;
using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Core.Interfaces.Repositories
{
    public interface IAppAdminsRepository<TEntity> : IRepository<TEntity> where TEntity : IAppAdmin
    {
        Task<bool> HasAdminRecord(int appId, int userId);
    }
}
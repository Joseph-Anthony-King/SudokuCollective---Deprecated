using System.Threading.Tasks;
using SudokuCollective.Core.Interfaces.DataModels;
using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Core.Interfaces.Repositories
{
    public interface IPasswordResetsRepository<TEntity> where TEntity : IEntityBase
    {
        Task<IRepositoryResponse> Create(TEntity entity);
        Task<IRepositoryResponse> Get(string token);
        Task<IRepositoryResponse> GetAll();
        Task<IRepositoryResponse> Delete(TEntity entity);
        Task<bool> HasEntity(int id);
    }
}

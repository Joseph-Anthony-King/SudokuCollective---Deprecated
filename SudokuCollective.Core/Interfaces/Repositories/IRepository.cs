using System.Collections.Generic;
using System.Threading.Tasks;
using SudokuCollective.Core.Interfaces.DataModels;
using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Core.Interfaces.Repositories
{
    public interface IRepository<TEntity> where TEntity : IEntityBase
    {
        Task<IRepositoryResponse> Create(TEntity entity);
        Task<IRepositoryResponse> GetById(int id, bool fullRecord = false);
        Task<IRepositoryResponse> GetAll(bool fullRecord = false);
        Task<IRepositoryResponse> Update(TEntity entity);
        Task<IRepositoryResponse> UpdateRange(List<TEntity> entities);
        Task<IRepositoryResponse> Delete(TEntity entity);
        Task<IRepositoryResponse> DeleteRange(List<TEntity> entities);
        Task<bool> HasEntity(int id);
    }
}

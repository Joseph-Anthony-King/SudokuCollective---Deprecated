using System.Collections.Generic;
using System.Threading.Tasks;
using SudokuCollective.Core.Interfaces.DataModels;
using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Core.Interfaces.Repositories
{
    public interface ISolutionsRepository<TEntity> where TEntity : ISudokuSolution
    {
        Task<IRepositoryResponse> Create(TEntity entity);
        Task<IRepositoryResponse> Get(int id, bool fullRecord = true);
        Task<IRepositoryResponse> GetAll(bool fullRecord = true);
        Task<IRepositoryResponse> AddSolutions(List<ISudokuSolution> solutions);
        Task<IRepositoryResponse> GetSolvedSolutions();
    }
}

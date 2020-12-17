using System.Threading.Tasks;
using SudokuCollective.Core.Interfaces.DataModels;
using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Core.Interfaces.Repositories
{
    public interface IEmailConfirmationsRepository<TEntity> : IRepository<TEntity> where TEntity : IEmailConfirmation
    {
        Task<IRepositoryResponse> GetByToken(string token);
    }
}

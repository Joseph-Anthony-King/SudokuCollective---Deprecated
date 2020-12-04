using System.Threading.Tasks;
using SudokuCollective.Core.Interfaces.DataModels;
using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Core.Interfaces.Repositories
{
    public interface IGamesRepository<TEntity> : IRepository<TEntity> where TEntity : IGame
    {
        Task<IRepositoryResponse> GetAppGame(int id, int appid, bool fullRecord = true);
        Task<IRepositoryResponse> GetAppGames(int appid, bool fullRecord = true);
        Task<IRepositoryResponse> GetMyGame(int userid, int gameid, int appid, bool fullRecord = true);
        Task<IRepositoryResponse> GetMyGames(int userid, int appid, bool fullRecord = true);
        Task<IRepositoryResponse> DeleteMyGame(int userid, int gameid, int appid);
    }
}

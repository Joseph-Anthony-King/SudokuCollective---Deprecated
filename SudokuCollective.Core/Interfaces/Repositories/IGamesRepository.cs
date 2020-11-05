using System.Threading.Tasks;
using SudokuCollective.Core.Interfaces.DataModels;
using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Core.Interfaces.Repositories
{
    public interface IGamesRepository<TEntity> : IRepository<TEntity> where TEntity : IGame
    {
        Task<IRepositoryResponse> GetGame(int id, int appid, bool fullRecord = false);
        Task<IRepositoryResponse> GetGames(int appid, bool fullRecord = false);
        Task<IRepositoryResponse> GetMyGame(int userid, int gameid, int appid, bool fullRecord = false);
        Task<IRepositoryResponse> GetMyGames(int userid, int appid, bool fullRecord = false);
        Task<IRepositoryResponse> DeleteMyGame(int userid, int gameid, int appid);
    }
}

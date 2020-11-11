using System.Threading.Tasks;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;

namespace SudokuCollective.Core.Interfaces.Services
{
    public interface IGamesService : IService
    {
        Task<IGameResult> CreateGame(ICreateGameRequest createGameRequest, bool fullRecord = true);
        Task<IGameResult> UpdateGame(int id, IUpdateGameRequest updateGameRequest);
        Task<IBaseResult> DeleteGame(int id);
        Task<IGameResult> GetGame(int id, int appId, bool fullRecord = true);
        Task<IGamesResult> GetGames(IGetGamesRequest getGamesRequest, bool fullRecord = true);
        Task<IGameResult> GetMyGame(int gameid, IGetGamesRequest getMyGameRequest, bool fullRecord = true);
        Task<IGamesResult> GetMyGames(IGetGamesRequest getMyGameRequest, bool fullRecord = true);
        Task<IBaseResult> DeleteMyGame(int gameid, IGetGamesRequest getMyGameRequest);
        Task<IGameResult> CheckGame(int id, IUpdateGameRequest updateGameRequest);
    }
}

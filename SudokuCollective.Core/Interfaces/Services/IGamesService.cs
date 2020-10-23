using System.Threading.Tasks;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;

namespace SudokuCollective.Core.Interfaces.Services
{
    public interface IGamesService
    {
        Task<IGameResult> CreateGame(ICreateGameRequest createGameRequest, bool fullRecord = false);
        Task<IGameResult> UpdateGame(int id, IUpdateGameRequest updateGameRequest);
        Task<IBaseResult> DeleteGame(int id);
        Task<IGameResult> GetGame(int id, int appId);
        Task<IGamesResult> GetGames(IBaseRequest baseRequest, bool fullRecord = false);
        Task<IGameResult> GetMyGame(int userId, int gameId, int appId, bool fullRecord = false);
        Task<IGamesResult> GetMyGames(int userId, IGetMyGameRequest getMyGameRequest, bool fullRecord = false);
        Task<IBaseResult> DeleteMyGame(int userId, int gameId);
        Task<IGameResult> CheckGame(int id, IUpdateGameRequest updateGameRequest);
    }
}

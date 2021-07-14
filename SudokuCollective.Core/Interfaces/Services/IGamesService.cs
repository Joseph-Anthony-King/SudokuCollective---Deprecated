using System.Collections.Generic;
using System.Threading.Tasks;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;

namespace SudokuCollective.Core.Interfaces.Services
{
    public interface IGamesService : IService
    {
        Task<IGameResult> CreateGame(ICreateGameRequest createGameRequest);
        Task<IGameResult> UpdateGame(int id, IUpdateGameRequest updateGameRequest);
        Task<IBaseResult> DeleteGame(int id);
        Task<IGameResult> GetGame(int id, int appId, bool fullRecord = true);
        Task<IGamesResult> GetGames(IGamesRequest getGamesRequest, bool fullRecord = true);
        Task<IGameResult> GetMyGame(int gameid, IGamesRequest getMyGameRequest, bool fullRecord = true);
        Task<IGamesResult> GetMyGames(IGamesRequest getMyGameRequest, bool fullRecord = true);
        Task<IBaseResult> DeleteMyGame(int gameid, IGamesRequest getMyGameRequest);
        Task<IGameResult> CheckGame(int id, IUpdateGameRequest updateGameRequest);
        Task<IAnnonymousGameResult> CreateAnnonymousGame(DifficultyLevel difficultyLevel);
        Task<IBaseResult> CheckAnnonymousGame(List<int> intList);
    }
}

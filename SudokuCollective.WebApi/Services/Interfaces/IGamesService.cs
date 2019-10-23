using System.Threading.Tasks;
using SudokuCollective.WebApi.Models.RequestModels;
using SudokuCollective.WebApi.Models.RequestModels.GameRequests;
using SudokuCollective.WebApi.Models.ResultModels;
using SudokuCollective.WebApi.Models.ResultModels.GameRequests;

namespace SudokuCollective.WebApi.Services.Interfaces {

    public interface IGamesService {

        Task<GameResult> CreateGame(CreateGameRequest createGameRequest, bool fullRecord = false);
        Task<GameResult> UpdateGame(int id, UpdateGameRequest updateGameRequest);
        Task<BaseResult> DeleteGame(int id);
        Task<GameResult> GetGame(int id);
        Task<GamesResult> GetGames(BaseRequest baseRequest, bool fullRecord = false);
        Task<GameResult> GetMyGame(int userId, int gameId, bool fullRecord = false);
        Task<GamesResult> GetMyGames(int userId, GetMyGameRequest getMyGameRequest, bool fullRecord = false);
        Task<BaseResult> DeleteMyGame(int userId, int gameId);
        Task<GameResult> CheckGame(int id, UpdateGameRequest updateGameRequest);
    }
}

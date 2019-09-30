using System.Threading.Tasks;
using SudokuCollective.WebApi.Models.RequestModels.GameRequests;
using SudokuCollective.WebApi.Models.TaskModels.GameRequests;

namespace SudokuCollective.WebApi.Services.Interfaces {

    public interface IGamesService {

        Task<GameTaskResult> CreateGame(CreateGameRO createGameRO);
        Task<GameTaskResult> UpdateGame(int id, UpdateGameRO updateGameRO);
        Task<bool> DeleteGame(int id);
        Task<GameTaskResult> GetGame(int id);
        Task<GameListTaskResult> GetGames(bool fullRecord = true);
        Task<GameTaskResult> GetMyGame(int userId, int gameId, bool fullRecord = true);
        Task<GameListTaskResult> GetMyGames(int userId, bool fullRecord = true);
        Task<bool> DeleteMyGame(int userId, int gameId);
        Task<GameTaskResult> CheckGame(int id, UpdateGameRO checkGameRO);
    }
}

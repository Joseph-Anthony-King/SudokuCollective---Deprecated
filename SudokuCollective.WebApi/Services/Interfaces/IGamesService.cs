using System.Threading.Tasks;
using SudokuCollective.WebApi.Models.RequestModels;
using SudokuCollective.WebApi.Models.RequestModels.GameRequests;
using SudokuCollective.WebApi.Models.TaskModels;
using SudokuCollective.WebApi.Models.TaskModels.GameRequests;

namespace SudokuCollective.WebApi.Services.Interfaces {

    public interface IGamesService {

        Task<GameTaskResult> CreateGame(CreateGameRO createGameRO, bool fullRecord = true);
        Task<GameTaskResult> UpdateGame(int id, UpdateGameRO updateGameRO);
        Task<BaseTaskResult> DeleteGame(int id);
        Task<GameTaskResult> GetGame(int id);
        Task<GameListTaskResult> GetGames(BaseRequestRO baseRequestRO, bool fullRecord = true);
        Task<GameTaskResult> GetMyGame(int userId, int gameId, bool fullRecord = true);
        Task<GameListTaskResult> GetMyGames(int userId, GetMyGameRO getMyGameRO, bool fullRecord = true);
        Task<BaseTaskResult> DeleteMyGame(int userId, int gameId);
        Task<GameTaskResult> CheckGame(int id, UpdateGameRO checkGameRO);
    }
}

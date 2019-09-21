using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Models;
using SudokuCollective.WebApi.Models.RequestObjects.GameRequests;

namespace SudokuCollective.WebApi.Services.Interfaces {

    public interface IGamesService {

        Task<Game> CreateGame(CreateGameRO createGameRO);
        Task<bool> UpdateGame(int id, UpdateGameRO updateGameRO);
        Task<Game> DeleteGame(int id);
        Task<Game> GetGame(int id);
        Task<ActionResult<IEnumerable<Game>>> GetGames(bool fullRecord = true);
        Task<Game> GetMyGame(int userId, int gameId, bool fullRecord = true);
        Task<ActionResult<IEnumerable<Game>>> GetMyGames(int userId, bool fullRecord = true);
        Task<Game> DeleteMyGame(int userId, int gameId);
        Task<Game> CheckGame(UpdateGameRO checkGameRO);
        bool IsGameValid(Game game);
    }
}

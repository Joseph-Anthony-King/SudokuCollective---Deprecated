using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SudokuApp.Models;
using SudokuApp.WebApp.Models.RequestObjects.GameRequests;

namespace SudokuApp.WebApp.Services.Interfaces {

    public interface IGamesService {

        Task<Game> CreateGame(CreateGameRO createGameRO);
        Task UpdateGame(int id, UpdateGameRO updateGameRO);
        Task<Game> DeleteGame(int id);
        Task<ActionResult<Game>> GetGame(int id);
        Task<ActionResult<IEnumerable<Game>>> GetGames(bool fullRecord = true);
        Task<ActionResult<Game>> GetMyGame(int userId, int gameId, bool fullRecord = true);
        Task<ActionResult<IEnumerable<Game>>> GetMyGames(int userId, bool fullRecord = true);
        Task<Game> DeleteMyGame(int userId, int gameId);
        Task<Game> CheckGame(UpdateGameRO checkGameRO);
    }
}

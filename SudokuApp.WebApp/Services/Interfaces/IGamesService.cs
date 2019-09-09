using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SudokuApp.Models;
using SudokuApp.WebApp.Models.RequestObjects.GameRequests;

namespace SudokuApp.WebApp.Services.Interfaces {

    public interface IGamesService {

        Task<Game> CreateGame(CreateGameRO createGameRO);
        Task UpdateGame(int id, Game game);
        Task<Game> DeleteGame(int id);
        Task<ActionResult<Game>> GetGame(int id);
        Task<ActionResult<IEnumerable<Game>>> GetGames();
        Task<ActionResult<Game>> GetMyGame(int userId, int gameId);
        Task<ActionResult<IEnumerable<Game>>> GetMyGames(int userId);
        Task<Game> DeleteMyGame(int userId, int gameId);
    }
}

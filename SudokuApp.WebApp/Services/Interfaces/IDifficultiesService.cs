using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SudokuApp.Models;
using SudokuApp.Models.Enums;

namespace SudokuApp.WebApp.Services.Interfaces {
    
    public interface IDifficultiesService {

        Task<ActionResult<Difficulty>> GetDifficulty(int id);
        Task<ActionResult<IEnumerable<Difficulty>>> GetDifficulties();
        Task<Difficulty> CreateDifficulty(string name, DifficultyLevel difficultyLevel);
        Task UpdateDifficulty(int id, Difficulty difficulty);
        Task<Difficulty> DeleteDifficulty(int id);
    }
}

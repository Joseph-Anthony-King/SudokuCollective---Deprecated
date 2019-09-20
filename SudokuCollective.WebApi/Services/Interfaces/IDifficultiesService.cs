using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Models;
using SudokuCollective.Models.Enums;

namespace SudokuCollective.WebApi.Services.Interfaces {
    
    public interface IDifficultiesService {

        Task<ActionResult<Difficulty>> GetDifficulty(int id, bool fullRecord = true);
        Task<ActionResult<IEnumerable<Difficulty>>> GetDifficulties(bool fullRecord = true);
        Task<Difficulty> CreateDifficulty(string name, DifficultyLevel difficultyLevel);
        Task UpdateDifficulty(int id, Difficulty difficulty);
        Task<Difficulty> DeleteDifficulty(int id);
    }
}

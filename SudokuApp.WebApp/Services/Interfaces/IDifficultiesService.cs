using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SudokuApp.Models;

namespace SudokuApp.WebApp.Services.Interfaces {
    
    public interface IDifficultiesService {

        Task<ActionResult<Difficulty>> GetDifficulty(int id);
        Task<ActionResult<IEnumerable<Difficulty>>> GetDifficulties();
        Task UpdateDifficulty(int id, Difficulty difficulty);
        Task<Difficulty> DeleteDifficulty(int id);
    }
}

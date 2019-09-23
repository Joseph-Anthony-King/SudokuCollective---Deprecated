using System.Threading.Tasks;
using SudokuCollective.Models;
using SudokuCollective.Models.Enums;
using SudokuCollective.WebApi.Models.TaskModels.DifficultyRequests;

namespace SudokuCollective.WebApi.Services.Interfaces {

    public interface IDifficultiesService {

        Task<DifficultyTaskResult> GetDifficulty(int id, bool fullRecord = true);
        Task<DifficultyListTaskResult> GetDifficulties(bool fullRecord = true);
        Task<DifficultyTaskResult> CreateDifficulty(string name, DifficultyLevel difficultyLevel);
        Task<bool> UpdateDifficulty(int id, Difficulty difficulty);
        Task<bool> DeleteDifficulty(int id);
    }
}

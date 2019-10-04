using System.Threading.Tasks;
using SudokuCollective.Models.Enums;
using SudokuCollective.WebApi.Models.RequestModels.DifficultyRequests;
using SudokuCollective.WebApi.Models.TaskModels;
using SudokuCollective.WebApi.Models.TaskModels.DifficultyRequests;

namespace SudokuCollective.WebApi.Services.Interfaces {

    public interface IDifficultiesService {

        Task<DifficultyTaskResult> GetDifficulty(int id, bool fullRecord = false);
        Task<DifficultyListTaskResult> GetDifficulties(bool fullRecord = false);
        Task<DifficultyTaskResult> CreateDifficulty(string name, DifficultyLevel difficultyLevel);
        Task<BaseTaskResult> UpdateDifficulty(int id, UpdateDifficultyRO updateDifficultyRO);
        Task<BaseTaskResult> DeleteDifficulty(int id);
    }
}

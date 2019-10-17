using System.Threading.Tasks;
using SudokuCollective.Domain.Enums;
using SudokuCollective.WebApi.Models.RequestModels.DifficultyRequests;
using SudokuCollective.WebApi.Models.ResultModels;
using SudokuCollective.WebApi.Models.ResultModels.DifficultyRequests;

namespace SudokuCollective.WebApi.Services.Interfaces {

    public interface IDifficultiesService {

        Task<DifficultyResult> GetDifficulty(int id, bool fullRecord = false);
        Task<DifficultiesResult> GetDifficulties(bool fullRecord = false);
        Task<DifficultyResult> CreateDifficulty(string name, DifficultyLevel difficultyLevel);
        Task<BaseResult> UpdateDifficulty(int id, UpdateDifficultyRequest updateDifficultyRO);
        Task<BaseResult> DeleteDifficulty(int id);
    }
}

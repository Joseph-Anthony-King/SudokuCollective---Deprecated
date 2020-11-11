using System.Threading.Tasks;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;

namespace SudokuCollective.Core.Interfaces.Services
{
    public interface IDifficultiesService : IService
    {
        Task<IDifficultyResult> GetDifficulty(int id, bool fullRecord = true);
        Task<IDifficultiesResult> GetDifficulties(bool fullRecord = true);
        Task<IDifficultyResult> CreateDifficulty(string name, DifficultyLevel difficultyLevel);
        Task<IBaseResult> UpdateDifficulty(int id, IUpdateDifficultyRequest updateDifficultyRO);
        Task<IBaseResult> DeleteDifficulty(int id);
    }
}

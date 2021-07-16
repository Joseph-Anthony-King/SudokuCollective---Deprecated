using System.Threading.Tasks;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;

namespace SudokuCollective.Core.Interfaces.Services
{
    public interface IDifficultiesService : IService
    {
        Task<IDifficultyResult> Create(string name, string displayName, DifficultyLevel difficultyLevel);
        Task<IDifficultyResult> Get(int id);
        Task<IBaseResult> Update(int id, IUpdateDifficultyRequest updateDifficultyRO);
        Task<IBaseResult> Delete(int id);
        Task<IDifficultiesResult> GetDifficulties();
    }
}

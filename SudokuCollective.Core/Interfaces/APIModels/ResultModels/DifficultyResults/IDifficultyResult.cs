using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Core.Interfaces.APIModels.ResultModels
{
    public interface IDifficultyResult : IBaseResult
    {
        IDifficulty Difficulty { get; set; }
    }
}

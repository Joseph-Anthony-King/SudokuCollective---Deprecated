using SudokuCollective.Core.Enums;

namespace SudokuCollective.Core.Interfaces.APIModels.RequestModels.GameRequests
{
    public interface IAnnonymousGameRequest
    {
        DifficultyLevel DifficultyLevel { get; set; }
    }
}

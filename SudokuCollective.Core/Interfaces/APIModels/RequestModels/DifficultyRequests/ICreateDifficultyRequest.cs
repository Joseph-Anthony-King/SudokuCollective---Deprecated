using SudokuCollective.Core.Enums;

namespace SudokuCollective.Core.Interfaces.APIModels.RequestModels
{
    public interface ICreateDifficultyRequest : IBaseRequest
    {
        string Name { get; set; }
        DifficultyLevel DifficultyLevel { get; set; }
    }
}

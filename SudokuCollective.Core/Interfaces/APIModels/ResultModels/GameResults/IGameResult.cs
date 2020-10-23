using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Core.Interfaces.APIModels.ResultModels
{
    public interface IGameResult : IBaseResult
    {
        IGame Game { get; set; }
    }
}

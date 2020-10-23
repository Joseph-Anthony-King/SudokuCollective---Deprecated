using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Core.Interfaces.APIModels.ResultModels
{
    public interface ISolutionResult : IBaseResult
    {
        ISudokuSolution Solution { get; set; }
    }
}

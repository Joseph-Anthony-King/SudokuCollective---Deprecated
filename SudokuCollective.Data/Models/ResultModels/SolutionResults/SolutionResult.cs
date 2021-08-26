using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Models;

namespace SudokuCollective.Data.Models.ResultModels
{
    public class SolutionResult : ISolutionResult
    {
        public bool IsSuccess { get; set; }
        public bool IsFromCache { get; set; }
        public string Message { get; set; }
        public ISudokuSolution Solution { get; set; }

        public SolutionResult() : base()
        {
            IsSuccess = false;
            IsFromCache = false;
            Message = string.Empty;
            Solution = new SudokuSolution();
        }
    }
}

using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Models;

namespace SudokuCollective.Data.Models.ResultModels
{
    public class DifficultyResult : IDifficultyResult
    {
        public bool IsSuccess { get; set; }
        public bool IsFromCache { get; set; }
        public string Message { get; set; }
        public IDifficulty Difficulty { get; set; }

        public DifficultyResult() : base()
        {
            IsSuccess = false;
            IsFromCache = false;
            Message = string.Empty;
            Difficulty = new Difficulty();
        }
    }
}

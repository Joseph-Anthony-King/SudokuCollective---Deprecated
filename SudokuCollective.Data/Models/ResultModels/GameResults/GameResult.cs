using SudokuCollective.Core.Models;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;

namespace SudokuCollective.Data.Models.ResultModels
{
    public class GameResult : IGameResult
    {
        public bool IsSuccess { get; set; }
        public bool FromCache { get; set; }
        public string Message { get; set; }
        public IGame Game { get; set; }

        public GameResult() : base()
        {
            IsSuccess = false;
            FromCache = false;
            Message = string.Empty;
            Game = new Game();
        }
    }
}

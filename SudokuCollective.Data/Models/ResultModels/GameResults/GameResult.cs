using SudokuCollective.Core.Models;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;

namespace SudokuCollective.Data.Models.ResultModels
{
    public class GameResult : IGameResult
    {
        public bool Success { get; set; }
        public bool FromCache { get; set; }
        public string Message { get; set; }
        public IGame Game { get; set; }

        public GameResult() : base()
        {
            Success = false;
            FromCache = false;
            Message = string.Empty;
            Game = new Game();
        }
    }
}

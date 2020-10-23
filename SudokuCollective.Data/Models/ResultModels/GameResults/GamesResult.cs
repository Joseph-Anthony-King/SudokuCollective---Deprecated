using System.Collections.Generic;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Data.Models.ResultModels
{
    public class GamesResult : IGamesResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<IGame> Games { get; set; }

        public GamesResult() : base()
        {
            Games = new List<IGame>();
        }
    }
}

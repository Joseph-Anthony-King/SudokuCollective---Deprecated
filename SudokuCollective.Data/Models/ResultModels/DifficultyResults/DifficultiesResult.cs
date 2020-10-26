using System.Collections.Generic;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Data.Models.ResultModels
{
    public class DifficultiesResult : IDifficultiesResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<IDifficulty> Difficulties { get; set; }

        public DifficultiesResult() : base()
        {
            Difficulties = new List<IDifficulty>();
        }
    }
}

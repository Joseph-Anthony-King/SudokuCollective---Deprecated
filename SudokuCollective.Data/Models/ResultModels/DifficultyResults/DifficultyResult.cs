using System.Collections.Generic;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Models;

namespace SudokuCollective.Data.Models.ResultModels
{
    public class DifficultyResult : IDifficultyResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public IDifficulty Difficulty { get; set; }

        public DifficultyResult() : base()
        {
            Difficulty = new Difficulty()
            {
                Id = 0,
                Name = string.Empty,
                DifficultyLevel = DifficultyLevel.NULL,
                Matrices = new List<SudokuMatrix>()
            };
        }
    }
}

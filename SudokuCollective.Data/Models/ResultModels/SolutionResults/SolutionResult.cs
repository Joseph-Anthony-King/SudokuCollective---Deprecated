using System.Collections.Generic;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Domain;
using SudokuCollective.Domain.Models;

namespace SudokuCollective.Data.Models.ResultModels
{
    public class SolutionResult : ISolutionResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ISudokuSolution Solution { get; set; }

        public SolutionResult() : base()
        {
            Solution = new SudokuSolution()
            {
                Id = 0,
                SolutionList = new List<int>(),
                Game = new Game()
            };
        }
    }
}

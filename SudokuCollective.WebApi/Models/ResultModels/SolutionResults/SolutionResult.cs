using System.Collections.Generic;
using SudokuCollective.Domain;
using SudokuCollective.Domain.Models;

namespace SudokuCollective.WebApi.Models.ResultModels.SolutionRequests {

    public class SolutionResult : BaseResult {
        
        public SudokuSolution Solution { get; set; }

        public SolutionResult() : base() {

            Solution = new SudokuSolution() {

                Id = 0,
                SolutionList = new List<int>(),
                Game = new Game() {

                    Id = 0,
                    UserId = 0,
                    SudokuMatrixId = 0
                }
            };
        }
    }
}

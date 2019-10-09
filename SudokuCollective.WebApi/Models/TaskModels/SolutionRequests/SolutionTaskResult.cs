using System.Collections.Generic;
using SudokuCollective.Domain;

namespace SudokuCollective.WebApi.Models.TaskModels.SolutionRequests {

    public class SolutionTaskResult : BaseTaskResult {
        
        public SudokuSolution Solution { get; set; }

        public SolutionTaskResult() : base() {

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

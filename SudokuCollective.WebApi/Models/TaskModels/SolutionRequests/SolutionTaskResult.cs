using SudokuCollective.Models;

namespace SudokuCollective.WebApi.Models.TaskModels.SolutionRequests {

    public class SolutionTaskResult {

        public bool Result { get; set; }
        public SudokuSolution Solution { get; set; }
    }
}

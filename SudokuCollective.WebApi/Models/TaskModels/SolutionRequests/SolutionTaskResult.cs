using SudokuCollective.Models;

namespace SudokuCollective.WebApi.Models.TaskModels.SolutionRequests {

    public class SolutionTaskResult : BaseTaskResult {
        
        public SudokuSolution Solution { get; set; }
    }
}

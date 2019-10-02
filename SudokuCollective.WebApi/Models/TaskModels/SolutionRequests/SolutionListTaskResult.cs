using System.Collections.Generic;
using SudokuCollective.Models;

namespace SudokuCollective.WebApi.Models.TaskModels.SolutionRequests {

    public class SolutionListTaskResult : BaseTaskResult {
        
        public IEnumerable<SudokuSolution> Solutions { get; set; }
    }
}

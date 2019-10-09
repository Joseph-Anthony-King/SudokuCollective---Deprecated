using System.Collections.Generic;
using SudokuCollective.Domain;

namespace SudokuCollective.WebApi.Models.TaskModels.SolutionRequests {

    public class SolutionListTaskResult : BaseTaskResult {
        
        public IEnumerable<SudokuSolution> Solutions { get; set; }

        public SolutionListTaskResult() : base() {

            Solutions = new List<SudokuSolution>();
        }
    }
}

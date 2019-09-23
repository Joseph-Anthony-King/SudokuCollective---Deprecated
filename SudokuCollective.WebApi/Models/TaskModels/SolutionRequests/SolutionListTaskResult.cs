using System.Collections.Generic;
using SudokuCollective.Models;

namespace SudokuCollective.WebApi.Models.TaskModels.SolutionRequests {

    public class SolutionListTaskResult {

        public bool Result { get; set; }
        public IEnumerable<SudokuSolution> Solutions { get; set; }
    }
}

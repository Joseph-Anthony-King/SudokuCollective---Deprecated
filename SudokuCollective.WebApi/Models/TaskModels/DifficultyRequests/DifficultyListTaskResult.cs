using SudokuCollective.Models;
using System.Collections.Generic;

namespace SudokuCollective.WebApi.Models.TaskModels.DifficultyRequests {

    public class DifficultyListTaskResult : BaseTaskResult {
        
        public IEnumerable<Difficulty> Difficulties { get; set; }
    }
}

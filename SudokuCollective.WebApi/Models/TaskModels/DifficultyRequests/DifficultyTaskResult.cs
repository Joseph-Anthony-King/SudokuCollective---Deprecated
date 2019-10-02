using SudokuCollective.Models;

namespace SudokuCollective.WebApi.Models.TaskModels.DifficultyRequests {

    public class DifficultyTaskResult : BaseTaskResult {
        
        public Difficulty Difficulty { get; set; }
    }
}

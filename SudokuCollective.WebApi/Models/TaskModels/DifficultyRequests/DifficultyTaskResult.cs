using SudokuCollective.Models;

namespace SudokuCollective.WebApi.Models.TaskModels.DifficultyRequests {

    public class DifficultyTaskResult {

        public bool Result { get; set; }
        public Difficulty Difficulty { get; set; }
    }
}

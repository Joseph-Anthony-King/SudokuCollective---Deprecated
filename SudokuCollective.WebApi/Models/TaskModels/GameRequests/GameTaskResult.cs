using SudokuCollective.Models;

namespace SudokuCollective.WebApi.Models.TaskModels.GameRequests {

    public class GameTaskResult : BaseTaskResult {
        
        public Game Game { get; set; }
    }
}

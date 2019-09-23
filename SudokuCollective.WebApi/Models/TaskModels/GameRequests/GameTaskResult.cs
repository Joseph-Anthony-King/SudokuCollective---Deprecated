using SudokuCollective.Models;

namespace SudokuCollective.WebApi.Models.TaskModels.GameRequests {

    public class GameTaskResult {

        public bool Result { get; set; }
        public Game Game { get; set; }
    }
}

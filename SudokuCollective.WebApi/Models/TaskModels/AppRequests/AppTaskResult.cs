using SudokuCollective.Models;

namespace SudokuCollective.WebApi.Models.TaskModels.AppRequests {

    public class AppTaskResult {

        public bool Result { get; set; }
        public App App { get; set; }
    }
}

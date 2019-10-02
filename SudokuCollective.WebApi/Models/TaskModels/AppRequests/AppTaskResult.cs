using SudokuCollective.Models;

namespace SudokuCollective.WebApi.Models.TaskModels.AppRequests {

    public class AppTaskResult : BaseTaskResult {
        
        public App App { get; set; }
    }
}

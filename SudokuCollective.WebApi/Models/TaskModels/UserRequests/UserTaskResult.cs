using SudokuCollective.Models;

namespace SudokuCollective.WebApi.Models.TaskModels.UserRequests {

    public class UserTaskResult : BaseTaskResult {
        
        public User User { get; set; }
    }
}

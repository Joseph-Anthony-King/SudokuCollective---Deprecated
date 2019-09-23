using SudokuCollective.Models;

namespace SudokuCollective.WebApi.Models.TaskModels.UserRequests {

    public class UserTaskResult {

        public bool Result { get; set; }
        public User User { get; set; }
    }
}

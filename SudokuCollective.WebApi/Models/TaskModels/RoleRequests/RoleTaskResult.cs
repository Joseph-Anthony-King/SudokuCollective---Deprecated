using SudokuCollective.Models;

namespace SudokuCollective.WebApi.Models.TaskModels.RoleRequests {

    public class RoleTaskResult : BaseTaskResult {
        
        public Role Role { get; set; }
    }
}
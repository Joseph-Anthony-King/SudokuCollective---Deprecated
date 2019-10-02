using System.Collections.Generic;
using SudokuCollective.Models;

namespace SudokuCollective.WebApi.Models.TaskModels.UserRequests {

    public class UserListTaskResult : BaseTaskResult {
        
        public IEnumerable<User> Users { get; set; }
    }
}

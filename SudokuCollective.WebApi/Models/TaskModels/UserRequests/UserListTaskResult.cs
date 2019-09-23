using System.Collections.Generic;
using SudokuCollective.Models;

namespace SudokuCollective.WebApi.Models.TaskModels.UserRequests {

    public class UserListTaskResult {

        public bool Result { get; set; }
        public IEnumerable<User> Users { get; set; }
    }
}

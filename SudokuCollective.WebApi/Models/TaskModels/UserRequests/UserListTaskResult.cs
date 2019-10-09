using System.Collections.Generic;
using SudokuCollective.Domain;

namespace SudokuCollective.WebApi.Models.TaskModels.UserRequests {

    public class UserListTaskResult : BaseTaskResult {
        
        public IEnumerable<User> Users { get; set; }

        public UserListTaskResult() : base() {

            Users = new List<User>();
        }
    }
}

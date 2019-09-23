using System.Collections.Generic;
using SudokuCollective.Models;

namespace SudokuCollective.WebApi.Models.TaskModels.RoleRequests {

    public class RoleListTaskResult {

        public bool Result { get; set; }
        public IEnumerable<Role> Roles { get; set; }
    }
}

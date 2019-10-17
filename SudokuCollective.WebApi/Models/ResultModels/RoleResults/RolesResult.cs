using System.Collections.Generic;
using SudokuCollective.Domain.Models;

namespace SudokuCollective.WebApi.Models.ResultModels.RoleRequests {

    public class RolesResult : BaseResult {
        
        public IEnumerable<Role> Roles { get; set; }

        public RolesResult() : base() {

            Roles = new List<Role>();
        }
    }
}

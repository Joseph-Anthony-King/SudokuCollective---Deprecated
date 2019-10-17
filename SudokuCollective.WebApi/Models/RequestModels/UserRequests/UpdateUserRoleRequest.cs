using System.Collections.Generic;

namespace SudokuCollective.WebApi.Models.RequestModels.UserRequests {
    
    public class UpdateUserRoleRequest : BaseRequest {

        public List<int> RoleIds { get; set; }

        public UpdateUserRoleRequest() : base() {

            RoleIds = new List<int>();
        }
    }
}

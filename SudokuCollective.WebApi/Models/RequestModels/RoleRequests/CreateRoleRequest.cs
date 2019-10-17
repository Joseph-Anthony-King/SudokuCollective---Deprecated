using SudokuCollective.Domain.Enums;

namespace SudokuCollective.WebApi.Models.RequestModels.RoleRequests {
    
    public class CreateRoleRequest : BaseRequest {

        public string Name { get; set; }
        public RoleLevel RoleLevel { get; set; }

        public CreateRoleRequest() : base() {

            Name = string.Empty;
            RoleLevel = RoleLevel.NULL;
        }
    }
}

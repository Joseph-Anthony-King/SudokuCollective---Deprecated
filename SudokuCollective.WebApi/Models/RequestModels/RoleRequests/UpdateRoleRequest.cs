using SudokuCollective.Domain.Enums;

namespace SudokuCollective.WebApi.Models.RequestModels.RoleRequests {

    public class UpdateRoleRequest : BaseRequest {

        public int Id { get; set; }
        public string Name { get; set; }
        public RoleLevel RoleLevel { get; set; }

        public UpdateRoleRequest() : base() {

            Id = 0;
            Name = string.Empty;
            RoleLevel = RoleLevel.NULL;
        }
    }
}

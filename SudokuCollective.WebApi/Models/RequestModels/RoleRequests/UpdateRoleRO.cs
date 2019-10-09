using SudokuCollective.Domain.Enums;

namespace SudokuCollective.WebApi.Models.RequestModels.RoleRequests {

    public class UpdateRoleRO : BaseRequestRO {

        public int Id { get; set; }
        public string Name { get; set; }
        public RoleLevel RoleLevel { get; set; }
    }
}

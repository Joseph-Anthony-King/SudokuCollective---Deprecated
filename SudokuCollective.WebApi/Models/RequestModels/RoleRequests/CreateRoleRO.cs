using SudokuCollective.Models.Enums;

namespace SudokuCollective.WebApi.Models.RequestModels.RoleRequests {
    
    public class CreateRoleRO {

        public string Name { get; set; }
        public RoleLevel RoleLevel { get; set; }
    }
}
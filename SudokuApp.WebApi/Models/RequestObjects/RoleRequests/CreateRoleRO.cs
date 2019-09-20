using SudokuApp.Models.Enums;

namespace SudokuApp.WebApi.Models.RequestObjects.RoleRequests {
    
    public class CreateRoleRO {

        public string Name { get; set; }
        public RoleLevel RoleLevel { get; set; }
    }
}
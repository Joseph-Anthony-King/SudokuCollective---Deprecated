using SudokuApp.Models.Enums;

namespace SudokuApp.WebApp.Models.RequestObjects {
    
    public class CreateRoleRO {

        public string Name { get; set; }
        public RoleLevel RoleLevel { get; set; }
    }
}
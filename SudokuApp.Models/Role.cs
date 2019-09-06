using System.Collections.Generic;
using SudokuApp.Models.Enums;

namespace SudokuApp.Models {

    public class Role {

        public int Id { get; set; }
        public string Name { get; set; }
        public RoleLevel RoleLevel { get; set; }
        public ICollection<UserRole> Users { get; set; }
    }
}

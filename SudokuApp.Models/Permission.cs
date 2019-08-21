using System.Collections.Generic;
using SudokuApp.Models.Enums;

namespace SudokuApp.Models {

    public class Permission {

        public int Id { get; set; }
        public string Name { get; set; }
        public PermissionLevel PermissionLevel { get; set; }
        public ICollection<UserPermission> Users { get; set; }
    }
}

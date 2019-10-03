using System.Collections.Generic;
using Newtonsoft.Json;
using SudokuCollective.Models.Enums;
using SudokuCollective.Models.Interfaces;

namespace SudokuCollective.Models {

    public class Role : ISudokuOject {

        public int Id { get; set; }
        public string Name { get; set; }
        public RoleLevel RoleLevel { get; set; }
        public ICollection<UserRole> Users { get; set; }

        public Role() {
            
            Id = 0;
            Name = string.Empty;
            RoleLevel = RoleLevel.NULL;
        }

        [JsonConstructor]
        public Role(int id, string name, RoleLevel roleLevel) : this() {
            
            Id = id;
            Name = name;
            RoleLevel = roleLevel;
        }
    }
}

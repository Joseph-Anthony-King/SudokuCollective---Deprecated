using Newtonsoft.Json;
using SudokuCollective.Domain.Interfaces;

namespace SudokuCollective.Domain {

    public class UserRole : IEntityBase {

        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

        public UserRole() {

            UserId = 0;
            User = null;
            RoleId = 0;
            Role = null;
        }

        [JsonConstructor]
        public UserRole(
            int id, 
            int userId, 
            int roleId) : this() {

            Id = id;
            UserId = userId;
            RoleId = roleId;
        }
    }
}

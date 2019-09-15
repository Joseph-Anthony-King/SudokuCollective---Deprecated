using Newtonsoft.Json;

namespace SudokuApp.Models {

    public class UserRole {

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
        public UserRole(int userId, int roleId) : this() {

            UserId = userId;
            RoleId = roleId;
        }
    }
}

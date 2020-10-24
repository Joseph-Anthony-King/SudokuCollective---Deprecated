using Newtonsoft.Json;
using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Domain.Models
{
    public class UserRole : IUserRole
    {
        #region Properties
        public int Id { get; set; }
        public int UserId { get; set; }
        public IUser User { get; set; }
        public int RoleId { get; set; }
        public IRole Role { get; set; }
        #endregion

        #region Constructors
        public UserRole()
        {
            Id = 0;
            UserId = 0;
            User = null;
            RoleId = 0;
            Role = null;
        }

        [JsonConstructor]
        public UserRole(
            int id,
            int userId,
            int roleId)
        {
            Id = id;
            UserId = userId;
            RoleId = roleId;
        }
        #endregion

        #region Methods
        public UserRole Convert(IUserRole userRole)
        {
            return (UserRole)userRole;
        }
        #endregion
    }
}

using SudokuCollective.Core.Models;

namespace SudokuCollective.Core.Interfaces.Models
{
    public interface IUserRole : IEntityBase
    {
        int UserId { get; set; }
        User User { get; set; }
        int RoleId { get; set; }
        Role Role { get; set; }
    }
}

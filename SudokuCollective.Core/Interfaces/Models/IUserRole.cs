namespace SudokuCollective.Core.Interfaces.Models
{
    public interface IUserRole : IEntityBase
    {
        int UserId { get; set; }
        IUser User { get; set; }
        int RoleId { get; set; }
        IRole Role { get; set; }
    }
}

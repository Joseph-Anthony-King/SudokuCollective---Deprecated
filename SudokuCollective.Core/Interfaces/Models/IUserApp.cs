namespace SudokuCollective.Core.Interfaces.Models
{
    public interface IUserApp : IEntityBase
    {
        int UserId { get; set; }
        IUser User { get; set; }
        int AppId { get; set; }
        IApp App { get; set; }
    }
}

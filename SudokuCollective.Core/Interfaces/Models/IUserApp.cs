using SudokuCollective.Core.Models;

namespace SudokuCollective.Core.Interfaces.Models
{
    public interface IUserApp : IEntityBase
    {
        int UserId { get; set; }
        User User { get; set; }
        int AppId { get; set; }
        App App { get; set; }
    }
}

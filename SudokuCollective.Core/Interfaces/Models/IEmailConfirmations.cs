namespace SudokuCollective.Core.Interfaces.Models
{
    public interface IEmailConfirmation : IEntityBase
    {
        int UserId { get; set; }
        string Code { get; set; }
    }
}

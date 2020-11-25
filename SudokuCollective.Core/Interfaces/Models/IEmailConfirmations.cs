namespace SudokuCollective.Core.Interfaces.Models
{
    public interface IEmailConfirmation : IEntityBase
    {
        int UserId { get; set; }
        int AppId { get; set; }
        string Token { get; set; }
    }
}

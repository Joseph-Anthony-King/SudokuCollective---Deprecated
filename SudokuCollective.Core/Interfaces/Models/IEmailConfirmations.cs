namespace SudokuCollective.Core.Interfaces.Models
{
    public interface IEmailConfirmation : IEntityBase
    {
        int UserId { get; set; }
        int AppId { get; set; }
        string Token { get; set; }
        string OldEmailAddress { get; set; }
        string NewEmailAddress { get; set; }
        bool OldEmailAddressConfirmed { get; set; }
        bool IsUpdate { get; }
    }
}

namespace SudokuCollective.Core.Interfaces.APIModels.RequestModels
{
    public interface IPasswordResetRequest
    {
        int UserId { get; set; }
        string NewPassword { get; set; }
    }
}

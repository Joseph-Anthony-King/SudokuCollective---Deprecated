namespace SudokuCollective.Core.Interfaces.APIModels.RequestModels
{
    public interface IUpdatePasswordRequest
    {
        int UserId { get; set; }
        string NewPassword { get; set; }
    }
}

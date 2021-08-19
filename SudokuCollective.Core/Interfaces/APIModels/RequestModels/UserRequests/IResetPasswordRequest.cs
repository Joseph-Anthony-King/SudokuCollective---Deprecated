namespace SudokuCollective.Core.Interfaces.APIModels.RequestModels
{
    public interface IResetPasswordRequest
    {
        string Token { get; set; }
        string NewPassword { get; set; }
    }
}

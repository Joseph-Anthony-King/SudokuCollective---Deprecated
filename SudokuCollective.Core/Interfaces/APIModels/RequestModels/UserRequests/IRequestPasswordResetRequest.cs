namespace SudokuCollective.Core.Interfaces.APIModels.RequestModels
{
    public interface IRequestPasswordResetRequest
    {
        string License { get; set; }
        string Email { get; set; }
    }
}

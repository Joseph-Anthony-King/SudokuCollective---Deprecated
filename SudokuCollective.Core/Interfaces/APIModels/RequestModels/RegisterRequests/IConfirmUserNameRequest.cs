namespace SudokuCollective.Core.Interfaces.APIModels.RequestModels.RegisterRequests
{
    public interface IConfirmUserNameRequest
    {
        string Email { get; set; }
        string License { get; set; }
    }
}

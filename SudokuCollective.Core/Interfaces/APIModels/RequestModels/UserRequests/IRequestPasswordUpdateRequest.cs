namespace SudokuCollective.Core.Interfaces.APIModels.RequestModels
{
    public interface IRequestPasswordUpdateRequest
    {
        string License { get; set; }
        string Email { get; set; }
    }
}

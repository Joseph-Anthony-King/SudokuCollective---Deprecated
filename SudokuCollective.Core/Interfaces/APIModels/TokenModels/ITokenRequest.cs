namespace SudokuCollective.Core.Interfaces.APIModels.TokenModels
{
    public interface ITokenRequest
    {
        string UserName { get; set; }
        string Password { get; set; }
        string License { get; set; }
    }
}

namespace SudokuCollective.Core.Interfaces.APIModels.ResultModels
{
    public interface IAuthenticationResult : IBaseResult
    {
        string UserName { get; set; }
    }
}

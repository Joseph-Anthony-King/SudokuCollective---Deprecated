namespace SudokuCollective.Core.Interfaces.APIModels.ResultModels.UserResults
{
    public interface IConfirmEmailResult : IBaseResult
    {
        string FirstName { get; set; }
        string AppTitle { get; set; }
        string Url { get; set; }
    }
}

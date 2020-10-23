namespace SudokuCollective.Core.Interfaces.APIModels.ResultModels
{
    public interface IBaseResult
    {
        bool Success { get; set; }
        string Message { get; set; }
    }
}

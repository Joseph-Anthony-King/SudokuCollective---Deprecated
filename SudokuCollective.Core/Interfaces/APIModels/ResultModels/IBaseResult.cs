namespace SudokuCollective.Core.Interfaces.APIModels.ResultModels
{
    public interface IBaseResult
    {
        bool IsSuccess { get; set; }
        bool IsFromCache { get; set; }
        string Message { get; set; }
    }
}

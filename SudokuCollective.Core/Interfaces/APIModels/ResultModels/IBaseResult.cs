namespace SudokuCollective.Core.Interfaces.APIModels.ResultModels
{
    public interface IBaseResult
    {
        bool IsSuccess { get; set; }
        bool FromCache { get; set; }
        string Message { get; set; }
    }
}

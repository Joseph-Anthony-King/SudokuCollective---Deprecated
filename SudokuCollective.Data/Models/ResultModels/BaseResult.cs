using SudokuCollective.Core.Interfaces.APIModels.ResultModels;

namespace SudokuCollective.Data.Models.ResultModels
{
    public class BaseResult : IBaseResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public BaseResult()
        {
            Success = false;
            Message = string.Empty;
        }
    }
}

using SudokuCollective.Core.Interfaces.APIModels.ResultModels;

namespace SudokuCollective.Data.Models.ResultModels
{
    public class BaseResult : IBaseResult
    {
        public bool IsSuccess { get; set; }
        public bool IsFromCache { get; set; }
        public string Message { get; set; }

        public BaseResult()
        {
            IsSuccess = false;
            IsFromCache = false;
            Message = string.Empty;
        }
    }
}

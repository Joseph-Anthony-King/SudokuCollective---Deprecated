using SudokuCollective.Core.Interfaces.APIModels.ResultModels;

namespace SudokuCollective.Data.Models.ResultModels
{
    public class BaseResult : IBaseResult
    {
        public bool IsSuccess { get; set; }
        public bool FromCache { get; set; }
        public string Message { get; set; }

        public BaseResult()
        {
            IsSuccess = false;
            FromCache = false;
            Message = string.Empty;
        }
    }
}

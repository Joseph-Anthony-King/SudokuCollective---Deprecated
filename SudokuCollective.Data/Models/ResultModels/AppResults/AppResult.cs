using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Models;

namespace SudokuCollective.Data.Models.ResultModels
{
    public class AppResult : IAppResult
    {
        public bool IsSuccess { get; set; }
        public bool IsFromCache { get; set; }
        public string Message { get; set; }
        public IApp App { get; set; }

        public AppResult() : base()
        {
            IsSuccess = false;
            IsFromCache = false;
            Message = string.Empty;
            App = new App();
        }
    }
}

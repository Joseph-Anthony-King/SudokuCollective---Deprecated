using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Models;

namespace SudokuCollective.Data.Models.ResultModels
{
    public class AppResult : IAppResult
    {
        public bool Success { get; set; }
        public bool FromCache { get; set; }
        public string Message { get; set; }
        public IApp App { get; set; }

        public AppResult() : base()
        {
            Success = false;
            FromCache = false;
            Message = string.Empty;
            App = new App();
        }
    }
}

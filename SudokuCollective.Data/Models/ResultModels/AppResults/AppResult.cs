using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Domain.Models;

namespace SudokuCollective.Data.Models.ResultModels
{
    public class AppResult : IAppResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public IApp App { get; set; }

        public AppResult() : base()
        {
            App = new App();
        }
    }
}

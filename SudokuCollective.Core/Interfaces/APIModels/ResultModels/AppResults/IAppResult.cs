using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Core.Interfaces.APIModels.ResultModels
{
    public interface IAppResult : IBaseResult
    {
        IApp App { get; set; }
    }
}

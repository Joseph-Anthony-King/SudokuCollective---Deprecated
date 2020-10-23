using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Core.Interfaces.APIModels.ResultModels
{
    public interface IRoleResult : IBaseResult
    {
        IRole Role { get; set; }
    }
}

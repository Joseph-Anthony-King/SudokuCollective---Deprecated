using System.Collections.Generic;
using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Core.Interfaces.APIModels.ResultModels
{
    public interface IGamesResult : IBaseResult
    {
        List<IGame> Games { get; set; }
    }
}

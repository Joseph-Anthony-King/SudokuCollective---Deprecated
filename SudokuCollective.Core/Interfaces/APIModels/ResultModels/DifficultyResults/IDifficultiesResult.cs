using System.Collections.Generic;
using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Core.Interfaces.APIModels.ResultModels
{
    public interface IDifficultiesResult : IBaseResult
    {
        List<IDifficulty> Difficulties { get; set; }
    }
}

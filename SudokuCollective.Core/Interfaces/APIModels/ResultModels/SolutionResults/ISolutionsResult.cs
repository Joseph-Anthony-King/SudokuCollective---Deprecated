using System.Collections.Generic;
using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Core.Interfaces.APIModels.ResultModels
{
    public interface ISolutionsResult : IBaseResult
    {
        List<ISudokuSolution> Solutions { get; set; }
    }
}

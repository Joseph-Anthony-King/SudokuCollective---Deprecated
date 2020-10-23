using System.Collections.Generic;
using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Core.Interfaces.APIModels.ResultModels
{
    public interface IAppsResult : IBaseResult
    {
        List<IApp> Apps { get; set; }
    }
}

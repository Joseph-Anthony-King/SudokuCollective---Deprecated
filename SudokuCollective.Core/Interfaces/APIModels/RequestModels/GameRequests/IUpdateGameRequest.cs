using System.Collections.Generic;
using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Core.Interfaces.APIModels.RequestModels
{
    public interface IUpdateGameRequest : IBaseRequest
    {
        int GameId { get; set; }
        List<ISudokuCell> SudokuCells { get; set; }
    }
}

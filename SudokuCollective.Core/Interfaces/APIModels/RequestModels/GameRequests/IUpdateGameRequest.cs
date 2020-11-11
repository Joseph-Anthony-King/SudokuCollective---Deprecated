using System.Collections.Generic;
using SudokuCollective.Core.Models;

namespace SudokuCollective.Core.Interfaces.APIModels.RequestModels
{
    public interface IUpdateGameRequest : IBaseRequest
    {
        int GameId { get; set; }
        List<SudokuCell> SudokuCells { get; set; }
    }
}

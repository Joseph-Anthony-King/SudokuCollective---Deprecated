using SudokuCollective.Models;
using System.Collections.Generic;

namespace SudokuCollective.WebApi.Models.RequestObjects.GameRequests
{

    public class UpdateGameRO {

        public int GameId { get; set; }
        public List<SudokuCell> SudokuCells { get; set; }

    }
}

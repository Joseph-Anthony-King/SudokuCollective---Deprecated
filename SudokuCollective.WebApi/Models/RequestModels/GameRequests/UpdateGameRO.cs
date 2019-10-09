using SudokuCollective.Domain;
using System.Collections.Generic;

namespace SudokuCollective.WebApi.Models.RequestModels.GameRequests {

    public class UpdateGameRO : BaseRequestRO {

        public int GameId { get; set; }
        public List<SudokuCell> SudokuCells { get; set; }

    }
}

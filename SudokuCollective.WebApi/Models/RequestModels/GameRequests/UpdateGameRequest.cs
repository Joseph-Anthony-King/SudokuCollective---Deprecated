using SudokuCollective.Domain.Models;
using System.Collections.Generic;

namespace SudokuCollective.WebApi.Models.RequestModels.GameRequests {

    public class UpdateGameRequest : BaseRequest {

        public int GameId { get; set; }
        public List<SudokuCell> SudokuCells { get; set; }

        public UpdateGameRequest() : base() {

            GameId = 0;
            SudokuCells = new List<SudokuCell>();
        }
    }
}

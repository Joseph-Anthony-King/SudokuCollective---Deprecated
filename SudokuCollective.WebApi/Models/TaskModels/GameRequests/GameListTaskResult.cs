using System.Collections.Generic;
using SudokuCollective.Models;

namespace SudokuCollective.WebApi.Models.TaskModels.GameRequests {

    public class GameListTaskResult {

        public bool Result { get; set; }
        public IEnumerable<Game> Games { get; set; }
    }
}

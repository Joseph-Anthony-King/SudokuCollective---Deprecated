using System.Collections.Generic;
using SudokuCollective.Domain;

namespace SudokuCollective.WebApi.Models.TaskModels.GameRequests {

    public class GameListTaskResult : BaseTaskResult {
        
        public IEnumerable<Game> Games { get; set; }

        public GameListTaskResult() : base() {

            Games = new List<Game>();
        }
    }
}

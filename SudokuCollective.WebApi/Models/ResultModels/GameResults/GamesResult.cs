using System.Collections.Generic;
using SudokuCollective.Domain;

namespace SudokuCollective.WebApi.Models.ResultModels.GameRequests {

    public class GamesResult : BaseResult {
        
        public List<Game> Games { get; set; }

        public GamesResult() : base() {

            Games = new List<Game>();
        }
    }
}

using SudokuCollective.Domain.Models;
using System.Collections.Generic;

namespace SudokuCollective.WebApi.Models.ResultModels.DifficultyRequests {

    public class DifficultiesResult : BaseResult {
        
        public List<Difficulty> Difficulties { get; set; }

        public DifficultiesResult() : base() {

            Difficulties = new List<Difficulty>();
        }
    }
}

using SudokuCollective.Models;
using System.Collections.Generic;

namespace SudokuCollective.WebApi.Models.TaskModels.DifficultyRequests {

    public class DifficultyListTaskResult {

        public bool Result { get; set; }
        public IEnumerable<Difficulty> Difficulties { get; set; }
    }
}

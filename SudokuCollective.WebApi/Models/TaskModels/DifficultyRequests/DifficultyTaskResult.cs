using SudokuCollective.Models;
using SudokuCollective.Models.Enums;
using System.Collections.Generic;

namespace SudokuCollective.WebApi.Models.TaskModels.DifficultyRequests {

    public class DifficultyTaskResult : BaseTaskResult {
        
        public Difficulty Difficulty { get; set; }

        public DifficultyTaskResult() : base() {

            Difficulty = new Difficulty() {

                Id = 0,
                Name = string.Empty,
                DifficultyLevel = DifficultyLevel.NULL,
                Matrices = new List<SudokuMatrix>()
            };
        }
    }
}

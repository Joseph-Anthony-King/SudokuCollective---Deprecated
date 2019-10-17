using SudokuCollective.Domain.Enums;
using SudokuCollective.Domain.Models;
using System.Collections.Generic;

namespace SudokuCollective.WebApi.Models.ResultModels.DifficultyRequests {

    public class DifficultyResult : BaseResult {
        
        public Difficulty Difficulty { get; set; }

        public DifficultyResult() : base() {

            Difficulty = new Difficulty() {

                Id = 0,
                Name = string.Empty,
                DifficultyLevel = DifficultyLevel.NULL,
                Matrices = new List<SudokuMatrix>()
            };
        }
    }
}

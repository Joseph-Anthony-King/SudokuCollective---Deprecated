using System;
using SudokuCollective.Domain;

namespace SudokuCollective.WebApi.Models.ResultModels.GameRequests {

    public class GameResult : BaseResult {
        
        public Game Game { get; set; }

        public GameResult() : base() {

            var createdDate = DateTime.UtcNow;

            Game = new Game() {

                Id = 0,
                DateCreated = createdDate,
                DateCompleted = createdDate,
                UserId = 0,
                SudokuMatrixId = 0,
                SudokuSolutionId = 0,
                ContinueGame = false
            };
        }
    }
}

using System;
using SudokuCollective.Models;

namespace SudokuCollective.WebApi.Models.TaskModels.GameRequests {

    public class GameTaskResult : BaseTaskResult {
        
        public Game Game { get; set; }

        public GameTaskResult() : base() {

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

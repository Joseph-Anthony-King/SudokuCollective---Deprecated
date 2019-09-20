using SudokuCollective.Models.Enums;

namespace SudokuCollective.WebApi.Models.RequestObjects.DifficultyRequests {

    public class CreateDifficultyRO {

        public string Name { get; set; }
        public DifficultyLevel DifficultyLevel { get; set; }
    }
}
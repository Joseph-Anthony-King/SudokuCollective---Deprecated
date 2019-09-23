using SudokuCollective.Models.Enums;

namespace SudokuCollective.WebApi.Models.RequestModels.DifficultyRequests {

    public class CreateDifficultyRO {

        public string Name { get; set; }
        public DifficultyLevel DifficultyLevel { get; set; }
    }
}
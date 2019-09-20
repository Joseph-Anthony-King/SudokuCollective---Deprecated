using SudokuApp.Models.Enums;

namespace SudokuApp.WebApi.Models.RequestObjects.DifficultyRequests {

    public class CreateDifficultyRO {

        public string Name { get; set; }
        public DifficultyLevel DifficultyLevel { get; set; }
    }
}
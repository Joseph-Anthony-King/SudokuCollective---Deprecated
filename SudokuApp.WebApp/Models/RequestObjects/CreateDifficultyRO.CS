using SudokuApp.Models.Enums;

namespace SudokuApp.WebApp.Models.RequestObjects {
    
    public class CreateDifficultyRO {

        public string Name { get; set; }
        public DifficultyLevel DifficultyLevel { get; set; }
    }
}
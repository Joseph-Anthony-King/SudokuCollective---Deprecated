using System.Collections.Generic;
using SudokuApp.Models.Enums;

namespace SudokuApp.Models {

    public class Difficulty {

        public int Id { get; set; }
        public string Name { get; set; }
        public DifficultyLevel DifficultyLevel { get; set; }
        public ICollection<SudokuMatrix> Matrices { get; set; }
    }
}

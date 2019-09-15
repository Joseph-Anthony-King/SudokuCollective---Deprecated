using System.Collections.Generic;
using Newtonsoft.Json;
using SudokuApp.Models.Enums;

namespace SudokuApp.Models {

    public class Difficulty {

        public int Id { get; set; }
        public string Name { get; set; }
        public DifficultyLevel DifficultyLevel { get; set; }
        public ICollection<SudokuMatrix> Matrices { get; set; }

        public Difficulty() {
            
            Id = 0;
            Name = string.Empty;
            DifficultyLevel = DifficultyLevel.NULL;
        }

        [JsonConstructor]
        public Difficulty(int id, string name, DifficultyLevel difficultyLevel) : this() {
            
            Id = id;
            Name = name;
            DifficultyLevel = difficultyLevel;
        }
    }
}

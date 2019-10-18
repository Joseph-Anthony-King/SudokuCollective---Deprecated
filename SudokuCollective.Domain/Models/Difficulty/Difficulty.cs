using System.Collections.Generic;
using Newtonsoft.Json;
using SudokuCollective.Domain.Enums;
using SudokuCollective.Domain.Interfaces;

namespace SudokuCollective.Domain.Models {

    public class Difficulty: IDifficulty {

        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public DifficultyLevel DifficultyLevel { get; set; }
        public ICollection<SudokuMatrix> Matrices { get; set; }

        public Difficulty() {
            
            Id = 0;
            Name = string.Empty;
            DifficultyLevel = DifficultyLevel.NULL;
        }

        [JsonConstructor]
        public Difficulty(int id, string name, 
            string displayName, DifficultyLevel difficultyLevel) {
            
            Id = id;
            Name = name;
            DisplayName = displayName;
            DifficultyLevel = difficultyLevel;
        }
    }
}

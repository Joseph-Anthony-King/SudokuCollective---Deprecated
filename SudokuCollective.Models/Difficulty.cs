using System.Collections.Generic;
using Newtonsoft.Json;
using SudokuCollective.Domain.Enums;
using SudokuCollective.Domain.Interfaces;

namespace SudokuCollective.Domain {

    public class Difficulty: IEntityBase {

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
        public Difficulty(int id, string name, DifficultyLevel difficultyLevel) : this() {
            
            Id = id;
            Name = name;
            DifficultyLevel = difficultyLevel;
        }
    }
}

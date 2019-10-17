using SudokuCollective.Domain.Enums;

namespace SudokuCollective.Domain.Interfaces {

    public interface IDifficulty : IEntityBase {

        string Name { get; set; }
        string DisplayName { get; set; }
        DifficultyLevel DifficultyLevel { get; set; }
    }
}

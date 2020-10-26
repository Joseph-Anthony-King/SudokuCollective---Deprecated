using System.Collections.Generic;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Models;

namespace SudokuCollective.Core.Interfaces.Models
{
    public interface IDifficulty : IEntityBase
    {
        string Name { get; set; }
        string DisplayName { get; set; }
        DifficultyLevel DifficultyLevel { get; set; }
        List<SudokuMatrix> Matrices { get; set; }
    }
}

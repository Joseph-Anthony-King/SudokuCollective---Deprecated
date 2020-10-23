using System;

namespace SudokuCollective.Core.Interfaces.Models
{
    public interface IGame : IEntityBase
    {
        int UserId { get; set; }
        IUser User { get; set; }
        int SudokuMatrixId { get; set; }
        ISudokuMatrix SudokuMatrix { get; set; }
        int SudokuSolutionId { get; set; }
        ISudokuSolution SudokuSolution { get; set; }
        bool ContinueGame { get; set; }
        int AppId { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateCompleted { get; set; }
        bool IsSolved();
    }
}

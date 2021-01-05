using System;
using SudokuCollective.Core.Models;

namespace SudokuCollective.Core.Interfaces.Models
{
    public interface IGame : IEntityBase
    {
        int UserId { get; set; }
        User User { get; set; }
        int SudokuMatrixId { get; set; }
        SudokuMatrix SudokuMatrix { get; set; }
        int SudokuSolutionId { get; set; }
        SudokuSolution SudokuSolution { get; set; }
        int AppId { get; set; }
        bool ContinueGame { get; set; }
        int Score { get; set; }
        bool KeepScore { get; set; }
        TimeSpan TimeToSolve { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateUpdated { get; set; }
        DateTime DateCompleted { get; set; }
        bool IsSolved();
    }
}

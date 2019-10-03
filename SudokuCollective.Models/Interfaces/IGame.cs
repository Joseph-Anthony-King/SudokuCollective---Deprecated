using System;

namespace SudokuCollective.Models.Interfaces { 

    public interface IGame {

        DateTime DateCreated { get; set; }
        DateTime DateCompleted { get; set; }
        User User { get; set; }
        SudokuMatrix SudokuMatrix { get; set; }
        SudokuSolution SudokuSolution { get; set; }
        bool ContinueGame { get; set; }
        bool IsSolved();
    }
}

using System;

namespace SudokuApp.Models.Interfaces { 

    public interface IGame {

        int Id { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateCompleted { get; set; }
        User User { get; set; }
        SudokuMatrix SudokuMatrix { get; set; }
        SudokuSolution SudokuSolution { get; set; }
        bool ContinueGame { get; set; }
        bool IsSolved();
    }
}

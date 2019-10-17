using System;

namespace SudokuCollective.Domain.Interfaces { 

    public interface IGame : IEntityBase {

        int UserId { get; set; }
        int SudokuMatrixId { get; set; }
        int SudokuSolutionId { get; set; }
        bool ContinueGame { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateCompleted { get; set; }
    }
}

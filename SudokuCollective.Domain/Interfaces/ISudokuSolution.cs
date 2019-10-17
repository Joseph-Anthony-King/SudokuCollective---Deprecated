using System;
using System.Collections.Generic;

namespace SudokuCollective.Domain.Interfaces {

    public interface ISudokuSolution : IEntityBase {

        List<int> SolutionList { get; }
        DateTime DateCreated { get; set; }
        DateTime DateSolved { get; set; }
    }
}

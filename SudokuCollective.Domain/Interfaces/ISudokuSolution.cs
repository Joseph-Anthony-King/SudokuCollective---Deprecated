using System.Collections.Generic;

namespace SudokuCollective.Domain.Interfaces {

    public interface ISudokuSolution {

        List<int> SolutionList { get; }
        List<int> FirstRow { get; }
        List<int> SecondRow { get; }
        List<int> ThirdRow { get; }
        List<int> FourthRow { get; }
        List<int> FifthRow { get; }
        List<int> SixthRow { get; }
        List<int> SeventhRow { get; }
        List<int> EighthRow { get; }
        List<int> NinthRow { get; }
    }
}

using System.Collections.Generic;

namespace SudokuApp.Models.Interfaces {

    public interface ISudokuSolution {

        int Id { get; set; }
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
        int? GameId { get; set; }
    }
}
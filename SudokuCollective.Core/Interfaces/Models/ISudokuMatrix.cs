using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using SudokuCollective.Core.Models;
using SudokuCollective.Core.Structs;

namespace SudokuCollective.Core.Interfaces.Models
{
    public interface ISudokuMatrix : IEntityBase
    {
        Game Game { get; set; }
        int DifficultyId { get; set; }
        Difficulty Difficulty { get; set; }
        List<SudokuCell> SudokuCells { get; set; }
        Stopwatch Stopwatch { get; }
        List<SudokuCell> FirstColumn { get; }
        List<SudokuCell> SecondColumn { get; }
        List<SudokuCell> ThirdColumn { get; }
        List<SudokuCell> FourthColumn { get; }
        List<SudokuCell> FifthColumn { get; }
        List<SudokuCell> SixthColumn { get; }
        List<SudokuCell> SeventhColumn { get; }
        List<SudokuCell> EighthColumn { get; }
        List<SudokuCell> NinthColumn { get; }
        List<List<SudokuCell>> Columns { get; }
        List<SudokuCell> FirstRegion { get; }
        List<SudokuCell> SecondRegion { get; }
        List<SudokuCell> ThirdRegion { get; }
        List<SudokuCell> FourthRegion { get; }
        List<SudokuCell> FifthRegion { get; }
        List<SudokuCell> SixthRegion { get; }
        List<SudokuCell> SeventhRegion { get; }
        List<SudokuCell> EighthRegion { get; }
        List<SudokuCell> NinthRegion { get; }
        List<List<SudokuCell>> Regions { get; }
        List<SudokuCell> FirstRow { get; }
        List<SudokuCell> SecondRow { get; }
        List<SudokuCell> ThirdRow { get; }
        List<SudokuCell> FourthRow { get; }
        List<SudokuCell> FifthRow { get; }
        List<SudokuCell> SixthRow { get; }
        List<SudokuCell> SeventhRow { get; }
        List<SudokuCell> EighthRow { get; }
        List<SudokuCell> NinthRow { get; }
        List<List<SudokuCell>> Rows { get; }
        List<int> FirstColumnValues { get; }
        List<int> SecondColumnValues { get; }
        List<int> ThirdColumnValues { get; }
        List<int> FourthColumnValues { get; }
        List<int> FifthColumnValues { get; }
        List<int> SixthColumnValues { get; }
        List<int> SeventhColumnValues { get; }
        List<int> EighthColumnValues { get; }
        List<int> NinthColumnValues { get; }
        List<int> FirstRegionValues { get; }
        List<int> SecondRegionValues { get; }
        List<int> ThirdRegionValues { get; }
        List<int> FourthRegionValues { get; }
        List<int> FifthRegionValues { get; }
        List<int> SixthRegionValues { get; }
        List<int> SeventhRegionValues { get; }
        List<int> EighthRegionValues { get; }
        List<int> NinthRegionValues { get; }
        List<int> FirstRowValues { get; }
        List<int> SecondRowValues { get; }
        List<int> ThirdRowValues { get; }
        List<int> FourthRowValues { get; }
        List<int> FifthRowValues { get; }
        List<int> SixthRowValues { get; }
        List<int> SeventhRowValues { get; }
        List<int> EighthRowValues { get; }
        List<int> NinthRowValues { get; }        
        bool IsValid();
        bool IsSolved();
        List<int> ToIntList();
        List<int> ToDisplayedIntList();
        string ToString();
        void SetDifficulty(IDifficulty difficulty);
        void GenerateSolution();
        Task Solve();
        void HandleSudokuCellEvent(object sender, SudokuCellEventArgs e);
    }
}

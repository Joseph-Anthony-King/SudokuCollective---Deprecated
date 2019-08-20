using System.Collections.Generic;
using SudokuApp.Models.Enums;

namespace SudokuApp.Models.Interfaces {

    public interface ISudokuMatrix {
        
        List<SudokuCell> SudokuCells { get; set; }
        List<List<SudokuCell>> Columns { get; }
        List<List<SudokuCell>> Regions { get; }
        List<List<SudokuCell>> Rows { get; }
        List<SudokuCell> FirstColumn { get; }
        List<SudokuCell> SecondColumn { get; }
        List<SudokuCell> ThirdColumn { get; }
        List<SudokuCell> FourthColumn { get; }
        List<SudokuCell> FifthColumn { get; }
        List<SudokuCell> SixthColumn { get; }
        List<SudokuCell> SeventhColumn { get; }
        List<SudokuCell> EighthColumn { get; }
        List<SudokuCell> NinthColumn { get; }
        List<SudokuCell> FirstRegion { get; }
        List<SudokuCell> SecondRegion { get; }
        List<SudokuCell> ThirdRegion { get; }
        List<SudokuCell> FourthRegion { get; }
        List<SudokuCell> FifthRegion { get; }
        List<SudokuCell> SixthRegion { get; }
        List<SudokuCell> SeventhRegion { get; }
        List<SudokuCell> EighthRegion { get; }
        List<SudokuCell> NinthRegion { get; }
        List<SudokuCell> FirstRow { get; }
        List<SudokuCell> SecondRow { get; }
        List<SudokuCell> ThirdRow { get; }
        List<SudokuCell> FourthRow { get; }
        List<SudokuCell> FifthRow { get; }
        List<SudokuCell> SixthRow { get; }
        List<SudokuCell> SeventhRow { get; }
        List<SudokuCell> EighthRow { get; }
        List<SudokuCell> NinthRow { get; }
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
        void GenerateSolution();
        List<int> ToInt32List();
        List<int> ToDisplayedValuesList();
        void SetDifficulty(Difficulty difficulty);
        void HandleSudokuCellUpdatedEvent( object sender, UpdateSudokuCellEventArgs e);
        void HandleSudokuCellResetEvent(object sender, ResetSudokuCellEventArgs e);
    }
}

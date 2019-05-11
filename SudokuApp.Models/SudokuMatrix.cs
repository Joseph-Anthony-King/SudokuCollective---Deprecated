using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuApp.Models {

    public class SudokuMatrix {

        public int Id { get; set; }

        public Difficulty Difficulty;

        public List<SudokuCell> SudokuCells;

        public List<int> FirstColumn { get => SudokuCells.Where(column => column.Column == 1).Select(i => i.Value).Distinct().ToList(); }
        public List<int> SecondColumn { get => SudokuCells.Where(column => column.Column == 2).Select(i => i.Value).Distinct().ToList(); }
        public List<int> ThirdColumn { get => SudokuCells.Where(column => column.Column == 3).Select(i => i.Value).Distinct().ToList(); }
        public List<int> FourthColumn { get => SudokuCells.Where(column => column.Column == 4).Select(i => i.Value).Distinct().ToList(); }
        public List<int> FifthColumn { get => SudokuCells.Where(column => column.Column == 5).Select(i => i.Value).Distinct().ToList(); }
        public List<int> SixthColumn { get => SudokuCells.Where(column => column.Column == 6).Select(i => i.Value).Distinct().ToList(); }
        public List<int> SeventhColumn { get => SudokuCells.Where(column => column.Column == 7).Select(i => i.Value).Distinct().ToList(); }
        public List<int> EighthColumn { get => SudokuCells.Where(column => column.Column == 8).Select(i => i.Value).Distinct().ToList(); }
        public List<int> NinthColumn { get => SudokuCells.Where(column => column.Column == 9).Select(i => i.Value).Distinct().ToList(); }

        public List<int> FirstRegion { get => SudokuCells.Where(region => region.Region == 1).Select(i => i.Value).Distinct().ToList(); }
        public List<int> SecondRegion { get => SudokuCells.Where(region => region.Region == 2).Select(i => i.Value).Distinct().ToList(); }
        public List<int> ThirdRegion { get => SudokuCells.Where(region => region.Region == 3).Select(i => i.Value).Distinct().ToList(); }
        public List<int> FourthRegion { get => SudokuCells.Where(region => region.Region == 4).Select(i => i.Value).Distinct().ToList(); }
        public List<int> FifthRegion { get => SudokuCells.Where(region => region.Region == 5).Select(i => i.Value).Distinct().ToList(); }
        public List<int> SixthRegion { get => SudokuCells.Where(region => region.Region == 6).Select(i => i.Value).Distinct().ToList(); }
        public List<int> SeventhRegion { get => SudokuCells.Where(region => region.Region == 7).Select(i => i.Value).Distinct().ToList(); }
        public List<int> EighthRegion { get => SudokuCells.Where(region => region.Region == 8).Select(i => i.Value).Distinct().ToList(); }
        public List<int> NinthRegion { get => SudokuCells.Where(region => region.Region == 9).Select(i => i.Value).Distinct().ToList(); }

        public List<int> FirstRow { get => SudokuCells.Where(row => row.Row == 1).Select(i => i.Value).Distinct().ToList(); }
        public List<int> SecondRow { get => SudokuCells.Where(row => row.Row == 2).Select(i => i.Value).Distinct().ToList(); }
        public List<int> ThirdRow { get => SudokuCells.Where(row => row.Row == 3).Select(i => i.Value).Distinct().ToList(); }
        public List<int> FourthRow { get => SudokuCells.Where(row => row.Row == 4).Select(i => i.Value).Distinct().ToList(); }
        public List<int> FifthRow { get => SudokuCells.Where(row => row.Row == 5).Select(i => i.Value).Distinct().ToList(); }
        public List<int> SixthRow { get => SudokuCells.Where(row => row.Row == 6).Select(i => i.Value).Distinct().ToList(); }
        public List<int> SeventhRow { get => SudokuCells.Where(row => row.Row == 7).Select(i => i.Value).Distinct().ToList(); }
        public List<int> EighthRow { get => SudokuCells.Where(row => row.Row == 8).Select(i => i.Value).Distinct().ToList(); }
        public List<int> NinthRow { get => SudokuCells.Where(row => row.Row == 9).Select(i => i.Value).Distinct().ToList(); }

        #region Constructors
        public SudokuMatrix() {

            var rowColumnDeliminators = new List<int>() {
                9, 18, 27, 36, 45, 54, 63, 72 };
            var firstRegionDeliminators = new List<int>() {
                1, 2, 3, 10, 11, 12, 19, 20, 21 };
            var secondRegionDeliminators = new List<int>() {
                4, 5, 6, 13, 14, 15, 22, 23, 24 };
            var thirdRegionDeliminators = new List<int>() {
                7, 8, 9, 16, 17, 18, 25, 26, 27 };
            var fourthRegionDeliminators = new List<int>() {
                28, 29, 30, 37, 38, 39, 46, 47, 48 };
            var fifthRegionDeliminators = new List<int>() {
                31, 32, 33, 40, 41, 42, 49, 50, 51 };
            var sixthRegionDeliminators = new List<int>() {
                34, 35, 36, 43, 44, 45, 52, 53, 54 };
            var seventhRegionDeliminators = new List<int>() {
                55, 56, 57, 64, 65, 66, 73, 74, 75 };
            var eighthRegionDeliminators = new List<int>() {
                58, 59, 60, 67, 68, 69, 76, 77, 78 };
            var ninthRegionDeliminators = new List<int>() {
                61, 62, 63, 70, 71, 72, 79, 80, 81 };

            var columnIndexer = 1;
            var regionIndexer = 1;
            var rowIndexer = 1;

            SudokuCells = new List<SudokuCell>();

            for (var i = 1; i < 82; i++) {

                if (firstRegionDeliminators.Contains(i)) {

                    regionIndexer = 1;

                } else if (secondRegionDeliminators.Contains(i)) {

                    regionIndexer = 2;

                } else if (thirdRegionDeliminators.Contains(i)) {

                    regionIndexer = 3;

                } else if (fourthRegionDeliminators.Contains(i)) {

                    regionIndexer = 4;

                } else if (fifthRegionDeliminators.Contains(i)) {

                    regionIndexer = 5;

                } else if (sixthRegionDeliminators.Contains(i)) {

                    regionIndexer = 6;

                } else if (seventhRegionDeliminators.Contains(i)) {

                    regionIndexer = 7;

                } else if (eighthRegionDeliminators.Contains(i)) {

                    regionIndexer = 8;

                } else {

                    regionIndexer = 9;
                }

                SudokuCells.Add(
                    new SudokuCell(
                        i,
                        columnIndexer,
                        regionIndexer,
                        rowIndexer
                    )
                );

                SudokuCells[i - 1].SudokuCellUpdatedEvent += HandleSudokuCellUpdatedEvent;

                columnIndexer++;

                if (rowColumnDeliminators.Contains(i)) {

                    columnIndexer = 1;
                    rowIndexer++;
                }
            }
        }

        public SudokuMatrix(List<int> intList) : this() {

            for (var i = 0; i < SudokuCells.Count; i++) {

                SudokuCells[i].Value = intList[i];
            }
        }

        public SudokuMatrix(string values) : this() {

            var intList = new List<int>();

            foreach (var value in values) {

                var s = char.ToString(value);

                if (Int32.TryParse(s, out var number)) {

                    intList.Add(number);

                } else {

                    intList.Add(0);
                }
            }

            for (var i = 0; i < SudokuCells.Count; i++) {

                SudokuCells[i].Value = intList[i];
            }
        }
        #endregion

        public void GenerateSolution() {

            do {

                ZeroOutSudokuCells();

                foreach (var sudokuCell in SudokuCells) {

                    if (sudokuCell.AvailableValues.Count > 1 && sudokuCell.Value == 0) {

                        AppExtensions.AppExtensions.Shuffle(sudokuCell.AvailableValues);

                        sudokuCell.Value = sudokuCell.AvailableValues[0];
                    }
                }

            } while (!IsValid());
        }

        public bool IsValid() {

            if (FirstColumn.Count == 9 && SecondColumn.Count == 9 && ThirdColumn.Count == 9 && FourthColumn.Count == 9 && FifthColumn.Count == 9 
                && SixthColumn.Count == 9 && SeventhColumn.Count == 9 && EighthColumn.Count == 9  && NinthColumn.Count == 9
                && FirstRegion.Count == 9 && SecondRegion.Count == 9 && ThirdRegion.Count == 9 && FourthRegion.Count == 9 && FifthRegion.Count == 9
                && SixthRegion.Count == 9 && SeventhRegion.Count == 9 && EighthRegion.Count == 9 && NinthRegion.Count == 9
                && FirstRow.Count == 9 && SecondRow.Count == 9 && ThirdRow.Count == 9 && FourthRow.Count == 9 && FifthRow.Count == 9
                && SixthRow.Count == 9 && SeventhRow.Count == 9 && EighthRow.Count == 9 && NinthRow.Count == 9) {

                return true;

            } else {

                return false;
            }
        }

        public void ZeroOutSudokuCells() {

            foreach (var sudokuCell in SudokuCells) {

                sudokuCell.Value = 0;
            }
        }

        public List<int> ToInt32List() {

            List<int> result = new List<int>();

            foreach (var sudokuCell in SudokuCells) {

                result.Add(sudokuCell.Value);
            }

            return result;
        }

        public List<int> ToDisplayedValuesList() {

            List<int> result = new List<int>();

            foreach (var sudokuCell in SudokuCells) {

                result.Add(sudokuCell.DisplayValue);
            }

            return result;
        }

        public override string ToString() {

            StringBuilder result = new StringBuilder();

            foreach (var sudokuCell in SudokuCells) {

                result.Append(sudokuCell);
            }

            return result.ToString();
        }

        public void SetDifficulty(Difficulty difficulty) {

            foreach (var sudokuCell in SudokuCells) {

                sudokuCell.Obscured = true;
            }

            this.Difficulty = difficulty;
            int index;

            if (this.Difficulty == Difficulty.EASY) {

                index = 35;

            } else if (this.Difficulty == Difficulty.MEDIUM) {

                index = 29;

            } else if (this.Difficulty == Difficulty.HARD) {

                index = 23;

            } else {

                index = 17;
            }

            if (this.Difficulty != Difficulty.TEST) {

                List<int> indexerList = new List<int>();

                for (var i = 0; i < SudokuCells.Count; i++) {

                    indexerList.Add(i);
                }

                AppExtensions.AppExtensions.Shuffle(indexerList);
                
                for (var i = 0; i < index; i++) {
                    
                    SudokuCells[indexerList[i]].Obscured = false;
                }

            } else {

                foreach (var sudokuCell in SudokuCells) {

                    sudokuCell.Obscured = false;
                }
            }
        }

        internal void HandleSudokuCellUpdatedEvent(
            object sender,
            UpdateSudokuCellEventArgs e) {

            foreach (var sudokuCell in SudokuCells) {

                if (sudokuCell.Column == e.Column) {

                    sudokuCell.UpdateAvailableValues(e.Value);

                } else if (sudokuCell.Region == e.Region) {

                    sudokuCell.UpdateAvailableValues(e.Value);

                } else if (sudokuCell.Row == e.Row) {

                    sudokuCell.UpdateAvailableValues(e.Value);

                } else {

                    // do nothing...
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuApp.Models {

    public class SudokuMatrix {

        public int Id { get; set; }

        public Difficulty Difficulty;

        public List<SudokuCell> SudokuCells;

        #region Constructors
        public SudokuMatrix(Difficulty difficulty = Difficulty.TEST) {

            var columnRowDeliminators = new List<int>() {
                10, 19, 28, 37, 46, 55, 64, 73 };
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

                SudokuCells[i].SudokuCellUpdatedEvent += HandleSudokuCellUpdatedEvent;

                columnIndexer++;

                if (columnRowDeliminators.Contains(i)) {

                    columnIndexer = 1;
                    rowIndexer++;
                }
            }
            
            setDifficulty();
        }

        public SudokuMatrix(List<int> intList) : this() {

            for (var i = 0; i < SudokuCells.Count; i++) {

                SudokuCells[i].Value = intList[i];
            }
        }

        public SudokuMatrix(string values) : this() {

            var intList = new List<int>();

            foreach (var value in values) {

                if (Int32.TryParse(value.ToString(), out var number)) {

                    intList.Add((Int32)value);

                } else {

                    intList.Add(0);
                }
            }

            for (var i = 0; i < SudokuCells.Count; i++) {

                SudokuCells[i].Value = intList[i];
            }
        }
        #endregion

        public void ZeroOutSudokuCells()
        {

            foreach (var sudokuCell in SudokuCells)
            {

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

        private void setDifficulty() {

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

                if (sudokuCell.Column == e.Column
                    || sudokuCell.Region == e.Region
                    || sudokuCell.Row == e.Row) {

                    sudokuCell.updateAvailableValues(e.Value.ToString());
                }
            }
        }
    }
}

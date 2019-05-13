using SudokuApp.Models;
using System.Collections.Generic;

namespace SudokuApp.Utilities {
    public class SudokuSolver : SudokuMatrix {

        public SudokuSolver(string values) : base(values) { }

        public void SolveSolution() {

            var resultSeed = new List<int>();
            var continueLoop = true;

            do {
                var tmp = new SudokuMatrix(this.ToInt32List());
                tmp.SetDifficulty(Difficulty.TEST);

                foreach (var sudokuCell in tmp.SudokuCells) {

                    if (sudokuCell.AvailableValues.Count > 1 && sudokuCell.Value == 0) {

                        AppExtensions.AppExtensions.Shuffle(sudokuCell.AvailableValues);

                        sudokuCell.Value = sudokuCell.AvailableValues[0];

                        if (sudokuCell.Value == 0) {

                            break;
                        }
                    }
                }

                if (tmp.IsValid()) {

                    resultSeed = tmp.ToInt32List();
                    continueLoop = false;
                }

            } while (continueLoop);

            var result = new SudokuMatrix(resultSeed);
            this.SudokuCells = result.SudokuCells;
        }
    }
}

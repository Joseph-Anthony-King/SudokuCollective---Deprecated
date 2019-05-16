using System.Linq;
using System.Collections.Generic;
using SudokuApp.Models;
using System;
using System.Text;

namespace SudokuApp.Utilities {
    public class SudokuSolver : SudokuMatrix {

        public SudokuSolver(string values) : base(values) { }

        public void Solve() {

            var resultSeed = new List<int>();
            var tmp = new SudokuMatrix(this.ToInt32List());
            tmp.SetDifficulty(Difficulty.TEST);
            var loopSeed = SudokuSolverMethods.IsolateIntersectingValues(tmp, tmp.ToInt32List());

            if (loopSeed.Contains(0)) {

                var loopTmp = new SudokuMatrix(this.ToInt32List());
                loopTmp.SetDifficulty(Difficulty.TEST);

                do {
                    loopTmp = new SudokuMatrix(this.ToInt32List());
                    loopTmp.SetDifficulty(Difficulty.TEST);

                    var unknownsIndex = new List<int>();

                    for (var i = 0; i < loopTmp.SudokuCells.Count; i++) {

                        if (loopTmp.SudokuCells[i].Value == 0) {

                            unknownsIndex.Add(i);
                        }
                    }

                    for (var i = 0; i < unknownsIndex.Count;) {

                        if (loopTmp.SudokuCells[unknownsIndex[i]].AvailableValues.Count > 0) {

                            Random random = new Random();

                            AppExtensions.AppExtensions.Shuffle(loopTmp.SudokuCells[unknownsIndex[i]].AvailableValues, random);

                            loopTmp.SudokuCells[unknownsIndex[i]].Value = loopTmp.SudokuCells[unknownsIndex[i]].AvailableValues[0];

                            i++;

                        } else if (loopTmp.SudokuCells[unknownsIndex[i]].Value > 0) {

                            i++;

                        } else {

                            loopTmp = new SudokuMatrix(loopSeed);
                            loopTmp.SetDifficulty(Difficulty.TEST);
                            i = 0;
                        }
                    }

                } while (!loopTmp.IsValid());

                resultSeed.AddRange(loopTmp.ToInt32List());

            } else {
            
                resultSeed.AddRange(loopSeed);
            }

            Console.WriteLine();

            var result = new SudokuMatrix(resultSeed);
            this.SudokuCells = result.SudokuCells;
        }

    }
}

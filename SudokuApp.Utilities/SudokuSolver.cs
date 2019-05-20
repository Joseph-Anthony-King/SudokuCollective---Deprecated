using System.Collections.Generic;
using SudokuApp.Models;
using System;
using System.Diagnostics;

namespace SudokuApp.Utilities {

    public class SudokuSolver : SudokuMatrix {

        public Stopwatch stopwatch = new Stopwatch();

        public SudokuSolver(string values) : base(values) { }

        public void Solve() {

            stopwatch.Reset();
            stopwatch.Start();

            var resultSeed = new List<int>();
            var tmp = new SudokuMatrix(this.ToInt32List());
            var loopSeed = SudokuSolverMethods.IsolateIntersectingValues(tmp, tmp.ToInt32List());

            if (loopSeed.Contains(0)) {

                var loopTmp = new SudokuMatrix(this.ToInt32List());

                do {

                    if (!stopwatch.IsRunning) {

                        stopwatch.Start();
                    }

                    loopTmp = new SudokuMatrix(this.ToInt32List());

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
                            i = 0;
                        }
                    }

                    stopwatch.Stop();

                } while (stopwatch.Elapsed.Ticks < TimeSpan.TicksPerMinute && !loopTmp.IsValid());

                resultSeed.AddRange(loopTmp.ToInt32List());

            } else {
            
                resultSeed.AddRange(loopSeed);
            }

            var result = new SudokuMatrix(resultSeed);
            this.SudokuCells = result.SudokuCells;

            if (stopwatch.IsRunning) {

                stopwatch.Stop();
            }
        }
    }
}

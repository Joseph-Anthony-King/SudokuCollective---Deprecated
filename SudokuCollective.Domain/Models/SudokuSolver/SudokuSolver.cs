using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SudokuCollective.Domain.Models {

    public class SudokuSolver : SudokuMatrix {

        private int _minutes;
        private long timeLimit;

        public Stopwatch stopwatch = new Stopwatch();
        public int Minutes {

            get => _minutes;
            set {

                if (value < 1) {

                    _minutes = 1;

                } else if (value > 15) {

                    _minutes = 15;

                } else {

                    _minutes = value;
                }
            }
        }

        public SudokuSolver(string values) : base(values) { 

            Minutes = 3;
            timeLimit = TimeSpan.TicksPerMinute * Minutes;
        }

        public SudokuSolver(List<int> values) : base(values) { 

            Minutes = 3;
            timeLimit = TimeSpan.TicksPerMinute * Minutes;
        }

        public void SetTimeLimit(int limit) {

            Minutes = limit;
            timeLimit = TimeSpan.TicksPerMinute * Minutes;
        }

        public async Task Solve() {
            
            await Task.Run(() => {

                stopwatch.Reset();
                stopwatch.Start();

                var resultSeed = new List<int>();
                var tmp = new SudokuMatrix(this.ToInt32List());
                var loopSeed = SudokuSolverMethods.IsolateIntersectingValues(tmp, tmp.ToInt32List());

                if (loopSeed.Contains(0)) {

                    var loopTmp = new SudokuMatrix(loopSeed);

                    do {

                        if (!stopwatch.IsRunning) {

                            stopwatch.Start();
                        }

                        loopTmp = new SudokuMatrix(loopSeed);

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

                    } while (stopwatch.Elapsed.Ticks < timeLimit && !loopTmp.IsValid());

                    resultSeed.AddRange(loopTmp.ToInt32List());

                } else {
                
                    resultSeed.AddRange(loopSeed);
                }

                var result = new SudokuMatrix(resultSeed);
                SudokuCells = result.SudokuCells;

                if (stopwatch.IsRunning) {

                    stopwatch.Stop();
                }
            });
        }
    }
}

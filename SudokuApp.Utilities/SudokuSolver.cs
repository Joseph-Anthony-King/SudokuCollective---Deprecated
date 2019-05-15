using System.Linq;
using System.Collections.Generic;
using SudokuApp.Models;
using System;
using System.Text;

namespace SudokuApp.Utilities {
    public class SudokuSolver : SudokuMatrix {

        #region Private Properties
        #endregion

        public SudokuSolver(string values) : base(values) { }

        public void Solve() {

            var resultSeed = new List<int>();
            var tmp = new SudokuMatrix(this.ToInt32List());
            var loopSeed = IsolateIntersectingValues(tmp.ToInt32List());

            if (loopSeed.Contains(0)) {

                var unknownsIndex = new List<int>();

                for (var i = 0; i < tmp.SudokuCells.Count; i++) {

                    if (tmp.SudokuCells[i].Value == 0) {

                        unknownsIndex.Add(i);
                    }
                }

                Console.WriteLine("\nThere are {0} unknown values with indexes of:\n\t", unknownsIndex.Count);

                StringBuilder sb = new StringBuilder();

                foreach (var i in unknownsIndex) {

                    sb.Append(i + ", ");
                }

                Console.WriteLine(sb.ToString().Trim().Substring(0, sb.Length - 2));



                resultSeed.AddRange(loopSeed);

            } else {
            
                resultSeed.AddRange(loopSeed);
            }

            Console.WriteLine();

            var result = new SudokuMatrix(resultSeed);
            this.SudokuCells = result.SudokuCells;
        }

        private List<List<int>> GenerateKeys(List<int> seed) {

            List<List<int>> result = new List<List<int>>();

            var loopTmp = new SudokuMatrix(seed);
            var targetCells = new List<SudokuCell>();
            var targetCellIndexes = new List<int>();

            var candidates = new List<List<int>>();
            var possibilities = new List<List<int>>();
            var possibilitiesStack = new List<Stack<int>>();

            foreach (var cell in loopTmp.SudokuCells) {

                if (cell.AvailableValues.Count < 6 && cell.Value == 0) {

                    targetCellIndexes.Add(cell.Index);
                    targetCells.Add(cell);
                }
            }

            foreach (var index in targetCellIndexes) {

                var tmpList = new List<List<int>>();

                foreach (var values in targetCells.Where(cell => cell.Index == index).Select(cell => cell.AvailableValues)) {

                    foreach (var value in values) {

                        tmpList.Add(new List<int> { value });
                    }
                }

                if (candidates.Count == 0) {

                    candidates.AddRange(tmpList);

                } else {

                    var tmpCandidate = new List<List<int>>();

                    foreach (var candidate in candidates) {

                        foreach (var tList in tmpList) {

                            var buildCandidate = new List<int>();

                            buildCandidate.AddRange(candidate);
                            buildCandidate.AddRange(tList);

                            tmpCandidate.Add(buildCandidate);
                        }
                    }

                    candidates.AddRange(tmpCandidate);
                }
            }

            possibilities = candidates.Where(list => list.Count == targetCellIndexes.Count).ToList();

            foreach (var possibility in possibilities) {

                possibilitiesStack.Add(new Stack<int>(possibility));
            }

            foreach (var possibility in possibilitiesStack) {

                var keyGenerationMatrix = new SudokuMatrix(seed);

                for (var i = 0; i < targetCellIndexes.Count; i++) {

                    if (possibility.Count > 0) {

                        keyGenerationMatrix.SudokuCells[targetCellIndexes[i] - 1].Value = possibility.Pop();
                    }
                }

                result.Add(keyGenerationMatrix.ToInt32List());
            }

            return result;
        }

        private List<int> IsolateIntersectingValues(List<int> paramList) {

            var MissingFirstColumn = MissingSudokuValues(FirstColumnValues);
            var MissingSecondColumn = MissingSudokuValues(SecondColumnValues);
            var MissingThirdColumn = MissingSudokuValues(ThirdColumnValues);
            var MissingFourthColumn = MissingSudokuValues(FourthColumnValues);
            var MissingFifthColumn = MissingSudokuValues(FifthColumnValues);
            var MissingSixthColumn = MissingSudokuValues(SixthColumnValues);
            var MissingSeventhColumn = MissingSudokuValues(SeventhColumnValues);
            var MissingEighthColumn = MissingSudokuValues(EighthColumnValues);
            var MissingNinthColumn = MissingSudokuValues(NinthColumnValues);
            var MissingFirstRegion = MissingSudokuValues(FirstRegionValues);
            var MissingSecondRegion = MissingSudokuValues(SecondRegionValues);
            var MissingThirdRegion = MissingSudokuValues(ThirdRegionValues);
            var MissingFourthRegion = MissingSudokuValues(FourthRegionValues);
            var MissingFifthRegion = MissingSudokuValues(FifthRegionValues);
            var MissingSixthRegion = MissingSudokuValues(SixthRegionValues);
            var MissingSeventhRegion = MissingSudokuValues(SeventhRegionValues);
            var MissingEighthRegion = MissingSudokuValues(EighthRegionValues);
            var MissingNinthRegion = MissingSudokuValues(NinthRegionValues);
            var MissingFirstRow = MissingSudokuValues(FirstRowValues);
            var MissingSecondRow = MissingSudokuValues(SecondRowValues);
            var MissingThirdRow = MissingSudokuValues(ThirdRowValues);
            var MissingFourthRow = MissingSudokuValues(FourthRowValues);
            var MissingFifthRow = MissingSudokuValues(FifthRowValues);
            var MissingSixthRow = MissingSudokuValues(SixthRowValues);
            var MissingSeventhRow = MissingSudokuValues(SeventhRowValues);
            var MissingEighthRow = MissingSudokuValues(EighthRowValues);
            var MissingNinthRow = MissingSudokuValues(NinthRowValues);

            var tmp = new SudokuMatrix(paramList);
            var count = 0;

            do {

                for (var i = 0; i < tmp.SudokuCells.Count; i++) {

                    if (i == 0) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingFirstColumn, 
                            ref MissingFirstRegion, ref MissingFirstRow, tmp.SudokuCells[i]);

                    } else if (i == 1) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingSecondColumn, 
                            ref MissingFirstRegion, ref MissingFirstRow, tmp.SudokuCells[i]);

                    } else if (i == 2) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingThirdColumn, 
                            ref MissingFirstRegion, ref MissingFirstRow, tmp.SudokuCells[i]);

                    } else if (i == 3) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingFourthColumn, 
                            ref MissingSecondRegion, ref MissingFirstRow, tmp.SudokuCells[i]);

                    } else if (i == 4) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingFifthColumn, 
                            ref MissingSecondRegion, ref MissingFirstRow, tmp.SudokuCells[i]);

                    } else if (i == 5) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingSixthColumn, 
                            ref MissingSecondRegion, ref MissingFirstRow, tmp.SudokuCells[i]);

                    } else if (i == 6) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingSeventhColumn, 
                            ref MissingThirdRegion, ref MissingFirstRow, tmp.SudokuCells[i]);

                    } else if (i == 7) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingEighthColumn, 
                            ref MissingThirdRegion, ref MissingFirstRow, tmp.SudokuCells[i]);

                    } else if (i == 8) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingNinthColumn, 
                            ref MissingThirdRegion, ref MissingFirstRow, tmp.SudokuCells[i]);

                    } else if (i == 9) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingFirstColumn, 
                            ref MissingFirstRegion, ref MissingSecondRow, tmp.SudokuCells[i]);

                    } else if (i == 10) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingSecondColumn, 
                            ref MissingFirstRegion, ref MissingSecondRow, tmp.SudokuCells[i]);

                    } else if (i == 11) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingThirdColumn, 
                            ref MissingFirstRegion, ref MissingSecondRow, tmp.SudokuCells[i]);

                    } else if (i == 12) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingFourthColumn, 
                            ref MissingSecondRegion, ref MissingSecondRow, tmp.SudokuCells[i]);

                    } else if (i == 13) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingFifthColumn, 
                            ref MissingSecondRegion, ref MissingSecondRow, tmp.SudokuCells[i]);

                    } else if (i == 14) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingSixthColumn, 
                            ref MissingSecondRegion, ref MissingSecondRow, tmp.SudokuCells[i]);

                    } else if (i == 15) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingSeventhColumn, 
                            ref MissingThirdRegion, ref MissingSecondRow, tmp.SudokuCells[i]);

                    } else if (i == 16) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingEighthColumn, 
                            ref MissingThirdRegion, ref MissingSecondRow, tmp.SudokuCells[i]);

                    } else if (i == 17) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingNinthColumn, 
                            ref MissingThirdRegion, ref MissingSecondRow, tmp.SudokuCells[i]);

                    } else if (i == 18) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingFirstColumn, 
                            ref MissingFirstRegion, ref MissingThirdRow, tmp.SudokuCells[i]);

                    } else if (i == 19) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingSecondColumn, 
                            ref MissingFirstRegion, ref MissingThirdRow, tmp.SudokuCells[i]);

                    } else if (i == 20) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingThirdColumn, 
                            ref MissingFirstRegion, ref MissingThirdRow, tmp.SudokuCells[i]);

                    } else if (i == 21) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingFourthColumn, 
                            ref MissingSecondRegion, ref MissingThirdRow, tmp.SudokuCells[i]);

                    } else if (i == 22) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingFifthColumn, 
                            ref MissingSecondRegion, ref MissingThirdRow, tmp.SudokuCells[i]);

                    } else if (i == 23) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingSixthColumn, 
                            ref MissingSecondRegion, ref MissingThirdRow, tmp.SudokuCells[i]);

                    } else if (i == 24) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingSeventhColumn, 
                            ref MissingThirdRegion, ref MissingThirdRow, tmp.SudokuCells[i]);

                    } else if (i == 25) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingEighthColumn, 
                            ref MissingThirdRegion, ref MissingThirdRow, tmp.SudokuCells[i]);

                    } else if (i == 26) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingNinthColumn, 
                            ref MissingThirdRegion, ref MissingThirdRow, tmp.SudokuCells[i]);

                    } else if (i == 27) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingFirstColumn, 
                            ref MissingFourthRegion, ref MissingFourthRow, tmp.SudokuCells[i]);

                    } else if (i == 28) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingSecondColumn, 
                            ref MissingFourthRegion, ref MissingFourthRow, tmp.SudokuCells[i]);

                    } else if (i == 29) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingThirdColumn, 
                            ref MissingFourthRegion, ref MissingFourthRow, tmp.SudokuCells[i]);

                    } else if (i == 30) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingFourthColumn, 
                            ref MissingFifthRegion, ref MissingFourthRow, tmp.SudokuCells[i]);

                    } else if (i == 31) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingFifthColumn, 
                            ref MissingFifthRegion, ref MissingFourthRow, tmp.SudokuCells[i]);

                    } else if (i == 32) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingSixthColumn, 
                            ref MissingFifthRegion, ref MissingFourthRow, tmp.SudokuCells[i]);

                    } else if (i == 33) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingSeventhColumn, 
                            ref MissingSixthRegion, ref MissingFourthRow, tmp.SudokuCells[i]);

                    } else if (i == 34) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingEighthColumn, 
                            ref MissingSixthRegion, ref MissingFourthRow, tmp.SudokuCells[i]);

                    } else if (i == 35) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingNinthColumn, 
                            ref MissingSixthRegion, ref MissingFourthRow, tmp.SudokuCells[i]);

                    } else if (i == 36) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingFirstColumn, 
                            ref MissingFourthRegion, ref MissingFifthRow, tmp.SudokuCells[i]);

                    } else if (i == 37) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingSecondColumn, 
                            ref MissingFourthRegion, ref MissingFifthRow, tmp.SudokuCells[i]);

                    } else if (i == 38) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingThirdColumn, 
                            ref MissingFourthRegion, ref MissingFifthRow, tmp.SudokuCells[i]);

                    } else if (i == 39) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingFourthColumn, 
                            ref MissingFifthRegion, ref MissingFifthRow, tmp.SudokuCells[i]);

                    } else if (i == 40) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingFifthColumn, 
                            ref MissingFifthRegion, ref MissingFifthRow, tmp.SudokuCells[i]);

                    } else if (i == 41) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingSixthColumn, 
                            ref MissingFifthRegion, ref MissingFifthRow, tmp.SudokuCells[i]);

                    } else if (i == 42) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingSeventhColumn, 
                            ref MissingSixthRegion, ref MissingFifthRow, tmp.SudokuCells[i]);

                    } else if (i == 43) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingEighthColumn, 
                            ref MissingSixthRegion, ref MissingFifthRow, tmp.SudokuCells[i]);

                    } else if (i == 44) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingNinthColumn, 
                            ref MissingSixthRegion, ref MissingFifthRow, tmp.SudokuCells[i]);

                    } else if (i == 45) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingFirstColumn, 
                            ref MissingFourthRegion, ref MissingSixthRow, tmp.SudokuCells[i]);

                    } else if (i == 46) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingSecondColumn, 
                            ref MissingFourthRegion, ref MissingSixthRow, tmp.SudokuCells[i]);

                    } else if (i == 47) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingThirdColumn, 
                            ref MissingFourthRegion, ref MissingSixthRow, tmp.SudokuCells[i]);

                    } else if (i == 48) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingFourthColumn, 
                            ref MissingFifthRegion, ref MissingSixthRow, tmp.SudokuCells[i]);

                    } else if (i == 49) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingFifthColumn, 
                            ref MissingFifthRegion, ref MissingSixthRow, tmp.SudokuCells[i]);

                    } else if (i == 50) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingSixthColumn, 
                            ref MissingFifthRegion, ref MissingSixthRow, tmp.SudokuCells[i]);

                    } else if (i == 51) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingSeventhColumn, 
                            ref MissingSixthRegion, ref MissingSixthRow, tmp.SudokuCells[i]);

                    } else if (i == 52) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingEighthColumn, 
                            ref MissingSixthRegion, ref MissingSixthRow, tmp.SudokuCells[i]);

                    } else if (i == 53) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingNinthColumn, 
                            ref MissingSixthRegion, ref MissingSixthRow, tmp.SudokuCells[i]);

                    } else if (i == 54) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingFirstColumn, 
                            ref MissingSeventhRegion, ref MissingSeventhRow, tmp.SudokuCells[i]);

                    } else if (i == 55) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingSecondColumn, 
                            ref MissingSeventhRegion, ref MissingSeventhRow, tmp.SudokuCells[i]);

                    } else if (i == 56) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingThirdColumn, 
                            ref MissingSeventhRegion, ref MissingSeventhRow, tmp.SudokuCells[i]);

                    } else if (i == 57) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingFourthColumn, 
                            ref MissingEighthRegion, ref MissingSeventhRow, tmp.SudokuCells[i]);

                    } else if (i == 58) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingFifthColumn, 
                            ref MissingEighthRegion, ref MissingSeventhRow, tmp.SudokuCells[i]);

                    } else if (i == 59) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingSixthColumn, 
                            ref MissingEighthRegion, ref MissingSeventhRow, tmp.SudokuCells[i]);

                    } else if (i == 60) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingSeventhColumn, 
                            ref MissingNinthRegion, ref MissingSeventhRow, tmp.SudokuCells[i]);

                    } else if (i == 61) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingEighthColumn, 
                            ref MissingNinthRegion, ref MissingSeventhRow, tmp.SudokuCells[i]);

                    } else if (i == 62) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingNinthColumn, 
                            ref MissingNinthRegion, ref MissingSeventhRow, tmp.SudokuCells[i]);

                    } else if (i == 63) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingFirstColumn, 
                            ref MissingSeventhRegion, ref MissingEighthRow, tmp.SudokuCells[i]);

                    } else if (i == 64) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingSecondColumn, 
                            ref MissingSeventhRegion, ref MissingEighthRow, tmp.SudokuCells[i]);

                    } else if (i == 65) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingThirdColumn, 
                            ref MissingSeventhRegion, ref MissingEighthRow, tmp.SudokuCells[i]);

                    } else if (i == 66) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingFourthColumn, 
                            ref MissingEighthRegion, ref MissingEighthRow, tmp.SudokuCells[i]);

                    } else if (i == 67) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingFifthColumn, 
                            ref MissingEighthRegion, ref MissingEighthRow, tmp.SudokuCells[i]);

                    } else if (i == 68) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingSixthColumn, 
                            ref MissingEighthRegion, ref MissingEighthRow, tmp.SudokuCells[i]);

                    } else if (i == 69) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingSeventhColumn, 
                            ref MissingNinthRegion, ref MissingEighthRow, tmp.SudokuCells[i]);

                    } else if (i == 70) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingEighthColumn, 
                            ref MissingNinthRegion, ref MissingEighthRow, tmp.SudokuCells[i]);

                    } else if (i == 71) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingNinthColumn, 
                            ref MissingNinthRegion, ref MissingEighthRow, tmp.SudokuCells[i]);

                    } else if (i == 72) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingFirstColumn, 
                            ref MissingSeventhRegion, ref MissingNinthRow, tmp.SudokuCells[i]);

                    } else if (i == 73) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingSecondColumn, 
                            ref MissingSeventhRegion, ref MissingNinthRow, tmp.SudokuCells[i]);

                    } else if (i == 74) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingThirdColumn, 
                            ref MissingSeventhRegion, ref MissingNinthRow, tmp.SudokuCells[i]);

                    } else if (i == 75) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingFourthColumn, 
                            ref MissingEighthRegion, ref MissingNinthRow, tmp.SudokuCells[i]);

                    } else if (i == 76) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingFifthColumn, 
                            ref MissingEighthRegion, ref MissingNinthRow, tmp.SudokuCells[i]);

                    } else if (i == 77) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingSixthColumn, 
                            ref MissingEighthRegion, ref MissingNinthRow, tmp.SudokuCells[i]);

                    } else if (i == 78) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingSeventhColumn, 
                            ref MissingNinthRegion, ref MissingNinthRow, tmp.SudokuCells[i]);

                    } else if (i == 79) {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingEighthColumn, 
                            ref MissingNinthRegion, ref MissingNinthRow, tmp.SudokuCells[i]);

                    } else {

                        ReviewSudokuCharacterForPossibleUpdate(ref MissingNinthColumn, 
                            ref MissingNinthRegion, ref MissingNinthRow, tmp.SudokuCells[i]);
                    }
                }

                count++;

            } while (count < 10);

            var result = new SudokuMatrix(tmp.ToInt32List());
            return result.ToInt32List();
        }

        private void ReviewSudokuCharacterForPossibleUpdate(ref List<int> _firstList, 
            ref List<int> _secondList, ref List<int> _thirdList, SudokuCell cell) {

            if (cell.Value == 0) {

                var i = FindSudokuValue(ref _firstList, ref _secondList, ref _thirdList, cell.Value);

                if (i != 0) {
                    cell.Value = i;
                }
            } else if (cell.Value != 0) {

                if (_firstList.Contains(cell.Value)) {

                    _firstList.Remove(cell.Value);

                } else if (_secondList.Contains(cell.Value)) {

                    _secondList.Remove(cell.Value);

                } else if (_thirdList.Contains(cell.Value)) {

                    _thirdList.Remove(cell.Value);
                }
            }
        }

        private int FindSudokuValue(ref List<int> firstList, ref List<int> secondList, 
            ref List<int> thirdList, int result) {

            if (result == 0) {

                var criteria = new List<int>();

                if (firstList.Count > 0 && secondList.Count > 0 && thirdList.Count > 0) {

                    foreach (var i in firstList) {

                        var AddToCriteria = false;

                        if (secondList.Contains(i) && thirdList.Contains(i)) {
                            AddToCriteria = true;
                        }

                        if (AddToCriteria) {
                            criteria.Add(i);
                        }
                    }

                    if (criteria.Count == 1) {

                        result = criteria[0];

                        firstList.Remove(result);
                        secondList.Remove(result);
                        thirdList.Remove(result);
                    }
                }
            }

            return result;
        }

        public MissingSudokuValuesReference GenerateMissingValuesReference(string key, string value) {

            return new MissingSudokuValuesReference() { Key = key, Value = value };
        }

        public List<int> MissingSudokuValues(List<int> values) {

            var result = new List<int>();
            var criteria = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            foreach (var i in criteria) {

                var AddToResult = true;

                if (values.Contains(i)) {

                    AddToResult = false;
                }

                if (AddToResult) {

                    result.Add(i);
                }
            }

            return result;
        }

        public class MissingSudokuValuesReference {

            public string Key { get; set; }
            public string Value { get; set; }
        }
    }
}

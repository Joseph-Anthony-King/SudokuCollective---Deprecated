using System.Linq;
using System.Collections.Generic;
using SudokuApp.Models;

namespace SudokuApp.Utilities {
    public class SudokuSolver : SudokuMatrix {

        #region Private Properties
        private List<int> MissingFirstColumn { get => MissingSudokuValues(FirstColumnValues); }
        private List<int> MissingSecondColumn { get => MissingSudokuValues(SecondColumnValues); }
        private List<int> MissingThirdColumn { get => MissingSudokuValues(ThirdColumnValues); }
        private List<int> MissingFourthColumn { get => MissingSudokuValues(FourthColumnValues); }
        private List<int> MissingFifthColumn { get => MissingSudokuValues(FifthColumnValues); }
        private List<int> MissingSixthColumn { get => MissingSudokuValues(SixthColumnValues); }
        private List<int> MissingSeventhColumn { get => MissingSudokuValues(SeventhColumnValues); }
        private List<int> MissingEighthColumn { get => MissingSudokuValues(EighthColumnValues); }
        private List<int> MissingNinthColumn { get => MissingSudokuValues(NinthColumnValues); }
        private List<int> MissingFirstRegion { get => MissingSudokuValues(FirstRegionValues); }
        private List<int> MissingSecondRegion { get => MissingSudokuValues(SecondRegionValues); }
        private List<int> MissingThirdRegion { get => MissingSudokuValues(ThirdRegionValues); }
        private List<int> MissingFourthRegion { get => MissingSudokuValues(FourthRegionValues); }
        private List<int> MissingFifthRegion { get => MissingSudokuValues(FifthRegionValues); }
        private List<int> MissingSixthRegion { get => MissingSudokuValues(SixthRegionValues); }
        private List<int> MissingSeventhRegion { get => MissingSudokuValues(SeventhRegionValues); }
        private List<int> MissingEighthRegion { get => MissingSudokuValues(EighthRegionValues); }
        private List<int> MissingNinthRegion { get => MissingSudokuValues(NinthRegionValues); }
        private List<int> MissingFirstRow { get => MissingSudokuValues(FirstRowValues); }
        private List<int> MissingSecondRow { get => MissingSudokuValues(SecondRowValues); }
        private List<int> MissingThirdRow { get => MissingSudokuValues(ThirdRowValues); }
        private List<int> MissingFourthRow { get => MissingSudokuValues(FourthRowValues); }
        private List<int> MissingFifthRow { get => MissingSudokuValues(FifthRowValues); }
        private List<int> MissingSixthRow { get => MissingSudokuValues(SixthRowValues); }
        private List<int> MissingSeventhRow { get => MissingSudokuValues(SeventhRowValues); }
        private List<int> MissingEighthRow { get => MissingSudokuValues(EighthRowValues); }
        private List<int> MissingNinthRow { get => MissingSudokuValues(NinthRowValues); }
        #endregion

        public SudokuSolver(string values) : base(values) { }

        public void Solve() {

            var resultSeed = new List<int>();
            var continueLoop = true;
            var tmp = new SudokuMatrix(this.ToInt32List());
            var loopSeed = IsolateIntersectingValues(tmp.ToInt32List());

            do {

                var loopTmp = new SudokuMatrix(loopSeed);
                loopTmp.SetDifficulty(Difficulty.TEST);

                foreach (var sudokuCell in loopTmp.SudokuCells) {

                    if (sudokuCell.AvailableValues.Count > 1 && sudokuCell.Value == 0) {

                        AppExtensions.AppExtensions.Shuffle(sudokuCell.AvailableValues);

                        sudokuCell.Value = sudokuCell.AvailableValues[0];

                        if (sudokuCell.Value == 0) {

                            break;
                        }
                    }
                }

                if (loopTmp.IsValid()) {

                    resultSeed = loopTmp.ToInt32List();
                    continueLoop = false;
                }

            } while (continueLoop);

            var result = new SudokuMatrix(resultSeed);
            this.SudokuCells = result.SudokuCells;
        }

        private List<int> IsolateIntersectingValues(List<int> paramList) {

            var tmp = new SudokuMatrix(paramList);
            var count = 0;

            do {

                for (var i = 0; i < tmp.SudokuCells.Count; i++) {

                    if (i == 0) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingFirstColumn, MissingFirstRegion, MissingFirstRow, tmp.SudokuCells[i].Value);

                    } else if (i == 1) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingSecondColumn, MissingFirstRegion, MissingFirstRow, tmp.SudokuCells[i].Value);

                    } else if (i == 2) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingThirdColumn, MissingFirstRegion, MissingFirstRow, tmp.SudokuCells[i].Value);

                    } else if (i == 3) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingFourthColumn, MissingSecondRegion, MissingFirstRow, tmp.SudokuCells[i].Value);

                    } else if (i == 4) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingFifthColumn, MissingSecondRegion, MissingFirstRow, tmp.SudokuCells[i].Value);

                    } else if (i == 5) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingSixthColumn, MissingSecondRegion, MissingFirstRow, tmp.SudokuCells[i].Value);

                    } else if (i == 6) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingSeventhColumn, MissingThirdRegion, MissingFirstRow, tmp.SudokuCells[i].Value);

                    } else if (i == 7) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingEighthColumn, MissingThirdRegion, MissingFirstRow, tmp.SudokuCells[i].Value);

                    } else if (i == 8) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingNinthColumn, MissingThirdRegion, MissingFirstRow, tmp.SudokuCells[i].Value);

                    } else if (i == 9) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingFirstColumn, MissingFirstRegion, MissingSecondRow, tmp.SudokuCells[i].Value);

                    } else if (i == 10) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingSecondColumn, MissingFirstRegion, MissingSecondRow, tmp.SudokuCells[i].Value);

                    } else if (i == 11) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingThirdColumn, MissingFirstRegion, MissingSecondRow, tmp.SudokuCells[i].Value);

                    } else if (i == 12) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingFourthColumn, MissingSecondRegion, MissingSecondRow, tmp.SudokuCells[i].Value);

                    } else if (i == 13) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingFifthColumn, MissingSecondRegion, MissingSecondRow, tmp.SudokuCells[i].Value);

                    } else if (i == 14) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingSixthColumn, MissingSecondRegion, MissingSecondRow, tmp.SudokuCells[i].Value);

                    } else if (i == 15) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingSeventhColumn, MissingThirdRegion, MissingSecondRow, tmp.SudokuCells[i].Value);

                    } else if (i == 16) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingEighthColumn, MissingThirdRegion, MissingSecondRow, tmp.SudokuCells[i].Value);

                    } else if (i == 17) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingNinthColumn, MissingThirdRegion, MissingSecondRow, tmp.SudokuCells[i].Value);

                    } else if (i == 18) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingFirstColumn, MissingFirstRegion, MissingThirdRow, tmp.SudokuCells[i].Value);

                    } else if (i == 19) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingSecondColumn, MissingFirstRegion, MissingThirdRow, tmp.SudokuCells[i].Value);

                    } else if (i == 20) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingThirdColumn, MissingFirstRegion, MissingThirdRow, tmp.SudokuCells[i].Value);

                    } else if (i == 21) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingFourthColumn, MissingSecondRegion, MissingThirdRow, tmp.SudokuCells[i].Value);

                    } else if (i == 22) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingFifthColumn, MissingSecondRegion, MissingThirdRow, tmp.SudokuCells[i].Value);

                    } else if (i == 23) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingSixthColumn, MissingSecondRegion, MissingThirdRow, tmp.SudokuCells[i].Value);

                    } else if (i == 24) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingSeventhColumn, MissingThirdRegion, MissingThirdRow, tmp.SudokuCells[i].Value);

                    } else if (i == 25) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingEighthColumn, MissingThirdRegion, MissingThirdRow, tmp.SudokuCells[i].Value);

                    } else if (i == 26) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingNinthColumn, MissingThirdRegion, MissingThirdRow, tmp.SudokuCells[i].Value);

                    } else if (i == 27) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingFirstColumn, MissingFourthRegion, MissingFourthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 28) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingSecondColumn, MissingFourthRegion, MissingFourthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 29) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingThirdColumn, MissingFourthRegion, MissingFourthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 30) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingFourthColumn, MissingFifthRegion, MissingFourthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 31) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingFifthColumn, MissingFifthRegion, MissingFourthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 32) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingSixthColumn, MissingFifthRegion, MissingFourthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 33) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingSeventhColumn, MissingSixthRegion, MissingFourthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 34) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingEighthColumn, MissingSixthRegion, MissingFourthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 35) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingNinthColumn, MissingSixthRegion, MissingFourthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 36) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingFirstColumn, MissingFourthRegion, MissingFifthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 37) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingSecondColumn, MissingFourthRegion, MissingFifthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 38) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingThirdColumn, MissingFourthRegion, MissingFifthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 39) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingFourthColumn, MissingFifthRegion, MissingFifthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 40) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingFifthColumn, MissingFifthRegion, MissingFifthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 41) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingSixthColumn, MissingFifthRegion, MissingFifthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 42) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingSeventhColumn, MissingSixthRegion, MissingFifthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 43) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingEighthColumn, MissingSixthRegion, MissingFifthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 44) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingNinthColumn, MissingSixthRegion, MissingFifthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 45) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingFirstColumn, MissingFourthRegion, MissingSixthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 46) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingSecondColumn, MissingFourthRegion, MissingSixthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 47) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingThirdColumn, MissingFourthRegion, MissingSixthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 48) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingFourthColumn, MissingFifthRegion, MissingSixthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 49) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingFifthColumn, MissingFifthRegion, MissingSixthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 50) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingSixthColumn, MissingFifthRegion, MissingSixthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 51) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingSeventhColumn, MissingSixthRegion, MissingSixthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 52) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingEighthColumn, MissingSixthRegion, MissingSixthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 53) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingNinthColumn, MissingSixthRegion, MissingSixthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 54) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingFirstColumn, MissingSeventhRegion, MissingSeventhRow, tmp.SudokuCells[i].Value);

                    } else if (i == 55) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingSecondColumn, MissingSeventhRegion, MissingSeventhRow, tmp.SudokuCells[i].Value);

                    } else if (i == 56) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingThirdColumn, MissingSeventhRegion, MissingSeventhRow, tmp.SudokuCells[i].Value);

                    } else if (i == 57) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingFourthColumn, MissingEighthRegion, MissingSeventhRow, tmp.SudokuCells[i].Value);

                    } else if (i == 58) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingFifthColumn, MissingEighthRegion, MissingSeventhRow, tmp.SudokuCells[i].Value);

                    } else if (i == 59) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingSixthColumn, MissingEighthRegion, MissingSeventhRow, tmp.SudokuCells[i].Value);

                    } else if (i == 60) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingSeventhColumn, MissingNinthRegion, MissingSeventhRow, tmp.SudokuCells[i].Value);

                    } else if (i == 61) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingEighthColumn, MissingNinthRegion, MissingSeventhRow, tmp.SudokuCells[i].Value);

                    } else if (i == 62) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingNinthColumn, MissingNinthRegion, MissingSeventhRow, tmp.SudokuCells[i].Value);

                    } else if (i == 63) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingFirstColumn, MissingSeventhRegion, MissingEighthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 64) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingSecondColumn, MissingSeventhRegion, MissingEighthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 65) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingThirdColumn, MissingSeventhRegion, MissingEighthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 66) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingFourthColumn, MissingEighthRegion, MissingEighthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 67) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingFifthColumn, MissingEighthRegion, MissingEighthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 68) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingSixthColumn, MissingEighthRegion, MissingEighthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 69) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingSeventhColumn, MissingNinthRegion, MissingEighthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 70) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingEighthColumn, MissingNinthRegion, MissingEighthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 71) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingNinthColumn, MissingNinthRegion, MissingEighthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 72) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingFirstColumn, MissingSeventhRegion, MissingNinthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 73) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingSecondColumn, MissingSeventhRegion, MissingNinthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 74) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingThirdColumn, MissingSeventhRegion, MissingNinthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 75) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingFourthColumn, MissingEighthRegion, MissingNinthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 76) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingFifthColumn, MissingEighthRegion, MissingNinthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 77) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingSixthColumn, MissingEighthRegion, MissingNinthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 78) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingSeventhColumn, MissingNinthRegion, MissingNinthRow, tmp.SudokuCells[i].Value);

                    } else if (i == 79) {

                        ReviewSudokuCharacterForPossibleUpdate(MissingEighthColumn, MissingNinthRegion, MissingNinthRow, tmp.SudokuCells[i].Value);

                    } else {

                        ReviewSudokuCharacterForPossibleUpdate(MissingNinthColumn, MissingNinthRegion, MissingNinthRow, tmp.SudokuCells[i].Value);
                    }
                }

                count++;

            } while (count < 10);

            return tmp.ToInt32List();
        }

        private void ReviewSudokuCharacterForPossibleUpdate(List<int> _firstList, List<int> _secondList,
                                List<int> _thirdList, int index) {
            if (this.SudokuCells[index].Value == 0) {

                var i = FindSudokuValue(_firstList, _secondList, _thirdList, this.SudokuCells[index].Value);

                if (i != 0) {
                    this.SudokuCells[index].Value = 0;
                }
            }
        }

        private int FindSudokuValue(List<int> firstList, List<int> secondList, List<int> thirdList, int result) {

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

        public class MissingSudokuValuesReference {

            public string Key { get; set; }
            public string Value { get; set; }
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
    }
}

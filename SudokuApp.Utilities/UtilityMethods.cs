using System;
using System.Collections.Generic;
using System.Linq;
using SudokuApp.AppExtensions;

namespace SudokuApp.Utilities {
    
    public static class UtilityMethods {

        public static List<int> GenerateSudokuCompliantIntList() {

            const int MAX_ITERATIONS = 1000000;
            int iterations;
            bool completed = false;
            bool maxIterationsReached;
            List<int> listOfNineNumbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            List<List<int>> sudokuMatrix = new List<List<int>>();
            List<int> result = new List<int>();

            do {

                iterations = 0;

                maxIterationsReached = false;
                List<int> firstRow = listOfNineNumbers.ToList();

                Random random = new Random();
                AppExtensions.AppExtensions.Shuffle(firstRow, random);

                List<int> secondRow = listOfNineNumbers.ToList();

                SetSecondRow(MAX_ITERATIONS, ref iterations, ref maxIterationsReached, ref secondRow, firstRow);

                List<int> thirdRow = listOfNineNumbers.ToList();

                if (maxIterationsReached == false) {
                    SetThirdRow(MAX_ITERATIONS, ref iterations, ref maxIterationsReached, ref thirdRow, firstRow, secondRow);
                }

                List<int> firstColumn = new List<int>();
                List<int> secondColumn = new List<int>();
                List<int> thirdColumn = new List<int>();
                List<int> fourthColumn = new List<int>();
                List<int> fifthColumn = new List<int>();
                List<int> sixthColumn = new List<int>();
                List<int> seventhColumn = new List<int>();
                List<int> eighthColumn = new List<int>();
                List<int> ninthColumn = new List<int>();

                if (maxIterationsReached == false) {
                    SetColumns(ref firstColumn, ref secondColumn, ref thirdColumn, ref fourthColumn, ref fifthColumn,
                        ref sixthColumn, ref seventhColumn, ref eighthColumn, ref ninthColumn, firstRow, secondRow,
                        thirdRow);
                }

                List<int> fourthRow = listOfNineNumbers.ToList();

                if (maxIterationsReached == false) {
                    SetFourthRow(MAX_ITERATIONS, ref iterations, ref maxIterationsReached, ref fourthRow, ref firstColumn,
                        ref secondColumn, ref thirdColumn, ref fourthColumn, ref fifthColumn, ref sixthColumn, ref seventhColumn,
                        ref eighthColumn, ref ninthColumn);
                }

                List<int> fifthRow = listOfNineNumbers.ToList();

                if (maxIterationsReached == false) {
                    SetFifthRow(MAX_ITERATIONS, ref iterations, ref maxIterationsReached, ref fifthRow, ref firstColumn, ref secondColumn,
                        ref thirdColumn, ref fourthColumn, ref fifthColumn, ref sixthColumn, ref seventhColumn, ref eighthColumn,
                        ref ninthColumn, fourthRow);
                }

                List<int> sixthRow = listOfNineNumbers.ToList();

                if (maxIterationsReached == false) {
                    SetSixthRow(MAX_ITERATIONS, ref iterations, ref maxIterationsReached, ref sixthRow, ref firstColumn, ref secondColumn,
                        ref thirdColumn, ref fourthColumn, ref fifthColumn, ref sixthColumn, ref seventhColumn, ref eighthColumn,
                        ref ninthColumn, fourthRow, fifthRow);
                }

                List<int> seventhRow = listOfNineNumbers.ToList();

                if (maxIterationsReached == false) {
                    SetSeventhRow(MAX_ITERATIONS, ref iterations, ref maxIterationsReached, ref seventhRow, ref firstColumn, ref secondColumn,
                        ref thirdColumn, ref fourthColumn, ref fifthColumn, ref sixthColumn, ref seventhColumn, ref eighthColumn,
                        ref ninthColumn);
                }

                List<int> eighthRow = listOfNineNumbers.ToList();

                if (maxIterationsReached == false) {
                    SetEighthRow(MAX_ITERATIONS, ref iterations, ref maxIterationsReached, ref eighthRow, ref firstColumn, ref secondColumn,
                        ref thirdColumn, ref fourthColumn, ref fifthColumn, ref sixthColumn, ref seventhColumn, ref eighthColumn,
                        ref ninthColumn, seventhRow);
                }

                List<int> ninthRow = listOfNineNumbers.ToList();

                if (maxIterationsReached == false) {
                    SetNinthRow(MAX_ITERATIONS, ref iterations, ref maxIterationsReached, ref ninthRow, ref firstColumn, ref secondColumn,
                        ref thirdColumn, ref fourthColumn, ref fifthColumn, ref sixthColumn, ref seventhColumn, ref eighthColumn,
                        ref ninthColumn, seventhRow, eighthRow);
                }

                if (maxIterationsReached == false) {
                    AddRowsToSudokuMatrix(ref sudokuMatrix, ref firstRow, ref secondRow, ref thirdRow, ref fourthRow, ref fifthRow,
                        ref sixthRow, ref seventhRow, ref eighthRow, ref ninthRow);

                    completed = true;
                }

            } while (!completed);

            return result = sudokuMatrix.SelectMany(row => row).ToList();
        }

        private static void SetSecondRow(int MAX_ITERATIONS, ref int iterations, ref bool maxIterationsReached, ref List<int> _secondRow, List<int> _firstRow) {

            do {

                Random random = new Random();
                AppExtensions.AppExtensions.Shuffle(_secondRow, random);

                ++iterations;

                if (iterations == MAX_ITERATIONS) {
                    maxIterationsReached = true;
                    break;
                }

            } while (
                (_firstRow.Take(3).OrderBy(n => n).IsThisListEqual(_secondRow.Take(3).OrderBy(n => n))) ||
                (_firstRow.Skip(3).Take(3).OrderBy(n => n).IsThisListEqual(_secondRow.Skip(3).Take(3).OrderBy(n => n))) ||
                (_firstRow.Skip(6).Take(3).OrderBy(n => n).IsThisListEqual(_secondRow.Skip(6).Take(3).OrderBy(n => n)))
            );
        }

        private static void SetThirdRow(int MAX_ITERATIONS, ref int iterations, ref bool maxIterationsReached, ref List<int> thirdRow, List<int> firstRow, List<int> secondRow) {

            do {

                Random random = new Random();
                AppExtensions.AppExtensions.Shuffle(thirdRow, random);

                ++iterations;

                if (iterations == MAX_ITERATIONS) {
                    maxIterationsReached = true;
                    break;
                }

            } while (
                (firstRow.Take(3).OrderBy(n => n).IsThisListEqual(thirdRow.Take(3).OrderBy(n => n))) ||
                (secondRow.Take(3).OrderBy(n => n).IsThisListEqual(thirdRow.Take(3).OrderBy(n => n))) ||
                (firstRow.Skip(3).Take(3).OrderBy(n => n).IsThisListEqual(thirdRow.Skip(3).Take(3).OrderBy(n => n))) ||
                (secondRow.Skip(3).Take(3).OrderBy(n => n).IsThisListEqual(thirdRow.Skip(3).Take(3).OrderBy(n => n))) ||
                (firstRow.Skip(6).Take(3).OrderBy(n => n).IsThisListEqual(thirdRow.Skip(6).Take(3).OrderBy(n => n))) ||
                (secondRow.Skip(6).Take(3).OrderBy(n => n).IsThisListEqual(thirdRow.Skip(6).Take(3).OrderBy(n => n)))
            );
        }

        private static void SetColumns(ref List<int> firstColumn, ref List<int> secondColumn, ref List<int> thirdColumn,
            ref List<int> fourthColumn, ref List<int> fifthColumn, ref List<int> sixthColumn, ref List<int> seventhColumn,
            ref List<int> eighthColumn, ref List<int> ninthColumn, List<int> firstRow, List<int> secondRow, List<int> thirdRow) {

            firstColumn.Add(firstRow[0]);
            firstColumn.Add(secondRow[0]);
            firstColumn.Add(thirdRow[0]);

            secondColumn.Add(firstRow[1]);
            secondColumn.Add(secondRow[1]);
            secondColumn.Add(thirdRow[1]);

            thirdColumn.Add(firstRow[2]);
            thirdColumn.Add(secondRow[2]);
            thirdColumn.Add(thirdRow[2]);

            fourthColumn.Add(firstRow[3]);
            fourthColumn.Add(secondRow[3]);
            fourthColumn.Add(thirdRow[3]);

            fifthColumn.Add(firstRow[4]);
            fifthColumn.Add(secondRow[4]);
            fifthColumn.Add(thirdRow[4]);

            sixthColumn.Add(firstRow[5]);
            sixthColumn.Add(secondRow[5]);
            sixthColumn.Add(thirdRow[5]);

            seventhColumn.Add(firstRow[6]);
            seventhColumn.Add(secondRow[6]);
            seventhColumn.Add(thirdRow[6]);

            eighthColumn.Add(firstRow[7]);
            eighthColumn.Add(secondRow[7]);
            eighthColumn.Add(thirdRow[7]);

            ninthColumn.Add(firstRow[8]);
            ninthColumn.Add(secondRow[8]);
            ninthColumn.Add(thirdRow[8]);
        }

        private static void SetFourthRow(int MAX_ITERATIONS, ref int iterations, ref bool maxIterationsReached, ref List<int> fourthRow,
            ref List<int> firstColumn, ref List<int> secondColumn, ref List<int> thirdColumn, ref List<int> fourthColumn,
            ref List<int> fifthColumn, ref List<int> sixthColumn, ref List<int> seventhColumn, ref List<int> eighthColumn,
            ref List<int> ninthColumn) {

            do {

                Random random = new Random();
                AppExtensions.AppExtensions.Shuffle(fourthRow, random);

                ++iterations;

                if (iterations == MAX_ITERATIONS) {
                    maxIterationsReached = true;
                    break;
                }

            } while (
                (firstColumn.Contains(fourthRow[0])) ||
                (secondColumn.Contains(fourthRow[1])) ||
                (thirdColumn.Contains(fourthRow[2])) ||
                (fourthColumn.Contains(fourthRow[3])) ||
                (fifthColumn.Contains(fourthRow[4])) ||
                (sixthColumn.Contains(fourthRow[5])) ||
                (seventhColumn.Contains(fourthRow[6])) ||
                (eighthColumn.Contains(fourthRow[7])) ||
                (ninthColumn.Contains(fourthRow[8]))
            );

            firstColumn.Add(fourthRow[0]);
            secondColumn.Add(fourthRow[1]);
            thirdColumn.Add(fourthRow[2]);
            fourthColumn.Add(fourthRow[3]);
            fifthColumn.Add(fourthRow[4]);
            sixthColumn.Add(fourthRow[5]);
            seventhColumn.Add(fourthRow[6]);
            eighthColumn.Add(fourthRow[7]);
            ninthColumn.Add(fourthRow[8]);
        }

        private static void SetFifthRow(int MAX_ITERATIONS, ref int iterations, ref bool maxIterationsReached, ref List<int> fifthRow,
            ref List<int> firstColumn, ref List<int> secondColumn, ref List<int> thirdColumn, ref List<int> fourthColumn,
            ref List<int> fifthColumn, ref List<int> sixthColumn, ref List<int> seventhColumn, ref List<int> eighthColumn,
            ref List<int> ninthColumn, List<int> fourthRow) {

            do {

                Random random = new Random();
                AppExtensions.AppExtensions.Shuffle(fifthRow, random);

                ++iterations;

                if (iterations == MAX_ITERATIONS) {
                    maxIterationsReached = true;
                    break;
                }

            } while (
                (firstColumn.Contains(fifthRow[0])) ||
                (secondColumn.Contains(fifthRow[1])) ||
                (thirdColumn.Contains(fifthRow[2])) ||
                (fourthColumn.Contains(fifthRow[3])) ||
                (fifthColumn.Contains(fifthRow[4])) ||
                (sixthColumn.Contains(fifthRow[5])) ||
                (seventhColumn.Contains(fifthRow[6])) ||
                (eighthColumn.Contains(fifthRow[7])) ||
                (ninthColumn.Contains(fifthRow[8])) ||
                (fourthRow.Take(3).OrderBy(n => n).IsThisListEqual(fifthRow.Take(3).OrderBy(n => n))) ||
                (fourthRow.Skip(3).Take(3).OrderBy(n => n).IsThisListEqual(fifthRow.Skip(3).Take(3).OrderBy(n => n))) ||
                (fourthRow.Skip(6).Take(3).OrderBy(n => n).IsThisListEqual(fifthRow.Skip(6).Take(3).OrderBy(n => n)))
            );

            firstColumn.Add(fifthRow[0]);
            secondColumn.Add(fifthRow[1]);
            thirdColumn.Add(fifthRow[2]);
            fourthColumn.Add(fifthRow[3]);
            fifthColumn.Add(fifthRow[4]);
            sixthColumn.Add(fifthRow[5]);
            seventhColumn.Add(fifthRow[6]);
            eighthColumn.Add(fifthRow[7]);
            ninthColumn.Add(fifthRow[8]);
        }

        private static void SetSixthRow(int MAX_ITERATIONS, ref int iterations, ref bool maxIterationsReached, ref List<int> sixthRow,
            ref List<int> firstColumn, ref List<int> secondColumn, ref List<int> thirdColumn, ref List<int> fourthColumn,
            ref List<int> fifthColumn, ref List<int> sixthColumn, ref List<int> seventhColumn, ref List<int> eighthColumn,
            ref List<int> ninthColumn, List<int> fourthRow, List<int> fifthRow) {

            do {

                Random random = new Random();
                AppExtensions.AppExtensions.Shuffle(sixthRow, random);

                ++iterations;

                if (iterations == MAX_ITERATIONS) {
                    maxIterationsReached = true;
                    break;
                }

            } while (
                (firstColumn.Contains(sixthRow[0])) ||
                (secondColumn.Contains(sixthRow[1])) ||
                (thirdColumn.Contains(sixthRow[2])) ||
                (fourthColumn.Contains(sixthRow[3])) ||
                (fifthColumn.Contains(sixthRow[4])) ||
                (sixthColumn.Contains(sixthRow[5])) ||
                (seventhColumn.Contains(sixthRow[6])) ||
                (eighthColumn.Contains(sixthRow[7])) ||
                (ninthColumn.Contains(sixthRow[8])) ||
                (fourthRow.Take(3).OrderBy(n => n).IsThisListEqual(sixthRow.Take(3).OrderBy(n => n))) ||
                (fifthRow.Take(3).OrderBy(n => n).IsThisListEqual(sixthRow.Take(3).OrderBy(n => n))) ||
                (fourthRow.Skip(3).Take(3).OrderBy(n => n).IsThisListEqual(sixthRow.Skip(3).Take(3).OrderBy(n => n))) ||
                (fifthRow.Skip(3).Take(3).OrderBy(n => n).IsThisListEqual(sixthRow.Skip(3).Take(3).OrderBy(n => n))) ||
                (fourthRow.Skip(6).Take(3).OrderBy(n => n).IsThisListEqual(sixthRow.Skip(6).Take(3).OrderBy(n => n))) ||
                (fifthRow.Skip(6).Take(3).OrderBy(n => n).IsThisListEqual(sixthRow.Skip(6).Take(3).OrderBy(n => n)))
            );

            firstColumn.Add(sixthRow[0]);
            secondColumn.Add(sixthRow[1]);
            thirdColumn.Add(sixthRow[2]);
            fourthColumn.Add(sixthRow[3]);
            fifthColumn.Add(sixthRow[4]);
            sixthColumn.Add(sixthRow[5]);
            seventhColumn.Add(sixthRow[6]);
            eighthColumn.Add(sixthRow[7]);
            ninthColumn.Add(sixthRow[8]);
        }

        private static void SetSeventhRow(int MAX_ITERATIONS, ref int iterations, ref bool maxIterationsReached, ref List<int> seventhRow,
            ref List<int> firstColumn, ref List<int> secondColumn, ref List<int> thirdColumn, ref List<int> fourthColumn,
            ref List<int> fifthColumn, ref List<int> sixthColumn, ref List<int> seventhColumn, ref List<int> eighthColumn,
            ref List<int> ninthColumn) {

            do {

                Random random = new Random();
                AppExtensions.AppExtensions.Shuffle(seventhRow, random);

                ++iterations;

                if (iterations == MAX_ITERATIONS) {
                    maxIterationsReached = true;
                    break;
                }

            } while (
                (firstColumn.Contains(seventhRow[0])) ||
                (secondColumn.Contains(seventhRow[1])) ||
                (thirdColumn.Contains(seventhRow[2])) ||
                (fourthColumn.Contains(seventhRow[3])) ||
                (fifthColumn.Contains(seventhRow[4])) ||
                (sixthColumn.Contains(seventhRow[5])) ||
                (seventhColumn.Contains(seventhRow[6])) ||
                (eighthColumn.Contains(seventhRow[7])) ||
                (ninthColumn.Contains(seventhRow[8]))
            );

            firstColumn.Add(seventhRow[0]);
            secondColumn.Add(seventhRow[1]);
            thirdColumn.Add(seventhRow[2]);
            fourthColumn.Add(seventhRow[3]);
            fifthColumn.Add(seventhRow[4]);
            sixthColumn.Add(seventhRow[5]);
            seventhColumn.Add(seventhRow[6]);
            eighthColumn.Add(seventhRow[7]);
            ninthColumn.Add(seventhRow[8]);
        }

        private static void SetEighthRow(int MAX_ITERATIONS, ref int iterations, ref bool maxIterationsReached, ref List<int> eighthRow,
            ref List<int> firstColumn, ref List<int> secondColumn, ref List<int> thirdColumn, ref List<int> fourthColumn,
            ref List<int> fifthColumn, ref List<int> sixthColumn, ref List<int> seventhColumn, ref List<int> eighthColumn,
            ref List<int> ninthColumn, List<int> seventhRow) {

            do {

                Random random = new Random();
                AppExtensions.AppExtensions.Shuffle(eighthRow, random);

                ++iterations;

                if (iterations == MAX_ITERATIONS) {
                    maxIterationsReached = true;
                    break;
                }

            } while (
                (firstColumn.Contains(eighthRow[0])) ||
                (secondColumn.Contains(eighthRow[1])) ||
                (thirdColumn.Contains(eighthRow[2])) ||
                (fourthColumn.Contains(eighthRow[3])) ||
                (fifthColumn.Contains(eighthRow[4])) ||
                (sixthColumn.Contains(eighthRow[5])) ||
                (seventhColumn.Contains(eighthRow[6])) ||
                (eighthColumn.Contains(eighthRow[7])) ||
                (ninthColumn.Contains(eighthRow[8])) ||
                (seventhRow.Take(3).OrderBy(n => n).IsThisListEqual(eighthRow.Take(3).OrderBy(n => n))) ||
                (seventhRow.Skip(3).Take(3).OrderBy(n => n).IsThisListEqual(eighthRow.Skip(3).Take(3).OrderBy(n => n))) ||
                (seventhRow.Skip(6).Take(3).OrderBy(n => n).IsThisListEqual(eighthRow.Skip(6).Take(3).OrderBy(n => n)))
            );

            firstColumn.Add(eighthRow[0]);
            secondColumn.Add(eighthRow[1]);
            thirdColumn.Add(eighthRow[2]);
            fourthColumn.Add(eighthRow[3]);
            fifthColumn.Add(eighthRow[4]);
            sixthColumn.Add(eighthRow[5]);
            seventhColumn.Add(eighthRow[6]);
            eighthColumn.Add(eighthRow[7]);
            ninthColumn.Add(eighthRow[8]);
        }

        private static void SetNinthRow(int MAX_ITERATIONS, ref int iterations, ref bool maxIterationsReached, ref List<int> ninthRow,
            ref List<int> firstColumn, ref List<int> secondColumn, ref List<int> thirdColumn, ref List<int> fourthColumn,
            ref List<int> fifthColumn, ref List<int> sixthColumn, ref List<int> seventhColumn, ref List<int> eighthColumn,
            ref List<int> ninthColumn, List<int> seventhRow, List<int> eighthRow) {

            do {

                Random random = new Random();
                AppExtensions.AppExtensions.Shuffle(ninthRow, random);

                ++iterations;

                if (iterations == MAX_ITERATIONS) {
                    maxIterationsReached = true;
                    break;
                }

            } while (
                (firstColumn.Contains(ninthRow[0])) ||
                (secondColumn.Contains(ninthRow[1])) ||
                (thirdColumn.Contains(ninthRow[2])) ||
                (fourthColumn.Contains(ninthRow[3])) ||
                (fifthColumn.Contains(ninthRow[4])) ||
                (sixthColumn.Contains(ninthRow[5])) ||
                (seventhColumn.Contains(ninthRow[6])) ||
                (eighthColumn.Contains(ninthRow[7])) ||
                (ninthColumn.Contains(ninthRow[8])) ||
                (seventhRow.Take(3).OrderBy(n => n).IsThisListEqual(ninthRow.Take(3).OrderBy(n => n))) ||
                (eighthRow.Take(3).OrderBy(n => n).IsThisListEqual(ninthRow.Take(3).OrderBy(n => n))) ||
                (seventhRow.Skip(3).Take(3).OrderBy(n => n).IsThisListEqual(ninthRow.Skip(3).Take(3).OrderBy(n => n))) ||
                (eighthRow.Skip(3).Take(3).OrderBy(n => n).IsThisListEqual(ninthRow.Skip(3).Take(3).OrderBy(n => n))) ||
                (seventhRow.Skip(6).Take(3).OrderBy(n => n).IsThisListEqual(ninthRow.Skip(6).Take(3).OrderBy(n => n))) ||
                (eighthRow.Skip(6).Take(3).OrderBy(n => n).IsThisListEqual(ninthRow.Skip(6).Take(3).OrderBy(n => n)))
            );

            firstColumn.Add(ninthRow[0]);
            secondColumn.Add(ninthRow[1]);
            thirdColumn.Add(ninthRow[2]);
            fourthColumn.Add(ninthRow[3]);
            fifthColumn.Add(ninthRow[4]);
            sixthColumn.Add(ninthRow[5]);
            seventhColumn.Add(ninthRow[6]);
            eighthColumn.Add(ninthRow[7]);
            ninthColumn.Add(ninthRow[8]);
        }

        private static void AddRowsToSudokuMatrix(ref List<List<int>> sudokuMatrix, ref List<int> firstRow, ref List<int> secondRow,
            ref List<int> thirdRow, ref List<int> fourthRow, ref List<int> fifthRow, ref List<int> sixthRow, ref List<int> seventhRow,
            ref List<int> eighthRow, ref List<int> ninthRow) {
            sudokuMatrix.Add(firstRow);
            sudokuMatrix.Add(secondRow);
            sudokuMatrix.Add(thirdRow);
            sudokuMatrix.Add(fourthRow);
            sudokuMatrix.Add(fifthRow);
            sudokuMatrix.Add(sixthRow);
            sudokuMatrix.Add(seventhRow);
            sudokuMatrix.Add(eighthRow);
            sudokuMatrix.Add(ninthRow);
        }
    }
}

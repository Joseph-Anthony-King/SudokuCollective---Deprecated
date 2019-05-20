using System;
using System.Text;
using System.Diagnostics;
using SudokuApp.Models;
using SudokuApp.Utilities;

namespace SudokuApp.ConsoleApp.Routines {

    public static class SolveSolutions {

        public static void Run() {

            Console.WriteLine("\nPlease enter the sudoku puzzle you wish to solve.");
            Console.WriteLine("You will be entering the nine values for each row.");
            Console.WriteLine("Just enter the values with no spaces, for unknown");
            Console.WriteLine("values enter 0.  Once you're done the solver will");
            Console.WriteLine("produce an answer if possible from the provided.");
            Console.WriteLine("values.  If it cannot produce an answer the solver");
            Console.WriteLine("will let you know.\n");
            Console.WriteLine("Press enter to continue!");

            Console.ReadLine();

            var continueLoop = true;

            do {

                var response = new StringBuilder();

                Console.Write("Enter the first row:   ");

                response.Append(Console.ReadLine());

                Console.Write("Enter the second row:  ");

                response.Append(Console.ReadLine());

                Console.Write("Enter the third row:   ");

                response.Append(Console.ReadLine());

                Console.Write("Enter the fourth row:  ");

                response.Append(Console.ReadLine());

                Console.Write("Enter the fifth row:   ");

                response.Append(Console.ReadLine());

                Console.Write("Enter the sixth row:   ");

                response.Append(Console.ReadLine());

                Console.Write("Enter the seventh row: ");

                response.Append(Console.ReadLine());

                Console.Write("Enter the eighth row:  ");

                response.Append(Console.ReadLine());

                Console.Write("Enter the ninth row:   ");

                response.Append(Console.ReadLine());
                Console.WriteLine();

                var matrix = new SudokuSolver(response.ToString());
                matrix.Solve();

                if (matrix.IsValid()) {
                    matrix.SetDifficulty(Difficulty.TEST);

                    foreach (var row in matrix.Rows) {

                        foreach (var cell in row) {

                            Console.Write(string.Format("{0} ", cell));
                        }

                        Console.WriteLine();
                    }

                    // Format and display the TimeSpan value.
                    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                        matrix.stopwatch.Elapsed.Hours,
                        matrix.stopwatch.Elapsed.Minutes,
                        matrix.stopwatch.Elapsed.Seconds,
                        matrix.stopwatch.Elapsed.Milliseconds / 10);

                    Console.Write("\nTime to generate solution: " + elapsedTime + "\n\n");

                } else {

                    Console.WriteLine("Need more values in order to deduce a possible solution.\n");
                }

                Console.Write("Would you like to solve another solution (yes/no): ");

                var result = Console.ReadLine();

                if (result.ToLower().Equals("no") || result.ToLower().Equals("n")) {

                    continueLoop = false;

                } else {

                    Console.WriteLine();
                }

            } while (continueLoop);
        }

    }
}

using System;
using System.Diagnostics;
using System.Text;
using SudokuApp.Models;
using SudokuApp.Utilities;

namespace SudokuApp.ConsoleApp.Routines {

    public static class SolveSolutions {

        public static void Run() {

            Console.WriteLine("\nPlease enter the sudoku puzzle you wish to solve.");
            Console.WriteLine("You will be entering the nine values for each row.");
            Console.WriteLine("Just enter the values with no spaces, For unknown");
            Console.WriteLine("values enter 0.  Once you're done the solver will");
            Console.WriteLine("produce your answer.\n");
            Console.WriteLine("Press enter to continue!");

            Console.ReadLine();

            Stopwatch stopwatch = new Stopwatch();
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

                stopwatch.Start();
                var matrix = new SudokuSolver(response.ToString());
                matrix.Solve();
                matrix.SetDifficulty(Difficulty.TEST);
                stopwatch.Stop();

                foreach (var row in matrix.Rows) {

                    foreach (var cell in row) {

                        Console.Write(string.Format("{0} ", cell));
                    }

                    Console.WriteLine();
                }

                // Format and display the TimeSpan value.
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    stopwatch.Elapsed.Hours,
                    stopwatch.Elapsed.Minutes,
                    stopwatch.Elapsed.Seconds,
                    stopwatch.Elapsed.Milliseconds / 10);

                Console.Write("\nTime to generate solution: " + elapsedTime + "\n\n");

                Console.Write("Would you like to solve another solutions (yes/no): ");

                var result = Console.ReadLine();
                Console.WriteLine();

                if (result.ToLower().Equals("no") || result.ToLower().Equals("n")) {

                    continueLoop = false;
                }

            } while (continueLoop);
        }

    }
}

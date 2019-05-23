using System;
using System.Text;
using System.Threading.Tasks;
using SudokuApp.Models;
using SudokuApp.Utilities;
using SudokuApp.ConsoleApp.Classes;

namespace SudokuApp.ConsoleApp.Routines {

    internal static class SolveSolutions {

        internal static void Run() {

            var firstRun = true;

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

                if (firstRun) {
                    
                    Console.WriteLine("\nThe solver will run for 3 minutes by default.  You");
                    Console.WriteLine("can adjust this time from anywhere between 1 to 15");
                    Console.WriteLine("minutes.  If you do not want to adjust the default");
                    Console.WriteLine("running time simply press enter at the prompt.");
                    firstRun = false;
                }

                Console.Write("\nIndicate new running time if desired: ");

                var runningTimeResponse = new string(Console.ReadLine());
                Console.WriteLine();

                var matrix = new SudokuSolver(response.ToString());
                if (Int32.TryParse(runningTimeResponse, out var runningTime)) {
                    
                    if (runningTime > 0 && runningTime < 16) {

                        matrix.SetTimeLimit(runningTime);
                    }
                }

                Task solver = matrix.Solve();

                ConsoleSpiner spin = new ConsoleSpiner();
                
                while (!solver.IsCompleted) {
                    
                    spin.Turn();
                }

                Console.WriteLine();
                Console.Beep();

                if (matrix.IsValid()) {
                    
                    matrix.SetDifficulty(Difficulty.TEST);

                    DisplayScreens.DisplayMatix(matrix);

                    // Format and display the TimeSpan value.
                    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                        matrix.stopwatch.Elapsed.Hours,
                        matrix.stopwatch.Elapsed.Minutes,
                        matrix.stopwatch.Elapsed.Seconds,
                        matrix.stopwatch.Elapsed.Milliseconds / 10);

                    Console.Write("\n\nTime to generate solution: " + elapsedTime + "\n\n");

                } else {

                    Console.WriteLine("Need more values in order to deduce a solution.\n");
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

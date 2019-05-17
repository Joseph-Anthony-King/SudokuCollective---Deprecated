using System;
using System.Linq;
using SudokuApp.Models;

namespace SudokuApp.ConsoleApp.Routines {

    public static class GenerateSolutions {

        public static void Run() {

            var result = string.Empty;
            var continueLoop = true;

            do {
                var matrix = new SudokuMatrix();
                matrix.SetDifficulty(Difficulty.TEST);
                matrix.GenerateSolution();
                Console.WriteLine();

                foreach (var row in matrix.Rows) {

                    foreach (var cell in row) {

                        Console.Write(string.Format("{0} ", cell));
                    }

                    Console.WriteLine();
                }

                Console.Write("\nWould you like to generate another solution (yes/no): ");

                result = Console.ReadLine();

                if (result.ToLower().Equals("no") || result.ToLower().Equals("n")) {

                    continueLoop = false;
                }

            } while (continueLoop);
        }
    }
}

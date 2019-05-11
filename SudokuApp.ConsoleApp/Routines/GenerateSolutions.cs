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
                matrix.GenerateSolution();
                matrix.SetDifficulty(Difficulty.TEST);
                Console.WriteLine();

                for (var i = 1; i < 10; i++) {

                    foreach (var row in matrix.SudokuCells.Where(cell => cell.Row == i)) {

                        Console.Write(row.PrintDisplayedValue());
                    }

                    Console.WriteLine();
                }

                Console.Write("\nWould you like to generate another solutions (yes/no): ");

                result = Console.ReadLine();

                if (result.ToLower().Equals("no") || result.ToLower().Equals("n")) {

                    continueLoop = false;
                }

            } while (continueLoop);
        }
    }
}

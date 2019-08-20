using System;
using SudokuApp.ConsoleDev.Classes;
using SudokuApp.Models;
using SudokuApp.Models.Enums;

namespace SudokuApp.ConsoleDev.Routines {

    internal static class GenerateSolutions {

        internal static void Run() {

            var result = string.Empty;
            var continueLoop = true;

            do {
                var matrix = new SudokuMatrix();

                matrix.SetDifficulty(new Difficulty() {
                    Name = "Test",
                    DifficultyLevel = DifficultyLevel.TEST
                });
                
                matrix.GenerateSolution();

                DisplayScreens.DisplayMatix(matrix);

                Console.Write("\n\nWould you like to generate another solution (yes/no): ");

                result = Console.ReadLine();

                if (result.ToLower().Equals("no") || result.ToLower().Equals("n")) {

                    continueLoop = false;
                }

            } while (continueLoop);
        }
    }
}

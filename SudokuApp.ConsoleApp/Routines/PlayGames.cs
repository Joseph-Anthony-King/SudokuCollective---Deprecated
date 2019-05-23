using System;
using SudokuApp.Models;

namespace SudokuApp.ConsoleApp.Routines {

    internal static class PlayGames {

        internal static void Run() {

            Console.WriteLine("\nWe're now going to set up your game.\n");
            Console.Write("Press enter your first name: ");

            var firstName = new string(Console.ReadLine());

            Console.Write("\nPress enter your last name: ");

            var lastName = new string(Console.ReadLine());

            var user = new User(firstName, lastName);

            Console.WriteLine("\nSet a difficulty level:\n");
            Console.WriteLine("\tEnter 1 for Steady Sloth (EASY)");
            Console.WriteLine("\tEnter 2 for Leaping Lemur (MEDIUM)");
            Console.WriteLine("\tEnter 3 for Mighty Mountain Lion (HARD)");
            Console.WriteLine("\tEnter 4 for Sneaky Shark (EVIL)\n");
            Console.Write("Please make your selection: ");

            var difficultyResponse = Console.ReadLine();
            Difficulty difficulty = new Difficulty();

            if (Int32.TryParse(difficultyResponse, out var difficultyNumber)) {

                if (difficultyNumber == 1 || difficultyNumber == 2 
                    || difficultyNumber == 3 || difficultyNumber == 4) {
                    
                    if (difficultyNumber == 1) {

                        difficulty = Difficulty.EASY;

                    } else if (difficultyNumber == 2) {

                        difficulty = Difficulty.MEDIUM;

                    } else if (difficultyNumber == 3) {

                        difficulty = Difficulty.HARD;

                    } else if (difficultyNumber == 4) {

                        difficulty = Difficulty.EVIL;
                    }
                }
            }

            SudokuMatrix matrix = new SudokuMatrix();
            matrix.GenerateSolution();
            matrix.SetDifficulty(difficulty);
            Game game = new Game(user, matrix, difficulty);

            Console.WriteLine();

            foreach (var row in game.SudokuMatrix.Rows) {

                foreach (var cell in row) {

                    Console.Write(string.Format("{0} ", cell));
                }

                Console.WriteLine();
            }            

            Console.WriteLine("\nPress enter to exit to main menu.");
        }
    }
}

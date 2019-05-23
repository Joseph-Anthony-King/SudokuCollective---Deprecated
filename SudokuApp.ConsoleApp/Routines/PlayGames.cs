using System;
using System.Linq;
using SudokuApp.ConsoleApp.Classes;
using SudokuApp.Models;

namespace SudokuApp.ConsoleApp.Routines {

    internal static class PlayGames {

        internal static void Run() {

            Console.WriteLine("\nWe're now going to set up your game.\n");
            Console.Write("Press enter your first name: ");

            var firstName = new string(Console.ReadLine());

            Console.Write("Press enter your last name: ");

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

            var continueGame = true;

            do {

                DisplayScreens.GameScreen(game.SudokuMatrix);

                var command = Console.ReadLine();
                command = command.ToUpper().Trim();

                if (command.Equals("1") || command.Equals("ENTER") || command.Equals("2") || command.Equals("DELETE")) {
                    
                    var continueX = true;

                    do {
                        Console.Write("\n\tEnter the X Coordinate> ");
                        var xValue = Console.ReadLine();

                        if (Int32.TryParse(xValue, out var xNumber)) {

                            if (xNumber > 0 && xNumber < 10) {

                                var continueY = true;

                                do {

                                    Console.Write("\n\tEnter the Y Coordinate> ");
                                    var yValue = Console.ReadLine();
                                    
                                    if (Int32.TryParse(yValue, out var yNumber)) {

                                        if (yNumber > 0 && yNumber < 10) {

                                            var cell = game.SudokuMatrix.SudokuCells
                                                .Where(c => c.Column == xNumber && c.Row == yNumber).FirstOrDefault();

                                            if (cell.Obscured) {

                                                bool userEntryInvalid = true;

                                                do {
                                                    if (command.Equals("1") || command.Equals("ENTER")) {

                                                        Console.Write("\n\tEnter a number from 1 through 9> ");
                                                        string userEntry = Console.ReadLine();
                                                        
                                                        if (Int32.TryParse(userEntry, out var userNumber)) {

                                                            if (userNumber > 0 && userNumber < 10) {

                                                                cell.DisplayValue = userNumber;
                                                                continueX = false;
                                                                continueY = false;
                                                                userEntryInvalid = false;

                                                            } else {

                                                                DisplayScreens.InvalidCoordinate();
                                                            }

                                                        } else {

                                                            DisplayScreens.InvalidCoordinate();
                                                        }

                                                    } else {

                                                        cell.DisplayValue = 0;
                                                        continueX = false;
                                                        continueY = false;
                                                        userEntryInvalid = false;

                                                    }

                                                } while (userEntryInvalid);

                                            } else {
                                                Console.WriteLine("\n\tThis value is a hint provided by the system and cannot be changed.");
                                                Console.WriteLine("\tPlease try again.\n\n\t\t         (Press Enter to Continue)");
                                                Console.ReadLine();
                                                break;
                                            }

                                        } else {
                                            DisplayScreens.InvalidCoordinate();
                                        }
                                    }

                                } while (continueY);

                            } else {

                                DisplayScreens.InvalidCoordinate();
                            }

                        } else {

                            DisplayScreens.InvalidCommand();
                        }

                    } while (continueX);

                } else if (command.Equals("3") || command.Equals("CHECK")) {
                    
                    if (game.IsSolved()) {

                        Console.WriteLine("\n\tYOU WIN!");
                        continueGame = false;

                    } else {

                        Console.WriteLine("\n\tNOPE... TRY AGAIN!");
                    }

                } else if (command.Equals("4") || command.Equals("EXIT")) {

                    continueGame = false;
                }

            } while (continueGame);            

            Console.WriteLine("\nPress enter to exit to main menu.");
        }
    }
}

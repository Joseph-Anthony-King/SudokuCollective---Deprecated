using System;
using System.Linq;
using SudokuCollective.ConsoleDev.Classes;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Models;

namespace SudokuCollective.ConsoleDev.Routines
{
    internal static class PlayGames
    {
        internal static void Run()
        {
            Console.Write("\nPlease enter your nickname: ");

            var nickName = new string(Console.ReadLine());

            var user = new User() { NickName = nickName };

            Console.WriteLine("\nSet a difficulty level:\n");
            Console.WriteLine("Enter 1 for Steady Sloth (EASY)");
            Console.WriteLine("Enter 2 for Leaping Lemur (MEDIUM)");
            Console.WriteLine("Enter 3 for Mighty Mountain Lion (HARD)");
            Console.WriteLine("Enter 4 for Sneaky Shark (EVIL)\n");
            Console.Write(string.Format("{0}, please make your selection: ", user.NickName));

            var difficultyResponse = Console.ReadLine();
            IDifficulty difficulty = new Difficulty();

            if (Int32.TryParse(difficultyResponse, out var difficultyNumber))
            {
                if (difficultyNumber == 1 || difficultyNumber == 2
                    || difficultyNumber == 3 || difficultyNumber == 4)
                {
                    if (difficultyNumber == 1)
                    {
                        difficulty = new Difficulty()
                        {
                            Name = "Easy",
                            DifficultyLevel = DifficultyLevel.EASY
                        };
                    }
                    else if (difficultyNumber == 2)
                    {
                        difficulty = new Difficulty()
                        {
                            Name = "Medium",
                            DifficultyLevel = DifficultyLevel.MEDIUM
                        };
                    }
                    else if (difficultyNumber == 3)
                    {
                        difficulty = new Difficulty()
                        {
                            Name = "Hard",
                            DifficultyLevel = DifficultyLevel.HARD
                        };
                    }
                    else if (difficultyNumber == 4)
                    {
                        difficulty = new Difficulty()
                        {
                            Name = "Evil",
                            DifficultyLevel = DifficultyLevel.EVIL
                        };
                    }
                }
            }

            ISudokuMatrix matrix = new SudokuMatrix();
            matrix.GenerateSolution();
            matrix.SetDifficulty(difficulty);
            IGame game = new Game(
                user, 
                (SudokuMatrix)matrix, 
                (Difficulty)difficulty);

            var continueGame = true;

            do
            {
                DisplayScreens.GameScreen(game);

                var command = Console.ReadLine();
                command = command.ToUpper().Trim();

                if (command.Equals("1") || command.Equals("ENTER") || command.Equals("2") || command.Equals("DELETE"))
                {
                    var continueX = true;

                    do
                    {
                        Console.Write("\nEnter the column: ");
                        var xValue = Console.ReadLine();

                        if (Int32.TryParse(xValue, out var xNumber))
                        {
                            if (xNumber > 0 && xNumber < 10)
                            {
                                var continueY = true;

                                do
                                {
                                    Console.Write("\nEnter the row: ");
                                    var yValue = Console.ReadLine();

                                    if (Int32.TryParse(yValue, out var yNumber))
                                    {
                                        if (yNumber > 0 && yNumber < 10)
                                        {
                                            var cell = game.SudokuMatrix.SudokuCells
                                                .Where(c => c.Column == xNumber && c.Row == yNumber).FirstOrDefault();

                                            if (cell.Obscured)
                                            {
                                                bool userEntryInvalid = true;

                                                do
                                                {
                                                    if (command.Equals("1") || command.Equals("ENTER"))
                                                    {

                                                        Console.Write("\nEnter a number from 1 through 9: ");
                                                        string userEntry = Console.ReadLine();

                                                        if (Int32.TryParse(userEntry, out var userNumber))
                                                        {
                                                            if (userNumber > 0 && userNumber < 10)
                                                            {
                                                                cell.DisplayValue = userNumber;
                                                                continueX = false;
                                                                continueY = false;
                                                                userEntryInvalid = false;
                                                            }
                                                            else
                                                            {
                                                                DisplayScreens.InvalidCoordinate();
                                                            }

                                                        }
                                                        else
                                                        {
                                                            DisplayScreens.InvalidCoordinate();
                                                        }

                                                    }
                                                    else
                                                    {
                                                        cell.DisplayValue = 0;
                                                        continueX = false;
                                                        continueY = false;
                                                        userEntryInvalid = false;
                                                    }

                                                } while (userEntryInvalid);
                                            }
                                            else
                                            {
                                                Console.WriteLine("\nThis value is a hint provided by the system and cannot be changed.");
                                                Console.WriteLine("Please try again.\n\n\t         (Press Enter to Continue)");
                                                Console.ReadLine();
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            DisplayScreens.InvalidCoordinate();
                                        }
                                    }

                                } while (continueY);

                            }
                            else
                            {
                                DisplayScreens.InvalidCoordinate();
                            }
                        }
                        else
                        {
                            DisplayScreens.InvalidCommand();
                        }

                    } while (continueX);

                }
                else if (command.Equals("3") || command.Equals("CHECK"))
                {
                    if (game.IsSolved())
                    {
                        Console.WriteLine("\nYOU WIN!");
                        continueGame = false;
                    }
                    else
                    {
                        Console.WriteLine("\nNOPE... TRY AGAIN!");
                    }

                }
                else if (command.Equals("4") || command.Equals("EXIT"))
                {
                    continueGame = false;
                }

            } while (continueGame);

            Console.WriteLine("\nPress enter to exit to main menu.");
        }
    }
}

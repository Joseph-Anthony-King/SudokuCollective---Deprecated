using System;
using System.Collections.Generic;
using System.Text;
using SudokuCollective.Models;

namespace SudokuCollective.ConsoleDev.Classes {

    public static class DisplayScreens {
        
        public static void GameScreen(Game game) {

            DisplayMatix(game.SudokuMatrix);

            Console.Write(string.Format("\n\n{0}, please make your selection\n\n1) Enter a value (ENTER)", game.User.NickName));
            Console.Write("\n2) Delete a value (DELETE) \n3) Check Your Answer (CHECK)");
            Console.Write("\n4) Exit to Main Menu (EXIT)\n");
            Console.Write("\nYour Selection: ");
        }

        internal static void ProgramPrompt() {

            Console.WriteLine("\nWould you like to generate solutions or solve a solution:\n");
            Console.WriteLine("Enter 1 to generate solutions");
            Console.WriteLine("Enter 2 to solve a solution");
            Console.WriteLine("Enter 3 to play a game");
            Console.WriteLine("Enter 4 to exit program\n");
            Console.Write("Please make your selection: ");

        }

        internal static void DisplayMatix(SudokuMatrix matrix) {

            Console.Write("\n       SudokuApp\n");
            Console.Write("\n   1 2 3 4 5 6 7 8 9\n");
            var i = 1;
            foreach (var row in matrix.Rows) {
                Console.Write(string.Format("\n{0}  ", i));
                DisplayRow(row);
                i++;
            }
        }
        
        private static void DisplayRow(List<SudokuCell> row) {

            foreach (var cell in row) {

                if (!cell.Obscured) {

                    var _previousColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(string.Format("{0} ", cell));
                    Console.ForegroundColor = _previousColor;

                } else {

                    Console.Write(string.Format("{0} ", cell));
                }
            }
        }

        internal static void InvalidCommand()
        {
            Console.WriteLine("\nInvalid Command.");
            Console.WriteLine("\tPlease try again.\n\n\t         (Press Enter to Continue)");
            Console.ReadLine();
            Console.Clear();
        }

        internal static void InvalidCoordinate()
        {
            Console.WriteLine("\nYour response must be an integer 1 through 9.");
            Console.WriteLine("\tPlease try again.\n\n\t         (Press Enter to Continue)");
            Console.ReadLine();
        }
    }
}

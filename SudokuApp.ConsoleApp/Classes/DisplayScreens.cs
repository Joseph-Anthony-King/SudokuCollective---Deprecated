using System;
using System.Collections.Generic;
using System.Text;
using SudokuApp.Models;

namespace SudokuApp.ConsoleApp.Classes {

    public static class DisplayScreens {
        
        public static void GameScreen(SudokuMatrix matrix) {
            
            Console.Write("\n\t   1 2 3 4 5 6 7 8 9\n");
            var i = 1;
            foreach (var row in matrix.Rows) {
                Console.Write(string.Format("\n\t{0}  ", i));
                DisplayRow(row);
                i++;
            }
            Console.Write("\n\n\tPlease make your selection\n\n\t1) Enter a value (ENTER)");
            Console.Write("\n\t2) Delete a value (DELETE) \n\t3) Check Your Answer (CHECK)");
            Console.Write("\n\t4) Exit to Main Menu (EXIT)\n");
            Console.Write("\n\tCommand> ");
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
            Console.WriteLine("\n\tInvalid Command.");
            Console.WriteLine("\tPlease try again.\n\n\t\t         (Press Enter to Continue)");
            Console.ReadLine();
            Console.Clear();
        }

        internal static void InvalidCoordinate()
        {
            Console.WriteLine("\n\tYour response must be an integer 1 through 9.");
            Console.WriteLine("\tPlease try again.\n\n\t\t         (Press Enter to Continue)");
            Console.ReadLine();
        }
    }
}

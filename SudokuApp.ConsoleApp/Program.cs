using System;
using System.Linq;
using SudokuApp.Models;

namespace SudokuApp.ConsoleApp {

    class Program {
        static void Main(string[] args) {

            Console.WriteLine("\nWelcome to the Sudoku Console App!\n");
            Console.WriteLine("Would you like to generate solutions or solve a solution:\n");
            Console.WriteLine("\tEnter 1 to generate solutions");
            Console.WriteLine("\tEnter 2 to solve a solution\n");
            Console.Write("Please make your selection: ");

            begin:

            var response = Console.ReadLine();

            do {

                if (Int32.TryParse(response, out var number)) {

                    if (number == 1 || number == 2) {

                        if (number == 1) {

                            Routines.GenerateSolutions.Run();
                        }
                        break;

                    } else {

                        Console.WriteLine("\nInvalid response.");
                        Console.Write("\nPlease make your selection: ");
                        goto begin;
                    }

                } else {

                    Console.WriteLine("\nInvalid response.");
                    Console.Write("\nPlease make your selection: ");
                    goto begin;
                }

            } while (true);

            Console.WriteLine("\nPress enter to exit the program...");
            Console.ReadLine();
        }
    }
}

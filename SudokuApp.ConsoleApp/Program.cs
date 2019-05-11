using System;
using System.Linq;
using SudokuApp.Models;

namespace SudokuApp.ConsoleApp {

    class Program {
        static void Main(string[] args) {

            Console.WriteLine("\nHello World from the SudokuApp Console App!");
            Console.ReadLine();

            SudokuMatrix matrix = new SudokuMatrix();

            for (var i = 0; i < 5; i++) {

                matrix.GenerateSolution();
                matrix.SetDifficulty(Difficulty.TEST);


                for (var j = 1; j < 10; j++) {

                    foreach (var row in matrix.SudokuCells.Where(cell => cell.Row == j)) {

                        Console.Write(row.PrintDisplayedValue());
                    }
                    Console.WriteLine();
                }

                matrix.ZeroOutSudokuCells();
                Console.WriteLine();
            }

            Console.ReadLine();
        }
    }
}

using System;
using SudokuApp.Models;

namespace SudokuApp.ConsoleApp {

    class Program {
        static void Main(string[] args) {

            Console.WriteLine("\nHello World from the SudokuApp Console App!");
            Console.ReadLine();

            SudokuMatrix matrix = new SudokuMatrix();
            matrix.SetDifficulty(Difficulty.TEST);
            matrix.GenerateSolution();

            Console.WriteLine(matrix);

            Console.ReadLine();
        }
    }
}

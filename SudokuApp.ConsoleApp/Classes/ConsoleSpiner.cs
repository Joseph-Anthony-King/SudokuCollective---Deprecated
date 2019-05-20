using System;
using System.Threading;

namespace SudokuApp.ConsoleApp.Classes {

    public class ConsoleSpiner {
        
        int counter;
        public ConsoleSpiner() {
            counter = 0;
        }

        public void Turn() {

            Thread.Sleep(100);
            counter++;
            Console.Write("  Working... ");
            switch (counter % 4) {

                case 0: Console.Write("/"); break;
                case 1: Console.Write("-"); break;
                case 2: Console.Write("\\"); break;
                case 3: Console.Write("|"); break;
            }

            Console.SetCursorPosition(Console.CursorLeft - 14, Console.CursorTop);
        }
    }
}

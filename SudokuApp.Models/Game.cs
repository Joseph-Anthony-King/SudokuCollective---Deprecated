using System;

namespace SudokuApp.Models {

    public class Game {

        internal int Id { get; set; }
        internal DateTime DateCreated { get; set; }
        internal DateTime DateCompleted { get; set; }
        internal int UserId { get; set; }
        internal SudokuMatrix SudokuMatrix { get; set; }
    }
}

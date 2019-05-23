using System;

namespace SudokuApp.Models {

    public class Game {

        internal int Id { get; set; }
        internal DateTime DateCreated { get; set; }
        internal DateTime DateCompleted { get; set; }
        public User User { get; set; }
        public SudokuMatrix SudokuMatrix { get; set; }
        public bool ContinueGame { get; set; }

        public Game(User user, SudokuMatrix matrix, Difficulty difficulty) {

            this.SudokuMatrix = matrix;
            this.DateCreated = DateTime.Now;
            this.User = user;
            this.SudokuMatrix.Difficulty = difficulty;
            this.ContinueGame = true;
        }

        public bool IsSolved() {

            if (this.SudokuMatrix.IsSolved()) {

                this.ContinueGame = false;
            }

            return !this.ContinueGame;
        }
    }
}

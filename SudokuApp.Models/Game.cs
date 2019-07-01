using System;
using SudokuApp.Models.Interfaces;

namespace SudokuApp.Models {

    public class Game : IGame {

        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateCompleted { get; set; }
        public User User { get; set; }
        public SudokuMatrix SudokuMatrix { get; set; }
        public bool ContinueGame { get; set; }

        public Game(User user, SudokuMatrix matrix, Difficulty difficulty) {

            this.SudokuMatrix = matrix;
            this.DateCreated = DateTime.Now;
            this.User = user;
            this.SudokuMatrix.Difficulty = difficulty;
            this.ContinueGame = true;

            this.User.Games.Add(this);
        }

        public bool IsSolved() {

            if (this.SudokuMatrix.IsSolved()) {

                this.ContinueGame = false;
            }

            return !this.ContinueGame;
        }
    }
}

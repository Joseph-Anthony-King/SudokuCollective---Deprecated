using System;
using SudokuApp.Models.Enums;
using SudokuApp.Models.Interfaces;

namespace SudokuApp.Models {

    public class Game : IGame {

        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateCompleted { get; set; }
        public User User { get; set; }
        public int SudokuMatrixId { get; set; }
        public SudokuMatrix SudokuMatrix { get; set; }
        public bool ContinueGame { get; set; }

        public Game() {

            this.DateCreated = DateTime.Now;
            this.ContinueGame = true;
        }

        public Game(
            User user, 
            SudokuMatrix matrix, 
            Difficulty difficulty) : this() {

            this.SudokuMatrix = matrix;
            this.User = user;
            this.SudokuMatrix.Difficulty = difficulty;

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

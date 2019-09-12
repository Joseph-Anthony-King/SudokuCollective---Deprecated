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
        public SudokuSolution SudokuSolution { get; set; }
        public bool ContinueGame { get; set; }

        public Game() {

            DateCreated = DateTime.UtcNow;
            SudokuSolution = new SudokuSolution();
            SudokuSolution.GameId = Id;
            ContinueGame = true;
        }

        public Game(
            User user, 
            SudokuMatrix matrix, 
            Difficulty difficulty) : this() {

            SudokuMatrix = matrix;
            User = user;
            SudokuMatrix.Difficulty = difficulty;
            SudokuMatrix.SetDifficulty(SudokuMatrix.Difficulty);

            User.Games.Add(this);
        }

        public bool IsSolved() {

            if (SudokuMatrix.IsSolved()) {

                ContinueGame = false;
                SudokuSolution.SolutionList = SudokuMatrix.ToInt32List();
            }

            return !ContinueGame;
        }
    }
}

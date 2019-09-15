using System;
using Newtonsoft.Json;
using SudokuApp.Models.Interfaces;

namespace SudokuApp.Models {

    public class Game : IGame {

        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateCompleted { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public int SudokuMatrixId { get; set; }
        public SudokuMatrix SudokuMatrix { get; set; }
        public int SudokuSolutionId { get; set; }
        public SudokuSolution SudokuSolution { get; set; }
        public bool ContinueGame { get; set; }

        public Game() {

            DateCreated = DateTime.UtcNow;
            ContinueGame = true;
            SudokuSolution = new SudokuSolution();
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

        [JsonConstructor]
        public Game(
            int id,
            DateTime dateCreated,
            DateTime dateCompleted,
            int userId,
            int sudokuMatrixId,
            bool continueGame) : this() {

            Id = id;
            DateCreated = dateCreated;
            DateCompleted = dateCompleted;
            UserId = userId;
            SudokuMatrixId = sudokuMatrixId;
            ContinueGame = continueGame;
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

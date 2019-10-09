using System;
using Newtonsoft.Json;
using SudokuCollective.Domain.Interfaces;

namespace SudokuCollective.Domain {

    public class Game : IGame, IDBEntry {

        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int SudokuMatrixId { get; set; }
        public SudokuMatrix SudokuMatrix { get; set; }
        public int SudokuSolutionId { get; set; }
        public SudokuSolution SudokuSolution { get; set; }
        public bool ContinueGame { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateCompleted { get; set; }

        public Game() {

            Id = 0;
            DateCreated = DateTime.UtcNow;
            ContinueGame = true;
            SudokuSolution = new SudokuSolution();
        }

        public Game(
            User user, 
            SudokuMatrix matrix, 
            Difficulty difficulty) : this() {

            Id = 0;
            User = user;
            SudokuMatrix = matrix;
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
            int sudokuSolutionId,
            bool continueGame) : this() {

            Id = id;
            DateCreated = dateCreated;
            DateCompleted = dateCompleted;
            UserId = userId;
            SudokuMatrixId = sudokuMatrixId;
            SudokuSolutionId = sudokuSolutionId;
            ContinueGame = continueGame;
        }

        public bool IsSolved() {

            if (ContinueGame) {

                if (SudokuMatrix.IsSolved()) {

                    var solvedDate = DateTime.UtcNow;

                    ContinueGame = false;
                    SudokuSolution.SolutionList = SudokuMatrix.ToInt32List();
                    SudokuSolution.DateSolved = solvedDate;
                }
            }

            return !ContinueGame;
        }
    }
}

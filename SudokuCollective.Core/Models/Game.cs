using System;
using Newtonsoft.Json;
using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Core.Models
{
    public class Game : IGame
    {
        #region Properties
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int SudokuMatrixId { get; set; }
        public SudokuMatrix SudokuMatrix { get; set; }
        public int SudokuSolutionId { get; set; }
        public SudokuSolution SudokuSolution { get; set; }
        public bool ContinueGame { get; set; }
        public int AppId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateCompleted { get; set; }
        #endregion

        #region Constructors
        public Game()
        {
            Id = 0;
            DateCreated = DateTime.UtcNow;
            ContinueGame = true;
            SudokuSolution = new SudokuSolution();
            AppId = 0;
        }

        public Game(
            User user,
            SudokuMatrix matrix,
            Difficulty difficulty,
            int appId = 0) : this()
        {
            User = user;
            SudokuMatrix = matrix;
            SudokuMatrix.Difficulty = difficulty;
            SudokuMatrix.SetDifficulty(SudokuMatrix.Difficulty);

            User.Games.Add(this);

            AppId = appId;
        }

        [JsonConstructor]
        public Game(
            int id,
            int userId,
            int sudokuMatrixId,
            int sudokuSolutionId,
            int appId,
            bool continueGame,
            DateTime dateCreated,
            DateTime dateCompleted)
        {
            Id = id;
            UserId = userId;
            SudokuMatrixId = sudokuMatrixId;
            SudokuSolutionId = sudokuSolutionId;
            AppId = appId;
            ContinueGame = continueGame;
            DateCreated = dateCreated;
            DateCompleted = dateCompleted;
        }
        #endregion

        #region Methods
        public bool IsSolved()
        {
            if (ContinueGame)
            {
                if (SudokuMatrix.IsSolved())
                {
                    var solvedDate = DateTime.UtcNow;

                    ContinueGame = false;
                    SudokuSolution.SolutionList = SudokuMatrix.ToInt32List();
                    SudokuSolution.DateSolved = solvedDate;
                }
            }

            return !ContinueGame;
        }

        public Game Convert(IGame game)
        {
            return (Game)game;
        }
        #endregion
    }
}

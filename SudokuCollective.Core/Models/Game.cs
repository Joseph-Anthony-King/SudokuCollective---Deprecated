using System;
using Newtonsoft.Json;
using SudokuCollective.Core.Enums;
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
        public int AppId { get; set; }
        public bool ContinueGame { get; set; }
        public double Score { get; set; }
        public TimeSpan TimeToSolve { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DateCompleted { get; set; }
        #endregion

        #region Constructors
        public Game()
        {
            Id = 0;
            DateCreated = DateTime.UtcNow;
            DateUpdated = DateTime.MinValue;
            DateCompleted = DateTime.MinValue;
            ContinueGame = true;
            Score = 0;
            SudokuSolution = new SudokuSolution();
            AppId = 0;
            TimeToSolve = new TimeSpan();
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
            AppId = appId;

            User.Games.Add(this);
        }

        [JsonConstructor]
        public Game(
            int id,
            int userId,
            int sudokuMatrixId,
            int sudokuSolutionId,
            int appId,
            bool continueGame,
            double score,
            long timeToSolve,
            DateTime dateCreated,
            DateTime dateUpdated,
            DateTime dateCompleted)
        {
            Id = id;
            UserId = userId;
            SudokuMatrixId = sudokuMatrixId;
            SudokuSolutionId = sudokuSolutionId;
            AppId = appId;
            ContinueGame = continueGame;
            Score = score;
            TimeToSolve = new TimeSpan(timeToSolve);
            DateCreated = dateCreated;
            DateUpdated = dateUpdated;
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
                    if (SudokuMatrix.Stopwatch.IsRunning)
                    {
                        SudokuMatrix.Stopwatch.Stop();
                    }

                    TimeToSolve = SudokuMatrix.Stopwatch.Elapsed;

                    if (SudokuMatrix.Difficulty.DifficultyLevel == DifficultyLevel.EASY)
                    {
                        Score = TimeToSolve.Ticks * .80;
                    }
                    else if (SudokuMatrix.Difficulty.DifficultyLevel == DifficultyLevel.MEDIUM)
                    {
                        Score = TimeToSolve.Ticks * .60;
                    }
                    else if (SudokuMatrix.Difficulty.DifficultyLevel == DifficultyLevel.HARD)
                    {
                        Score = TimeToSolve.Ticks * .40;
                    }
                    else if (SudokuMatrix.Difficulty.DifficultyLevel == DifficultyLevel.EVIL)
                    {
                        Score = TimeToSolve.Ticks * 20;
                    }
                    else
                    {
                        Score = double.MaxValue;
                    }

                    var solvedDate = DateTime.UtcNow;

                    ContinueGame = false;
                    DateUpdated = solvedDate;
                    DateCompleted = solvedDate;
                    SudokuSolution.SolutionList = SudokuMatrix.ToIntList();
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

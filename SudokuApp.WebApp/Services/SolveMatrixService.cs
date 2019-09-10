using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SudokuApp.Models;
using SudokuApp.Models.Enums;
using SudokuApp.Utilities;
using SudokuApp.WebApp.Models.DataModel;
using SudokuApp.WebApp.Models.RequestObjects.SolveRequests;
using SudokuApp.WebApp.Services.Interfaces;

namespace SudokuApp.WebApp.Services {

    public class SolveMatrixService : ISolveMatrixService {

        private readonly ApplicationDbContext _context;

        public SolveMatrixService(ApplicationDbContext context) {

            _context = context;
        }

        public async Task<ActionResult<Game>> Solve(SolveRequestsRO solveRequestsRO) {

            var user = await _context.Users
                .Where(u => u.Id == solveRequestsRO.UserId)
                .FirstOrDefaultAsync();

            var intList = new List<int>();

            intList.AddRange(solveRequestsRO.FirstRow);
            intList.AddRange(solveRequestsRO.SecondRow);
            intList.AddRange(solveRequestsRO.ThirdRow);
            intList.AddRange(solveRequestsRO.FourthRow);
            intList.AddRange(solveRequestsRO.FifthRow);
            intList.AddRange(solveRequestsRO.SixthRow);
            intList.AddRange(solveRequestsRO.SeventhRow);
            intList.AddRange(solveRequestsRO.EighthRow);
            intList.AddRange(solveRequestsRO.NinthRow);
            
            var sudokuSolver = new SudokuSolver(intList);
            sudokuSolver.SetTimeLimit(solveRequestsRO.Minutes);

            await sudokuSolver.Solve();

            var result = new Game(
                user, 
                new SudokuMatrix(sudokuSolver.ToInt32List()), 
                new Difficulty() {Name = "Test", DifficultyLevel = DifficultyLevel.TEST});
            
            result.ContinueGame = false;
            result.DateCompleted = DateTime.UtcNow;

            return result;
        }
    }
}
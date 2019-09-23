using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SudokuCollective.Models;
using SudokuCollective.Utilities;
using SudokuCollective.WebApi.Helpers;
using SudokuCollective.WebApi.Models.DataModel;
using SudokuCollective.WebApi.Models.RequestModels.SolveRequests;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.WebApi.Services
{

    public class SolutionsService : ISolutionsService {

        private readonly ApplicationDbContext _context;

        public SolutionsService(ApplicationDbContext context) {

            _context = context;
        }

        public async Task<ActionResult<SudokuSolution>> GetSolution(
            int id, bool fullRecord = true) {

            var solution = new SudokuSolution();

            if (fullRecord) {

                solution = await _context.SudokuSolutions
                    .Where(s => s.Id == id)
                    .Include(s => s.Game)
                    .ThenInclude(g => g.User)
                    .FirstOrDefaultAsync();

                if (solution.Game != null) {

                    solution.Game.User.Roles = await _context.UsersRoles
                        .Where(r => r.UserId == solution.Game.User.Id)
                        .ToListAsync();

                    solution.Game.SudokuMatrix = await _context.SudokuMatrices
                        .Where(matrix => matrix.Id == solution.Game.SudokuMatrixId)
                        .FirstOrDefaultAsync();

                    solution.Game.SudokuMatrix = await StaticApiHelpers
                        .AttachSudokuMatrix(solution.Game, _context);
                }

            } else {

                solution = await _context.SudokuSolutions
                    .Where(s => s.Id == id)
                    .FirstOrDefaultAsync();
            }

            return solution;
        }

        public async Task<ActionResult<IEnumerable<SudokuSolution>>> GetSolutions(
            bool fullRecord = true) {

            var solutions = new List<SudokuSolution>();

            if (fullRecord) {

                solutions = await _context.SudokuSolutions
                    .Include(s => s.Game)
                    .ThenInclude(g => g.User)
                    .ToListAsync();

                foreach (var solution in solutions) {

                    if (solution.Game != null) {

                        solution.Game.User.Roles = await _context.UsersRoles
                            .Where(r => r.UserId == solution.Game.User.Id)
                            .ToListAsync();

                        solution.Game.SudokuMatrix = await _context.SudokuMatrices
                            .Where(matrix => matrix.Id == solution.Game.SudokuMatrixId)
                            .FirstOrDefaultAsync();

                        solution.Game.SudokuMatrix = await StaticApiHelpers
                            .AttachSudokuMatrix(solution.Game, _context);
                    }
                }

            } else {

                solutions = await _context.SudokuSolutions.ToListAsync();
            }

            return solutions;
        }

        public async Task<ActionResult<SudokuSolution>> Solve(
            SolveRequestsRO solveRequestsRO) {

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

            var result = new SudokuSolution(sudokuSolver.ToInt32List());

            _context.SudokuSolutions.Add(result);
            await _context.SaveChangesAsync();

            return result;
        }
    }
}
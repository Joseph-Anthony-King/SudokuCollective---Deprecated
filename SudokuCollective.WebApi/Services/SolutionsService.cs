using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SudokuCollective.Models;
using SudokuCollective.Utilities;
using SudokuCollective.WebApi.Helpers;
using SudokuCollective.WebApi.Models.DataModel;
using SudokuCollective.WebApi.Models.RequestModels.SolveRequests;
using SudokuCollective.WebApi.Models.TaskModels.SolutionRequests;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.WebApi.Services {

    public class SolutionsService : ISolutionsService {

        private readonly ApplicationDbContext _context;

        public SolutionsService(ApplicationDbContext context) {

            _context = context;
        }

        public async Task<SolutionTaskResult> GetSolution(
            int id, bool fullRecord = true) {

            var solutionTaskResult = new SolutionTaskResult() {

                Result = false,
                Solution = new SudokuSolution()
                {
                    Id = 0,
                    SolutionList = new List<int>(),
                    Game = new Game() {

                        Id = 0,
                        UserId = 0,
                        SudokuMatrixId = 0
                    }
                }
            };

            try {

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

                    solutionTaskResult.Result = true;
                    solutionTaskResult.Solution = solution;

                } else {

                    solution = await _context.SudokuSolutions
                        .Where(s => s.Id == id)
                        .FirstOrDefaultAsync();

                    solutionTaskResult.Result = true;
                    solutionTaskResult.Solution = solution;
                }

                return solutionTaskResult;

            } catch (Exception) {

                return solutionTaskResult;
            }
        }

        public async Task<SolutionListTaskResult> GetSolutions(
            bool fullRecord = true) {

            var solutionListTaskResult = new SolutionListTaskResult() {

                Result = false,
                Solutions = new List<SudokuSolution>()
            };

            try {

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

                    solutionListTaskResult.Result = true;
                    solutionListTaskResult.Solutions = solutions;

                } else {

                    solutions = await _context.SudokuSolutions.ToListAsync();

                    solutionListTaskResult.Result = true;
                    solutionListTaskResult.Solutions = solutions;
                }

                return solutionListTaskResult;

            } catch (Exception) {

                return solutionListTaskResult;
            }
        }

        public async Task<SolutionTaskResult> Solve(
            SolveRequestsRO solveRequestsRO) {

            var solutionTaskResult = new SolutionTaskResult() {

                Result = false,
                Solution = new SudokuSolution() {

                    Id = 0,
                    SolutionList = new List<int>(),
                    Game = new Game() {

                        Id = 0,
                        UserId = 0,
                        SudokuMatrixId = 0
                    }
                }
            };

            try {

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

                var solutions = await _context.SudokuSolutions.ToListAsync();

                var solutionInDB = false;

                foreach (var solution in solutions) {

                    if (solution.SolutionList.Count > 0) {

                        var possibleSolution = true;

                        for (var i = 0; i < intList.Count - 1; i++) {

                            if (intList[i] != 0 && intList[i] != solution.SolutionList[i]) {

                                possibleSolution = false;
                                break;
                            }
                        }

                        if (possibleSolution) {

                            solutionInDB = possibleSolution;
                            solutionTaskResult.Result = true;
                            solutionTaskResult.Solution = solution;
                            break;
                        }
                    }
                }

                if (!solutionInDB) {

                    var sudokuSolver = new SudokuSolver(intList);
                    sudokuSolver.SetTimeLimit(solveRequestsRO.Minutes);

                    await sudokuSolver.Solve();

                    var result = new SudokuSolution(sudokuSolver.ToInt32List());

                    var addResultToDataContext = true;

                    foreach (var solution in solutions) {

                        if (solution.ToString().Equals(result.ToString())) {

                            addResultToDataContext = false;
                            result = solution;
                        }
                    }

                    if (addResultToDataContext && !result.ToString().Contains('0')) {

                        _context.SudokuSolutions.Add(result);
                        await _context.SaveChangesAsync();
                    }
                    
                    if (!result.ToString().Contains('0')) {
                        
                        solutionTaskResult.Result = true;
                    }

                    solutionTaskResult.Solution = result;
                }

                return solutionTaskResult;

            } catch (Exception) {

                return solutionTaskResult;
            }
        }        

        public async Task<SolutionTaskResult> Generate() {

            var solutionTaskResult = new SolutionTaskResult() {

                Result = false,
                Solution = new SudokuSolution() {

                    Id = 0,
                    SolutionList = new List<int>(),
                    Game = new Game() {

                        Id = 0,
                        UserId = 0,
                        SudokuMatrixId = 0
                    }
                }
            };
            
            var continueLoop = true;

            do {

                var matrix = new SudokuMatrix();

                matrix.GenerateSolution();

                var solutions = await _context.SudokuSolutions
                    .ToListAsync();

                var matrixNotInDB = true;

                foreach (var solution in solutions) {

                    if (solution.SolutionList.Count > 0) {

                        if (solution.ToString().Equals(matrix)) {
                            
                            matrixNotInDB = false;
                        }
                    }
                }

                if (matrixNotInDB) {

                    solutionTaskResult.Solution = new SudokuSolution(
                        matrix.ToInt32List());

                    continueLoop = false;
                }

            } while (continueLoop);

            _context.SudokuSolutions.Add(solutionTaskResult.Solution);
            await _context.SaveChangesAsync();

            solutionTaskResult.Result = true;

            return solutionTaskResult;
        }
    }
}

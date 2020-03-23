using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SudokuCollective.Domain.Models;
using SudokuCollective.WebApi.Helpers;
using SudokuCollective.WebApi.Models.DataModel;
using SudokuCollective.WebApi.Models.RequestModels;
using SudokuCollective.WebApi.Models.RequestModels.SolveRequests;
using SudokuCollective.WebApi.Models.ResultModels;
using SudokuCollective.WebApi.Models.ResultModels.SolutionResults;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.WebApi.Services {

    public class SolutionsService : ISolutionsService {

        private readonly DatabaseContext _context;

        public SolutionsService(DatabaseContext context) {

            _context = context;
        }

        public async Task<SolutionResult> GetSolution(
            int id, bool fullRecord = false) {

            var solutionTaskResult = new SolutionResult();

            try {

                var solution = new SudokuSolution();

                if (fullRecord) {

                    solution = await _context.SudokuSolutions
                        .Where(s => s.Id == id)
                        .Include(s => s.Game)
                        .ThenInclude(g => g.User)
                        .FirstOrDefaultAsync();

                    if (solution == null) {

                        solutionTaskResult.Message = "Solution not found";

                        return solutionTaskResult;
                    }

                    if (solution.Game != null) {

                        solution.Game.User.Roles = await _context.UsersRoles
                            .Where(r => r.UserId == solution.Game.User.Id)
                            .ToListAsync();

                        solution.Game.SudokuMatrix = await _context.SudokuMatrices
                            .Where(matrix => matrix.Id == solution.Game.SudokuMatrixId)
                            .FirstOrDefaultAsync();

                        await solution.Game.SudokuMatrix.AttachSudokuCells(_context);
                    }

                    solutionTaskResult.Success = true;
                    solutionTaskResult.Solution = solution;

                } else {

                    solution = await _context.SudokuSolutions
                        .Where(s => s.Id == id)
                        .FirstOrDefaultAsync();

                    solutionTaskResult.Success = true;
                    solutionTaskResult.Solution = solution;
                }

                return solutionTaskResult;

            } catch (Exception e) {

                solutionTaskResult.Message = e.Message;

                return solutionTaskResult;
            }
        }

        public async Task<SolutionsResult> GetSolutions(
            BaseRequest baseRequestRO,
            bool fullRecord = false,
            int userId = 0) {

            var solutionListTaskResult = new SolutionsResult();

            try {

                var solutions = new List<SudokuSolution>();

                if (fullRecord) {

                    solutions = await SolutionsServiceUtilities
                        .RetrieveGames(baseRequestRO, _context, userId);

                    foreach (var solution in solutions) {

                        if (solution.Game != null) {

                            solution.Game.User.Roles = await _context.UsersRoles
                                .Where(r => r.UserId == solution.Game.User.Id)
                                .ToListAsync();

                            solution.Game.SudokuMatrix = await _context.SudokuMatrices
                                .Where(matrix => matrix.Id == solution.Game.SudokuMatrixId)
                                .FirstOrDefaultAsync();

                            await solution.Game.SudokuMatrix.AttachSudokuCells(_context);
                        }
                    }

                    solutionListTaskResult.Success = true;
                    solutionListTaskResult.Solutions = solutions;

                } else {

                    solutions =await SolutionsServiceUtilities
                        .RetrieveGames(baseRequestRO, _context, userId);

                    solutionListTaskResult.Success = true;
                    solutionListTaskResult.Solutions = solutions;
                }

                return solutionListTaskResult;

            } catch (Exception e) {

                solutionListTaskResult.Message = e.Message;

                return solutionListTaskResult;
            }
        }

        public async Task<SolutionResult> Solve(
            SolveRequest solveRequest) {

            var solutionTaskResult = new SolutionResult();

            try {

                var user = await _context.Users
                    .Where(u => u.Id == solveRequest.UserId)
                    .FirstOrDefaultAsync();

                if (user == null) {

                    solutionTaskResult.Message = "Requesting User Not Found";

                    return solutionTaskResult;
                }

                var intList = new List<int>();

                intList.AddRange(solveRequest.FirstRow);
                intList.AddRange(solveRequest.SecondRow);
                intList.AddRange(solveRequest.ThirdRow);
                intList.AddRange(solveRequest.FourthRow);
                intList.AddRange(solveRequest.FifthRow);
                intList.AddRange(solveRequest.SixthRow);
                intList.AddRange(solveRequest.SeventhRow);
                intList.AddRange(solveRequest.EighthRow);
                intList.AddRange(solveRequest.NinthRow);

                var firstNonZeroValue = 0;
                var firstNonZeroValueIndex = 0;

                for (var i = 0; i < intList.Count - 1; i++) {

                    if (intList[i] != 0) {

                        firstNonZeroValue = intList[i];
                        firstNonZeroValueIndex = i;
                        i = intList.Count - 2;
                    }
                }

                var solutions = await _context.SudokuSolutions
                    .Where(s => s.SolutionList.Count > 0 && s.SolutionList[firstNonZeroValueIndex]
                        .Equals((char)firstNonZeroValue))
                    .ToListAsync();

                var solutionInDB = false;

                if (solutions.Count > 0) {

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
                                solutionTaskResult.Success = true;
                                solutionTaskResult.Solution = solution;
                                break;
                            }
                        }
                    }
                }

                if (!solutionInDB) {

                    var sudokuSolver = new SudokuSolver(intList);
                    sudokuSolver.SetTimeLimit(solveRequest.Minutes);

                    await sudokuSolver.Solve();

                    if (sudokuSolver.IsValid()) {

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

                            solutionTaskResult.Success = true;
                        }

                        solutionTaskResult.Solution = result;

                    } else {

                        solutionTaskResult.Solution = null;
                        solutionTaskResult.Success = true;
                    }
                }

                return solutionTaskResult;

            } catch (Exception e) {

                solutionTaskResult.Message = e.Message;

                return solutionTaskResult;
            }
        }

        public async Task<SolutionResult> Generate() {

            var solutionTaskResult = new SolutionResult();
            
            var continueLoop = true;

            do {

                var matrix = new SudokuMatrix();

                matrix.GenerateSolution();

                var solutions = await _context.SudokuSolutions
                    .ToListAsync();

                var matrixNotInDB = true;

                if (solutions.Count > 0) {

                    foreach (var solution in solutions) {

                        if (solution.SolutionList.Count > 0 && solution.ToString().Equals(matrix)) {

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

            solutionTaskResult.Success = true;

            return solutionTaskResult;
        }

        public async Task<BaseResult> AddSolutions(int limitArg) {

            var limit = 1000;

            if (limitArg <= limit) {

                limit = limitArg;
            }

            var reduceLimitBy = 0;

            var solutions = await _context.SudokuSolutions
                .ToListAsync();

            List<List<int>> solutionsInDB = new List<List<int>>();

            if (solutions.Count > 0) {

                foreach (var solution in solutions) {

                    if (solution.SolutionList.Count > 0) {

                        solutionsInDB.Add(solution.SolutionList);

                    }
                }
            }

            var matrix = new SudokuMatrix();
            var baseTaskResult = new BaseResult();

            try {

                List<List<int>> solutionsList = new List<List<int>>();
                List<SudokuSolution> result = new List<SudokuSolution>();

                var continueLoop = true;

                do {

                    for (var i = 0; i < limit - reduceLimitBy; i++) {

                        matrix.GenerateSolution();

                        if (!solutionsInDB.Contains(matrix.ToInt32List())) {

                            solutionsList.Add(matrix.ToInt32List());
                        }
                    }

                    solutionsList = solutionsList
                        .Distinct()
                        .ToList();

                    if (limit == solutionsList.Count) {

                        continueLoop = false;

                    } else {

                        reduceLimitBy = limit - solutionsList.Count;
                    }

                } while (continueLoop);

                foreach(var solutionList in solutionsList) {

                    result.Add(new SudokuSolution(solutionList));
                }

                _context.SudokuSolutions.AddRange(result);
                await _context.SaveChangesAsync();

                baseTaskResult.Success = true;
                
            } catch (Exception e) {
                
                baseTaskResult.Message = e.Message;
                baseTaskResult.Success = false;
            }

            return baseTaskResult;
        }
    }
}

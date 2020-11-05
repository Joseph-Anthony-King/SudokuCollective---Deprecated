using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Data.Models.ResultModels;
using SudokuCollective.Core.Models;
using SudokuCollective.Core.Interfaces.Repositories;
using SudokuCollective.Core.Enums;

namespace SudokuCollective.Data.Services
{
    public class SolutionsService : ISolutionsService
    {
        private readonly ISolutionsRepository<SudokuSolution> solutionsRepository;
        private readonly IUsersRepository<User> usersRepository;
        private readonly string solutionNotFoundMessage;
        private readonly string solutionsNotFoundMessage;
        private readonly string sortValueNotImplementedMessage;
        private readonly string userNotFoundMessage;
        private readonly string solutionsAddedMessage;
        private readonly string unableToAddSolutionsMessage;

        public SolutionsService(
            ISolutionsRepository<SudokuSolution> solutionsRepo,
            IUsersRepository<User> usersRepo)
        {
            solutionsRepository = solutionsRepo;
            usersRepository = usersRepo;
            solutionNotFoundMessage = "Solution not found";
            solutionsNotFoundMessage = "Solutions not found";
            sortValueNotImplementedMessage = "Sorting not implemented for this sort value";
            userNotFoundMessage = "User not found message";
            solutionsAddedMessage = "Solutions added";
            unableToAddSolutionsMessage = "Unable to add solutions";
        }

        public async Task<ISolutionResult> GetSolution(
            int id, bool fullRecord = false)
        {
            var result = new SolutionResult();

            try
            {
                var solutionResponse = await solutionsRepository.GetById(id, fullRecord);

                if (solutionResponse.Success)
                {
                    result.Success = solutionResponse.Success;
                    result.Solution = (SudokuSolution)solutionResponse.Object;

                    return result;
                }
                else if (!solutionResponse.Success && solutionResponse.Exception != null)
                {
                    result.Success = solutionResponse.Success;
                    result.Message = solutionResponse.Exception.Message;

                    return result;
                }
                else
                {
                    result.Success = false;
                    result.Message = solutionNotFoundMessage;

                    return result;
                }
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Message = e.Message;

                return result;
            }
        }

        public async Task<ISolutionsResult> GetSolutions(
            IBaseRequest request,
            bool fullRecord = false)
        {
            var result = new SolutionsResult();

            try
            {
                var solutionsResponse = await solutionsRepository.GetAll(fullRecord);

                if (solutionsResponse.Success)
                {
                    if (request.PageListModel.SortBy == SortValue.NULL)
                    {
                        result.Solutions = solutionsResponse.Objects.ConvertAll(s => (ISudokuSolution)s);
                    }
                    else if (request.PageListModel.SortBy == SortValue.ID)
                    {
                        if (!request.PageListModel.OrderByDescending)
                        {
                            result.Solutions = (List<ISudokuSolution>)solutionsResponse.Objects
                                .ConvertAll(s => (ISudokuSolution)s)
                                .OrderBy(s => s.Id)
                                .Skip((request.PageListModel.Page - 1) * request.PageListModel.ItemsPerPage)
                                .Take(request.PageListModel.ItemsPerPage);
                        }
                        else
                        {
                            result.Solutions = (List<ISudokuSolution>)solutionsResponse.Objects
                                .ConvertAll(s => (ISudokuSolution)s)
                                .OrderByDescending(s => s.Id)
                                .Skip((request.PageListModel.Page - 1) * request.PageListModel.ItemsPerPage)
                                .Take(request.PageListModel.ItemsPerPage);
                        }
                    }
                    else if (request.PageListModel.SortBy == SortValue.DATECREATED)
                    {
                        if (!request.PageListModel.OrderByDescending)
                        {
                            result.Solutions = (List<ISudokuSolution>)solutionsResponse.Objects
                                .ConvertAll(s => (ISudokuSolution)s)
                                .OrderBy(s => s.DateCreated)
                                .Skip((request.PageListModel.Page - 1) * request.PageListModel.ItemsPerPage)
                                .Take(request.PageListModel.ItemsPerPage);
                        }
                        else
                        {
                            result.Solutions = (List<ISudokuSolution>)solutionsResponse.Objects
                                .ConvertAll(s => (ISudokuSolution)s)
                                .OrderByDescending(s => s.DateCreated)
                                .Skip((request.PageListModel.Page - 1) * request.PageListModel.ItemsPerPage)
                                .Take(request.PageListModel.ItemsPerPage);
                        }
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = sortValueNotImplementedMessage;

                        return result;
                    }

                    result.Success = solutionsResponse.Success;
                    result.Solutions = solutionsResponse.Objects.ConvertAll(s => (ISudokuSolution)s);

                    return result;
                }
                else if (!solutionsResponse.Success && solutionsResponse.Exception != null)
                {
                    result.Success = solutionsResponse.Success;
                    result.Message = solutionsResponse.Exception.Message;

                    return result;
                }
                else
                {
                    result.Success = false;
                    result.Message = solutionsNotFoundMessage;

                    return result;
                }
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Message = e.Message;

                return result;
            }
        }

        public async Task<ISolutionResult> Solve(
            ISolveRequest request)
        {
            var result = new SolutionResult();

            try
            {
                if (await usersRepository.HasEntity(request.UserId))
                {
                    var intList = new List<int>();

                    intList.AddRange(request.FirstRow);
                    intList.AddRange(request.SecondRow);
                    intList.AddRange(request.ThirdRow);
                    intList.AddRange(request.FourthRow);
                    intList.AddRange(request.FifthRow);
                    intList.AddRange(request.SixthRow);
                    intList.AddRange(request.SeventhRow);
                    intList.AddRange(request.EighthRow);
                    intList.AddRange(request.NinthRow);

                    var firstNonZeroValue = 0;
                    var firstNonZeroValueIndex = 0;

                    for (var i = 0; i < intList.Count - 1; i++)
                    {
                        if (intList[i] != 0)
                        {
                            firstNonZeroValue = intList[i];
                            firstNonZeroValueIndex = i;
                            i = intList.Count - 2;
                        }
                    }

                    var solvedSolutions = ((await solutionsRepository.GetSolvedSolutions()).Objects)
                        .ConvertAll(s => (SudokuSolution)s);

                    var solutionInDB = false;

                    if (solvedSolutions.Count > 0)
                    {
                        foreach (var solution in solvedSolutions)
                        {
                            if (solution.SolutionList.Count > 0)
                            {
                                var possibleSolution = true;

                                for (var i = 0; i < intList.Count - 1; i++)
                                {
                                    if (intList[i] != 0 && intList[i] != solution.SolutionList[i])
                                    {
                                        possibleSolution = false;
                                        break;
                                    }
                                }

                                if (possibleSolution)
                                {
                                    solutionInDB = possibleSolution;
                                    result.Success = true;
                                    result.Solution = solution;
                                    break;
                                }
                            }
                        }
                    }

                    if (!solutionInDB)
                    {
                        var sudokuSolver = new SudokuSolver(intList);

                        sudokuSolver.SetTimeLimit(request.Minutes);

                        await sudokuSolver.Solve();

                        if (sudokuSolver.IsValid())
                        {
                            var possibleResult = new SudokuSolution(sudokuSolver.ToInt32List());

                            var addResultToDataContext = true;

                            foreach (var solution in solvedSolutions)
                            {
                                if (solution.ToString().Equals(possibleResult.ToString()))
                                {
                                    addResultToDataContext = false;
                                }
                            }

                            if (addResultToDataContext && !possibleResult.ToString().Contains('0'))
                            {
                                possibleResult = (SudokuSolution)(await solutionsRepository.Create(possibleResult)).Object;

                                result.Success = true;
                                result.Solution = possibleResult;
                            }
                        }
                        else
                        {
                            result.Solution = null;
                            result.Success = true;
                        }
                    }

                    return result;
                }
                else
                {
                    result.Success = false;
                    result.Message = userNotFoundMessage;

                    return result;
                }
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Message = e.Message;

                return result;
            }
        }

        public async Task<ISolutionResult> Generate()
        {
            var result = new SolutionResult();

            var continueLoop = true;

            do
            {
                var matrix = new SudokuMatrix();

                matrix.GenerateSolution();

                var solutions = (List<SudokuSolution>)(await solutionsRepository.GetSolvedSolutions()).Object;

                var matrixNotInDB = true;

                if (solutions.Count > 0)
                {
                    foreach (var solution in solutions)
                    {
                        if (solution.SolutionList.Count > 0 && solution.ToString().Equals(matrix))
                        {
                            matrixNotInDB = false;
                        }
                    }
                }

                if (matrixNotInDB)
                {
                    result.Solution = new SudokuSolution(
                        matrix.ToInt32List());

                    continueLoop = false;
                }

            } while (continueLoop);

            var solutionResponse = await solutionsRepository.Create((SudokuSolution)result.Solution);

            if (solutionResponse.Success)
            {
                result.Success = solutionResponse.Success;
                result.Message = "Soltuion generated";

                return result;
            }
            else
            {
                result.Success = solutionResponse.Success;
                result.Message = "Unable to generate solution";

                return result;
            }
        }

        public async Task<IBaseResult> AddSolutions(int limitArg)
        {
            var result = new BaseResult();

            var limit = 1000;

            if (limitArg <= limit)
            {
                limit = limitArg;
            }

            var reduceLimitBy = 0;

            var solutionsInDB = new List<List<int>>();

            var solutions = (await solutionsRepository.GetSolvedSolutions())
                .Objects.ConvertAll(s => (SudokuSolution)s);

            foreach (var solution in solutions)
            {
                solutionsInDB.Add(solution.SolutionList);
            }

            var matrix = new SudokuMatrix();

            try
            {
                List<List<int>> solutionsList = new List<List<int>>();
                List<SudokuSolution> newSolutions = new List<SudokuSolution>();

                var continueLoop = true;

                do
                {
                    for (var i = 0; i < limit - reduceLimitBy; i++)
                    {
                        matrix.GenerateSolution();

                        if (!solutionsInDB.Contains(matrix.ToInt32List()))
                        {
                            solutionsList.Add(matrix.ToInt32List());
                        }
                    }

                    solutionsList = solutionsList
                        .Distinct()
                        .ToList();

                    if (limit == solutionsList.Count)
                    {
                        continueLoop = false;

                    }
                    else
                    {
                        reduceLimitBy = limit - solutionsList.Count;
                    }

                } while (continueLoop);

                foreach (var solutionList in solutionsList)
                {
                    newSolutions.Add(new SudokuSolution(solutionList));
                }

                var solutionsResponse = await solutionsRepository
                    .AddSolutions(newSolutions.ConvertAll(s => (ISudokuSolution)s));

                if (solutionsResponse.Success)
                {
                    result.Success = solutionsResponse.Success;
                    result.Message = solutionsAddedMessage;

                    return result;
                }
                else if (!solutionsResponse.Success && solutionsResponse.Exception != null)
                {
                    result.Success = solutionsResponse.Success;
                    result.Message = solutionsResponse.Exception.Message;

                    return result;
                }
                else
                {
                    result.Success = false;
                    result.Message = unableToAddSolutionsMessage;

                    return result;
                }

            }
            catch (Exception e)
            {
                result.Success = false;
                result.Message = e.Message;

                return result;
            }
        }
    }
}

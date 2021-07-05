using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Core.Models;
using SudokuCollective.Core.Interfaces.Repositories;
using SudokuCollective.Core.Enums;
using SudokuCollective.Data.Helpers;
using SudokuCollective.Data.Messages;
using SudokuCollective.Data.Models.ResultModels;
using SudokuCollective.Core.Extensions;

namespace SudokuCollective.Data.Services
{
    public class SolutionsService : ISolutionsService
    {
        #region Fields
        private readonly ISolutionsRepository<SudokuSolution> solutionsRepository;
        #endregion

        #region Constructor
        public SolutionsService(
            ISolutionsRepository<SudokuSolution> solutionsRepo)
        {
            solutionsRepository = solutionsRepo;
        }
        #endregion

        #region Methods
        public async Task<ISolutionResult> GetSolution(
            int id,
            bool fullRecord = true)
        {
            var result = new SolutionResult();

            try
            {
                var solutionResponse = await solutionsRepository.GetById(id, fullRecord);

                if (solutionResponse.Success)
                {
                    var solution = (SudokuSolution)solutionResponse.Object;

                    if (fullRecord)
                    {
                        solution.Game.SudokuMatrix.Difficulty.Matrices = new List<SudokuMatrix>();
                    }

                    result.Success = solutionResponse.Success;
                    result.Message = SolutionsMessages.SolutionFoundMessage;
                    result.Solution = solution;

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
                    result.Message = SolutionsMessages.SolutionNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<ISolutionsResult> GetSolutions(
            IBaseRequest request,
            bool fullRecord = true)
        {
            var result = new SolutionsResult();

            try
            {
                var response = await solutionsRepository.GetAll(fullRecord);

                if (response.Success)
                {
                    if (request.Paginator != null)
                    {
                        if (StaticDataHelpers.IsPageValid(request.Paginator, response.Objects))
                        {
                            if (request.Paginator.SortBy == SortValue.NULL)
                            {
                                result.Solutions = response.Objects.ConvertAll(s => (ISudokuSolution)s);
                            }
                            else if (request.Paginator.SortBy == SortValue.ID)
                            {
                                if (!request.Paginator.OrderByDescending)
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Solutions.Add((ISudokuSolution)obj);
                                    }

                                    result.Solutions = result.Solutions
                                        .OrderBy(s => s.Id)
                                        .Skip((request.Paginator.Page - 1) * request.Paginator.ItemsPerPage)
                                        .Take(request.Paginator.ItemsPerPage)
                                        .ToList();
                                }
                                else
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Solutions.Add((ISudokuSolution)obj);
                                    }

                                    result.Solutions = result.Solutions
                                        .OrderByDescending(s => s.Id)
                                        .Skip((request.Paginator.Page - 1) * request.Paginator.ItemsPerPage)
                                        .Take(request.Paginator.ItemsPerPage)
                                        .ToList();
                                }
                            }
                            else if (request.Paginator.SortBy == SortValue.DATECREATED)
                            {
                                if (!request.Paginator.OrderByDescending)
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Solutions.Add((ISudokuSolution)obj);
                                    }

                                    result.Solutions = result.Solutions
                                        .OrderBy(s => s.DateCreated)
                                        .Skip((request.Paginator.Page - 1) * request.Paginator.ItemsPerPage)
                                        .Take(request.Paginator.ItemsPerPage)
                                        .ToList();
                                }
                                else
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Solutions.Add((ISudokuSolution)obj);
                                    }

                                    result.Solutions = result.Solutions
                                        .OrderByDescending(s => s.DateCreated)
                                        .Skip((request.Paginator.Page - 1) * request.Paginator.ItemsPerPage)
                                        .Take(request.Paginator.ItemsPerPage)
                                        .ToList();
                                }
                            }
                            else if (request.Paginator.SortBy == SortValue.DATEUPDATED)
                            {
                                if (!request.Paginator.OrderByDescending)
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Solutions.Add((ISudokuSolution)obj);
                                    }

                                    result.Solutions = result.Solutions
                                        .OrderBy(s => s.DateSolved)
                                        .Skip((request.Paginator.Page - 1) * request.Paginator.ItemsPerPage)
                                        .Take(request.Paginator.ItemsPerPage)
                                        .ToList();
                                }
                                else
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Solutions.Add((ISudokuSolution)obj);
                                    }

                                    result.Solutions = result.Solutions
                                        .OrderByDescending(g => g.DateSolved)
                                        .Skip((request.Paginator.Page - 1) * request.Paginator.ItemsPerPage)
                                        .Take(request.Paginator.ItemsPerPage)
                                        .ToList();
                                }
                            }
                            else
                            {
                                result.Success = false;
                                result.Message = ServicesMesages.SortValueNotImplementedMessage;

                                return result;
                            }
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = ServicesMesages.PageNotFoundMessage;

                            return result;
                        }
                    }
                    else
                    {
                        result.Solutions = response.Objects.ConvertAll(s => (ISudokuSolution)s);
                    }

                    if (fullRecord)
                    {
                        foreach (var solution in result.Solutions)
                        {
                            if (solution.Game != null)
                            {
                                solution.Game.SudokuMatrix.Difficulty.Matrices = new List<SudokuMatrix>();
                            }
                        }
                    }

                    result.Success = response.Success;
                    result.Message = SolutionsMessages.SolutionsFoundMessage;

                    return result;
                }
                else if (!response.Success && response.Exception != null)
                {
                    result.Success = response.Success;
                    result.Message = response.Exception.Message;

                    return result;
                }
                else
                {
                    result.Success = false;
                    result.Message = SolutionsMessages.SolutionsNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<ISolutionResult> Solve(
            ISolveRequest request)
        {
            var result = new SolutionResult();

            try
            {
                var solvedSolutions = ((await solutionsRepository.GetSolvedSolutions()).Objects)
                    .ConvertAll(s => (SudokuSolution)s);

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

                var sudokuSolver = new SudokuMatrix(intList);

                await sudokuSolver.Solve();

                if (sudokuSolver.IsValid())
                {
                    var solution = new SudokuSolution(sudokuSolver.ToIntList());

                    var addResultToDataContext = true;

                    if (solvedSolutions.Count > 0)
                    {
                        foreach (var solvedSolution in solvedSolutions)
                        {
                            if (solvedSolution.ToString().Equals(solution.ToString()))
                            {
                                addResultToDataContext = false;
                            }
                        }
                    }

                    if (addResultToDataContext)
                    {
                        solution = (SudokuSolution)(await solutionsRepository.Create(solution)).Object;
                    }
                    else
                    {
                        solution = solvedSolutions.Where(s => s.SolutionList.IsThisListEqual(solution.SolutionList)).FirstOrDefault();
                    }

                    result.Success = true;
                    result.Solution = solution;
                    result.Message = SolutionsMessages.SudokuSolutionFoundMessage;
                }
                else
                {
                    intList = sudokuSolver.ToIntList();

                    if (solvedSolutions.Count > 0)
                    {
                        var solutonInDB = false;

                        foreach (var solution in solvedSolutions)
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
                                solutonInDB = possibleSolution;
                                result.Success = possibleSolution;
                                result.Solution = solution;
                                result.Message = SolutionsMessages.SudokuSolutionFoundMessage;
                                break;
                            }
                        }

                        if (!solutonInDB)
                        {
                            result.Success = false;
                            result.Solution = null;
                            result.Message = SolutionsMessages.SudokuSolutionNotFoundMessage;
                        }
                    }
                    else
                    {
                        result.Success = false;
                        result.Solution = null;
                        result.Message = SolutionsMessages.SudokuSolutionNotFoundMessage;
                    }
                }

                return result;
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Message = exp.Message;

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

                var response = (await solutionsRepository.GetSolvedSolutions());

                var matrixNotInDB = true;

                if (response.Success)
                {
                    foreach (var solution in response.Objects.ConvertAll(s => (SudokuSolution)s))
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
                        matrix.ToIntList());

                    continueLoop = false;
                }

            } while (continueLoop);

            var solutionResponse = await solutionsRepository.Create((SudokuSolution)result.Solution);

            if (solutionResponse.Success)
            {
                result.Success = solutionResponse.Success;
                result.Message = SolutionsMessages.SolutionGeneratedMessage;

                return result;
            }
            else
            {
                result.Success = solutionResponse.Success;
                result.Message = SolutionsMessages.SolutionNotGeneratedMessage;

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

            try
            {
                var solutions = (await solutionsRepository.GetSolvedSolutions())
                    .Objects.ConvertAll(s => (SudokuSolution)s);

                foreach (var solution in solutions)
                {
                    solutionsInDB.Add(solution.SolutionList);
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Message = exp.Message;

                return result;
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

                        if (!solutionsInDB.Contains(matrix.ToIntList()))
                        {
                            solutionsList.Add(matrix.ToIntList());
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
                    result.Message = SolutionsMessages.SolutionsAddedMessage;

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
                    result.Message = SolutionsMessages.SolutionsNotAddedMessage;

                    return result;
                }

            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Message = exp.Message;

                return result;
            }
        }
        #endregion
    }
}

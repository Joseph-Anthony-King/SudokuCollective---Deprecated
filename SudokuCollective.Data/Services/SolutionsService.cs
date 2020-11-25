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

namespace SudokuCollective.Data.Services
{
    public class SolutionsService : ISolutionsService
    {
        #region Fields
        private readonly ISolutionsRepository<SudokuSolution> solutionsRepository;
        private readonly IUsersRepository<User> usersRepository;
        #endregion

        #region Constructor
        public SolutionsService(
            ISolutionsRepository<SudokuSolution> solutionsRepo,
            IUsersRepository<User> usersRepo)
        {
            solutionsRepository = solutionsRepo;
            usersRepository = usersRepo;
        }
        #endregion

        #region Methods
        public async Task<ISolutionResult> GetSolution(
            int id, bool fullRecord = true)
        {
            var result = new SolutionResult();

            try
            {
                var solutionResponse = await solutionsRepository.GetById(id, fullRecord);

                if (solutionResponse.Success)
                {
                    result.Success = solutionResponse.Success;
                    result.Message = SolutionsMessages.SolutionFoundMessage;
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
                    if (request.PageListModel != null)
                    {
                        if (StaticDataHelpers.IsPageValid(request.PageListModel, response.Objects))
                        {
                            if (request.PageListModel.SortBy == SortValue.NULL)
                            {
                                result.Solutions = response.Objects.ConvertAll(s => (ISudokuSolution)s);
                            }
                            else if (request.PageListModel.SortBy == SortValue.ID)
                            {
                                if (!request.PageListModel.OrderByDescending)
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Solutions.Add((ISudokuSolution)obj);
                                    }

                                    result.Solutions = result.Solutions
                                        .OrderBy(s => s.Id)
                                        .Skip((request.PageListModel.Page - 1) * request.PageListModel.ItemsPerPage)
                                        .Take(request.PageListModel.ItemsPerPage)
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
                                        .Skip((request.PageListModel.Page - 1) * request.PageListModel.ItemsPerPage)
                                        .Take(request.PageListModel.ItemsPerPage)
                                        .ToList();
                                }
                            }
                            else if (request.PageListModel.SortBy == SortValue.DATECREATED)
                            {
                                if (!request.PageListModel.OrderByDescending)
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Solutions.Add((ISudokuSolution)obj);
                                    }

                                    result.Solutions = result.Solutions
                                        .OrderBy(s => s.DateCreated)
                                        .Skip((request.PageListModel.Page - 1) * request.PageListModel.ItemsPerPage)
                                        .Take(request.PageListModel.ItemsPerPage)
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
                                        .Skip((request.PageListModel.Page - 1) * request.PageListModel.ItemsPerPage)
                                        .Take(request.PageListModel.ItemsPerPage)
                                        .ToList();
                                }
                            }
                            else if (request.PageListModel.SortBy == SortValue.DATEUPDATED)
                            {
                                if (!request.PageListModel.OrderByDescending)
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Solutions.Add((ISudokuSolution)obj);
                                    }

                                    result.Solutions = result.Solutions
                                        .OrderBy(s => s.DateSolved)
                                        .Skip((request.PageListModel.Page - 1) * request.PageListModel.ItemsPerPage)
                                        .Take(request.PageListModel.ItemsPerPage)
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
                                        .Skip((request.PageListModel.Page - 1) * request.PageListModel.ItemsPerPage)
                                        .Take(request.PageListModel.ItemsPerPage)
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
                                    result.Success = possibleSolution;
                                    result.Solution = solution;
                                    result.Message = SolutionsMessages.SolutionSolvedMessage;
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
                                result.Message = SolutionsMessages.SolutionSolvedMessage;
                            }
                        }
                        else
                        {
                            result.Success = true;
                            result.Solution = null;
                            result.Message = SolutionsMessages.SolutionNotSolvedMessage;
                        }
                    }

                    return result;
                }
                else
                {
                    result.Success = false;
                    result.Message = UsersMessages.UserNotFoundMessage;

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
                        matrix.ToInt32List());

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

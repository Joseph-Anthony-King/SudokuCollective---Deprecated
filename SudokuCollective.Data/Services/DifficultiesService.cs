﻿using System;
using System.Linq;
using System.Threading.Tasks;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Core.Models;
using SudokuCollective.Core.Interfaces.Repositories;
using SudokuCollective.Core.Interfaces.APIModels.PageModels;
using SudokuCollective.Data.Messages;
using SudokuCollective.Data.Models.ResultModels;

namespace SudokuCollective.Data.Services
{
    public class DifficultiesService : IDifficultiesService
    {
        #region Fields
        private readonly IDifficultiesRepository<Difficulty> difficultiesRepository;
        #endregion

        #region Constructor
        public DifficultiesService(IDifficultiesRepository<Difficulty> difficultiesRepo)
        {
            difficultiesRepository = difficultiesRepo;
        }
        #endregion

        #region Methods
        public async Task<IDifficultyResult> GetDifficulty(
            int id, bool fullRecord = true)
        {
            var result = new DifficultyResult();

            if (id == 0)
            {
                result.Success = false;
                result.Message = DifficultiesMessages.DifficultiesNotFoundMessage;

                return result;
            }

            try
            {
                var response = await difficultiesRepository.GetById(id, fullRecord);

                if (response.Success)
                {
                    result.Success = response.Success;
                    result.Message = DifficultiesMessages.DifficultyFoundMessage;
                    result.Difficulty = (Difficulty)response.Object;

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
                    result.Message = DifficultiesMessages.DifficultyNotFoundMessage;

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

        public async Task<IDifficultiesResult> GetDifficulties(
            IPaginator paginator,
            bool fullRecord = true)
        {
            if (paginator == null) throw new ArgumentNullException(nameof(paginator));

            var result = new DifficultiesResult();

            try
            {
                var response = await difficultiesRepository.GetAll(fullRecord);

                if (response.Success)
                {
                    if (paginator.SortBy == SortValue.NULL)
                    {
                        result.Difficulties = response.Objects.ConvertAll(d => (IDifficulty)d);
                    }
                    else if (paginator.SortBy == SortValue.ID)
                    {
                        if (!paginator.OrderByDescending)
                        {
                            foreach (var obj in response.Objects)
                            {
                                result.Difficulties.Add((IDifficulty)obj);
                            }

                            result.Difficulties = result.Difficulties
                                .OrderBy(d => d.Id)
                                .ToList();
                        }
                        else
                        {
                            foreach (var obj in response.Objects)
                            {
                                result.Difficulties.Add((IDifficulty)obj);
                            }

                            result.Difficulties = result.Difficulties
                                .OrderByDescending(g => g.Id)
                                .ToList();
                        }
                    }
                    else if (paginator.SortBy == SortValue.NAME)
                    {
                        if (!paginator.OrderByDescending)
                        {
                            foreach (var obj in response.Objects)
                            {
                                result.Difficulties.Add((IDifficulty)obj);
                            }

                            result.Difficulties = result.Difficulties
                                .OrderBy(d => d.DisplayName)
                                .ToList();
                        }
                        else
                        {
                            foreach (var obj in response.Objects)
                            {
                                result.Difficulties.Add((IDifficulty)obj);
                            }

                            result.Difficulties = result.Difficulties
                                .OrderByDescending(g => g.DisplayName)
                                .ToList();
                        }
                    }
                    else if (paginator.SortBy == SortValue.DIFFICULTYLEVEL)
                    {
                        if (!paginator.OrderByDescending)
                        {
                            foreach (var obj in response.Objects)
                            {
                                result.Difficulties.Add((IDifficulty)obj);
                            }

                            result.Difficulties = result.Difficulties
                                .OrderBy(d => d.DifficultyLevel)
                                .ToList();
                        }
                        else
                        {
                            foreach (var obj in response.Objects)
                            {
                                result.Difficulties.Add((IDifficulty)obj);
                            }

                            result.Difficulties = result.Difficulties
                                .OrderByDescending(g => g.DifficultyLevel)
                                .ToList();
                        }
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = ServicesMesages.SortValueNotImplementedMessage;

                        return result;
                    }

                    result.Success = response.Success;
                    result.Message = DifficultiesMessages.DifficultiesFoundMessage;

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
                    result.Message = DifficultiesMessages.DifficultiesNotFoundMessage;

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

        public async Task<IDifficultyResult> CreateDifficulty(
            string name,
            string displayName,
            DifficultyLevel difficultyLevel)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            if (string.IsNullOrEmpty(displayName)) throw new ArgumentNullException(nameof(displayName));

            var result = new DifficultyResult();

            try
            {
                if (!(await difficultiesRepository.HasDifficultyLevel(difficultyLevel)))
                {

                    var difficulty = new Difficulty()
                    {
                        Name = name,
                        DisplayName = displayName,
                        DifficultyLevel = difficultyLevel
                    };

                    var response = await difficultiesRepository.Add(difficulty);

                    if (response.Success)
                    {
                        result.Success = response.Success;
                        result.Message = DifficultiesMessages.DifficultyCreatedMessage;
                        result.Difficulty = (IDifficulty)response.Object;

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
                        result.Message = DifficultiesMessages.DifficultyNotCreatedMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = DifficultiesMessages.DifficultyAlreadyExistsMessage;

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

        public async Task<IBaseResult> UpdateDifficulty(
            int id,
            IUpdateDifficultyRequest updateDifficultyRequest)
        {
            if (updateDifficultyRequest == null) throw new ArgumentNullException(nameof(updateDifficultyRequest));

            var result = new BaseResult();

            if (id == 0)
            {
                result.Success = false;
                result.Message = DifficultiesMessages.DifficultiesNotFoundMessage;

                return result;
            }

            try
            {
                var response = await difficultiesRepository.GetById(id);

                if (response.Success)
                {
                    ((Difficulty)response.Object).Name = updateDifficultyRequest.Name;
                    ((Difficulty)response.Object).DisplayName = updateDifficultyRequest.DisplayName;

                    var updateDifficultyResponse = await difficultiesRepository
                        .Update((Difficulty)response.Object);

                    if (updateDifficultyResponse.Success)
                    {
                        result.Success = updateDifficultyResponse.Success;
                        result.Message = DifficultiesMessages.DifficultyUpdatedMessage;

                        return result;
                    }
                    else if (!updateDifficultyResponse.Success && updateDifficultyResponse.Exception != null)
                    {
                        result.Success = updateDifficultyResponse.Success;
                        result.Message = updateDifficultyResponse.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = DifficultiesMessages.DifficultyNotUpdatedMessage;

                        return result;
                    }

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
                    result.Message = DifficultiesMessages.DifficultyNotFoundMessage;

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

        public async Task<IBaseResult> DeleteDifficulty(int id)
        {
            var result = new BaseResult();

            if (id == 0)
            {
                result.Success = false;
                result.Message = DifficultiesMessages.DifficultiesNotFoundMessage;

                return result;
            }

            try
            {
                var response = await difficultiesRepository.GetById(id, true);

                if (response.Success)
                {
                    var updateDeleteResponse = await difficultiesRepository.Delete((Difficulty)response.Object);

                    if (updateDeleteResponse.Success)
                    {
                        result.Success = updateDeleteResponse.Success;
                        result.Message = DifficultiesMessages.DifficultyDeletedMessage;

                        return result;
                    }
                    else if (!updateDeleteResponse.Success && updateDeleteResponse.Exception != null)
                    {
                        result.Success = updateDeleteResponse.Success;
                        result.Message = updateDeleteResponse.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = DifficultiesMessages.DifficultyNotDeletedMessage;

                        return result;
                    }

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
                    result.Message = DifficultiesMessages.DifficultyNotFoundMessage;

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

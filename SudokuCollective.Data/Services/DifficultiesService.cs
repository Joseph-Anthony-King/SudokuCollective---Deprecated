using System;
using System.Threading.Tasks;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Data.Models.ResultModels;
using SudokuCollective.Core.Models;
using SudokuCollective.Core.Interfaces.Repositories;

namespace SudokuCollective.Data.Services
{
    public class DifficultiesService : IDifficultiesService
    {
        private readonly IDifficultiesRepository<Difficulty> difficultiesRepository;
        private readonly string difficultyNotFoundMessage;
        private readonly string difficultiesNotFoundMessage;
        private readonly string unableToAddDifficultyMessage;
        private readonly string difficultyAlreadyExistsMessage;
        private readonly string unableToUpdateDifficultyMessage;
        private readonly string unableToDeleteDifficultyMessage;

        public DifficultiesService(IDifficultiesRepository<Difficulty> difficultiesRepo)
        {
            difficultiesRepository = difficultiesRepo;
            difficultyNotFoundMessage = "Difficulty not found";
            difficultiesNotFoundMessage = "Difficulties not found";
            unableToAddDifficultyMessage = "Unable to add difficulty";
            difficultyAlreadyExistsMessage = "Difficulty already exists";
            unableToUpdateDifficultyMessage = "Unable to update difficulty";
            unableToDeleteDifficultyMessage = "Unable to delete difficulty";
        }

        public async Task<IDifficultyResult> GetDifficulty(
            int id, bool fullRecord = true)
        {
            var result = new DifficultyResult();

            try
            {
                var difficutlyResponse = await difficultiesRepository.GetById(id, fullRecord);

                if (difficutlyResponse.Success)
                {
                    result.Success = difficutlyResponse.Success;
                    result.Difficulty = (Difficulty)difficutlyResponse.Object;

                    return result;
                }
                else if (!difficutlyResponse.Success && difficutlyResponse.Exception != null)
                {
                    result.Success = difficutlyResponse.Success;
                    result.Message = difficutlyResponse.Exception.Message;

                    return result;
                }
                else
                {
                    result.Success = false;
                    result.Message = difficultyNotFoundMessage;

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

        public async Task<IDifficultiesResult> GetDifficulties(
            bool fullRecord = true)
        {
            var result = new DifficultiesResult();

            try
            {
                var difficutliesResponse = await difficultiesRepository.GetAll(fullRecord);

                if (difficutliesResponse.Success)
                {
                    result.Success = difficutliesResponse.Success;
                    result.Difficulties = difficutliesResponse.Objects.ConvertAll(d => (IDifficulty)d);

                    return result;
                }
                else if (!difficutliesResponse.Success && difficutliesResponse.Exception != null)
                {
                    result.Success = difficutliesResponse.Success;
                    result.Message = difficutliesResponse.Exception.Message;

                    return result;
                }
                else
                {
                    result.Success = false;
                    result.Message = difficultiesNotFoundMessage;

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

        public async Task<IDifficultyResult> CreateDifficulty(
            string name, 
            DifficultyLevel difficultyLevel)
        {
            var result = new DifficultyResult();

            try
            {
                if (!(await difficultiesRepository.HasDifficultyLevel(difficultyLevel)))
                {

                    var difficulty = new Difficulty()
                    {
                        Name = name,
                        DifficultyLevel = difficultyLevel
                    };

                    var difficultyResponse = await difficultiesRepository.Create(difficulty);

                    if (difficultyResponse.Success)
                    {
                        result.Success = difficultyResponse.Success;
                        result.Difficulty = (IDifficulty)difficultyResponse.Object;

                        return result;
                    }
                    else if (!difficultyResponse.Success && difficultyResponse.Exception != null)
                    {
                        result.Success = difficultyResponse.Success;
                        result.Message = difficultyResponse.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = unableToAddDifficultyMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = difficultyAlreadyExistsMessage;

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

        public async Task<IBaseResult> UpdateDifficulty(int id,
            IUpdateDifficultyRequest updateDifficultyRequest)
        {
            var result = new BaseResult();

            try
            {
                if (await difficultiesRepository.HasEntity(id))
                {
                    var difficultyResponse = await difficultiesRepository.GetById(id);

                    if (difficultyResponse.Success)
                    {
                        ((Difficulty)difficultyResponse.Object).Name = updateDifficultyRequest.Name;
                        ((Difficulty)difficultyResponse.Object).DisplayName = updateDifficultyRequest.DisplayName;

                        var updateDifficultyResponse = await difficultiesRepository
                            .Update((Difficulty)difficultyResponse.Object);

                        if (updateDifficultyResponse.Success)
                        {
                            result.Success = updateDifficultyResponse.Success;

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
                            result.Message = unableToUpdateDifficultyMessage;

                            return result;
                        }

                    }
                    else if (!difficultyResponse.Success && difficultyResponse.Exception != null)
                    {
                        result.Success = difficultyResponse.Success;
                        result.Message = difficultyResponse.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = difficultyNotFoundMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = difficultyNotFoundMessage;

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

        public async Task<IBaseResult> DeleteDifficulty(int id)
        {
            var result = new BaseResult();

            try
            {
                if (await difficultiesRepository.HasEntity(id))
                {
                    var difficultyResponse = await difficultiesRepository.GetById(id);

                    if (difficultyResponse.Success)
                    {
                        var updateDeleteResponse = await difficultiesRepository.Delete((Difficulty)difficultyResponse.Object);

                        if (updateDeleteResponse.Success)
                        {
                            result.Success = updateDeleteResponse.Success;

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
                            result.Message = unableToDeleteDifficultyMessage;

                            return result;
                        }

                    }
                    else if (!difficultyResponse.Success && difficultyResponse.Exception != null)
                    {
                        result.Success = difficultyResponse.Success;
                        result.Message = difficultyResponse.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = difficultyNotFoundMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = difficultyNotFoundMessage;

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

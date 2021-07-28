using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Core.Models;
using SudokuCollective.Core.Interfaces.Repositories;
using SudokuCollective.Data.Messages;
using SudokuCollective.Data.Models.ResultModels;
using SudokuCollective.Data.Resiliency;
using SudokuCollective.Data.Models.DataModels;

namespace SudokuCollective.Data.Services
{
    public class DifficultiesService : IDifficultiesService
    {
        #region Fields
        private readonly IDifficultiesRepository<Difficulty> _difficultiesRepository;
        private readonly IDistributedCache _distributedCache;
        #endregion

        #region Constructor
        public DifficultiesService(
            IDifficultiesRepository<Difficulty> difficultiesRepository,
            IDistributedCache distributedCache)
        {
            _difficultiesRepository = difficultiesRepository;
            _distributedCache = distributedCache;
        }
        #endregion

        #region Methods
        public async Task<IDifficultyResult> Create(
            string name,
            string displayName,
            DifficultyLevel difficultyLevel)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            if (string.IsNullOrEmpty(displayName)) throw new ArgumentNullException(nameof(displayName));

            var result = new DifficultyResult();

            try
            {
                if (!await CacheFactory.HasDifficultyLevelWithCacheAsync(
                    _difficultiesRepository,
                    _distributedCache,
                    string.Format(CacheKeys.GetDifficulty, difficultyLevel),
                    CachingStrategy.Heavy,
                    difficultyLevel))
                {

                    var difficulty = new Difficulty()
                    {
                        Name = name,
                        DisplayName = displayName,
                        DifficultyLevel = difficultyLevel
                    };

                    var response = await CacheFactory.AddWithCacheAsync<Difficulty>(
                        _difficultiesRepository,
                        _distributedCache,
                        CacheKeys.GetDifficulty,
                        CachingStrategy.Heavy,
                        difficulty);

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

        public async Task<IDifficultyResult> Get(
            int id)
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
                var cacheFactoryResponse = await CacheFactory.GetWithCacheAsync(
                    _difficultiesRepository,
                    _distributedCache,
                    string.Format(CacheKeys.GetDifficulty, id),
                    CachingStrategy.Heavy,
                    id,
                    result);

                var response = (RepositoryResponse)cacheFactoryResponse.Item1;
                result = (DifficultyResult)cacheFactoryResponse.Item2;

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

        public async Task<IBaseResult> Update(
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
                var response = await _difficultiesRepository.Get(id);

                if (response.Success)
                {
                    ((Difficulty)response.Object).Name = updateDifficultyRequest.Name;
                    ((Difficulty)response.Object).DisplayName = updateDifficultyRequest.DisplayName;

                    var updateDifficultyResponse = await CacheFactory.UpdateWithCacheAsync(
                        _difficultiesRepository,
                        _distributedCache,
                        (Difficulty)response.Object);

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

        public async Task<IBaseResult> Delete(int id)
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
                var response = await _difficultiesRepository.Get(id);

                if (response.Success)
                {
                    var updateDeleteResponse = await CacheFactory.DeleteWithCacheAsync(
                        _difficultiesRepository,
                        _distributedCache,
                        (Difficulty)response.Object);

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

        public async Task<IDifficultiesResult> GetDifficulties()
        {
            var result = new DifficultiesResult();

            try
            {
                var cacheFactoryResponse = await CacheFactory.GetAllWithCacheAsync(
                    _difficultiesRepository,
                    _distributedCache,
                    CacheKeys.GetDifficulties,
                    CachingStrategy.Heavy,
                    result);

                var response = (RepositoryResponse)cacheFactoryResponse.Item1;
                result = (DifficultiesResult)cacheFactoryResponse.Item2;

                if (response.Success)
                {
                    result.Difficulties = response.Objects.ConvertAll(d => (IDifficulty)d);

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
        #endregion
    }
}

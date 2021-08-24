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
    public class RolesService : IRolesService
    {
        #region Fields
        private readonly IRolesRepository<Role> _rolesRepository;
        private readonly IDistributedCache _distributedCache;
        #endregion

        #region Constructor
        public RolesService(
            IRolesRepository<Role> rolesRepository,
            IDistributedCache distributedCache)
        {
            _rolesRepository = rolesRepository;
            _distributedCache = distributedCache;
        }
        #endregion

        #region Methods
        public async Task<IRoleResult> Create(
            string name,
            RoleLevel roleLevel)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            var result = new RoleResult();

            try
            {
                if (!await CacheFactory.HasRoleLevelWithCacheAsync(
                    _rolesRepository,
                    _distributedCache,
                    string.Format(CacheKeys.GetRole, roleLevel),
                    CachingStrategy.Heavy,
                    roleLevel))
                {
                    var role = new Role()
                    {
                        Name = name,
                        RoleLevel = roleLevel
                    };

                    var response = await CacheFactory.AddWithCacheAsync(
                        _rolesRepository,
                        _distributedCache,
                        CacheKeys.GetRole,
                        CachingStrategy.Heavy,
                        role);

                    if (response.Success)
                    {
                        result.IsSuccess = response.Success;
                        result.Message = RolesMessages.RoleCreatedMessage;
                        result.Role = (IRole)response.Object;

                        return result;
                    }
                    else if (!response.Success && response.Exception != null)
                    {
                        result.IsSuccess = response.Success;
                        result.Message = response.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = RolesMessages.RoleNotCreatedMessage;

                        return result;
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = RolesMessages.RoleAlreadyExistsMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.IsSuccess = false;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IRoleResult> Get(int id)
        {
            var result = new RoleResult();

            if (id == 0)
            {
                result.IsSuccess = false;
                result.Message = RolesMessages.RoleNotFoundMessage;

                return result;
            }

            try
            {
                var cacheFactoryResponse = await CacheFactory.GetWithCacheAsync<Role>(
                    _rolesRepository,
                    _distributedCache,
                    string.Format(CacheKeys.GetRole, id),
                    CachingStrategy.Heavy,
                    id,
                    result);

                var response = (RepositoryResponse)cacheFactoryResponse.Item1;
                result = (RoleResult)cacheFactoryResponse.Item2;

                if (response.Success)
                {
                    var role = (Role)response.Object;

                    result.IsSuccess = response.Success;
                    result.Message = RolesMessages.RoleFoundMessage;
                    result.Role = role;

                    return result;
                }
                else if (!response.Success && response.Exception != null)
                {
                    result.IsSuccess = response.Success;
                    result.Message = response.Exception.Message;

                    return result;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = RolesMessages.RoleNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.IsSuccess = false;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IBaseResult> Update(
            int id, 
            IUpdateRoleRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var result = new BaseResult();

            if (id == 0)
            {
                result.IsSuccess = false;
                result.Message = RolesMessages.RoleNotFoundMessage;

                return result;
            }

            try
            {
                var cacheFactoryResponse = await CacheFactory.GetWithCacheAsync<Role>(
                    _rolesRepository,
                    _distributedCache,
                    string.Format(CacheKeys.GetRole, id),
                    CachingStrategy.Heavy,
                    id,
                    result);

                var response = (RepositoryResponse)cacheFactoryResponse.Item1;
                result = (BaseResult)cacheFactoryResponse.Item2;

                if (response.Success)
                {
                    ((Role)response.Object).Name = request.Name;

                    var updateResponse = await CacheFactory.UpdateWithCacheAsync(
                        _rolesRepository,
                        _distributedCache,
                        (Role)response.Object);

                    if (updateResponse.Success)
                    {
                        result.IsSuccess = updateResponse.Success;
                        result.Message = RolesMessages.RoleUpdatedMessage;

                        return result;
                    }
                    else if (!updateResponse.Success && updateResponse.Exception != null)
                    {
                        result.IsSuccess = updateResponse.Success;
                        result.Message = updateResponse.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = RolesMessages.RoleNotUpdatedMessage;

                        return result;
                    }

                }
                else if (!response.Success && response.Exception != null)
                {
                    result.IsSuccess = response.Success;
                    result.Message = response.Exception.Message;

                    return result;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = RolesMessages.RoleNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.IsSuccess = false;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IBaseResult> Delete(int id)
        {
            var result = new BaseResult();

            if (id == 0)
            {
                result.IsSuccess = false;
                result.Message = RolesMessages.RoleNotFoundMessage;

                return result;
            }

            try
            {
                var response = await _rolesRepository.Get(id);

                if (response.Success)
                {
                    var deleteResponse = await CacheFactory.DeleteWithCacheAsync(
                        _rolesRepository,
                        _distributedCache,
                        (Role)response.Object);

                    if (deleteResponse.Success)
                    {
                        result.IsSuccess = deleteResponse.Success;
                        result.Message = RolesMessages.RoleDeletedMessage;

                        return result;
                    }
                    else if (!deleteResponse.Success && deleteResponse.Exception != null)
                    {
                        result.IsSuccess = deleteResponse.Success;
                        result.Message = deleteResponse.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = RolesMessages.RoleNotDeletedMessage;

                        return result;
                    }

                }
                else if (!response.Success && response.Exception != null)
                {
                    result.IsSuccess = response.Success;
                    result.Message = response.Exception.Message;

                    return result;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = RolesMessages.RoleNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.IsSuccess = false;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IRolesResult> GetRoles()
        {
            var result = new RolesResult();

            try
            {
                var cacheFactoryResponse = await CacheFactory.GetAllWithCacheAsync<Role>(
                    _rolesRepository,
                    _distributedCache,
                    string.Format(CacheKeys.GetRoles),
                    CachingStrategy.Heavy,
                    result);

                var response = (RepositoryResponse)cacheFactoryResponse.Item1;
                result = (RolesResult)cacheFactoryResponse.Item2;

                if (response.Success)
                {
                    var roles = response.Objects.ConvertAll(r => (IRole)r);

                    result.IsSuccess = response.Success;
                    result.Message = RolesMessages.RolesFoundMessage;
                    result.Roles = roles;

                    return result;
                }
                else if (!response.Success && response.Exception != null)
                {
                    result.IsSuccess = response.Success;
                    result.Message = response.Exception.Message;

                    return result;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = RolesMessages.RolesNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.IsSuccess = false;
                result.Message = exp.Message;

                return result;
            }
        }
        #endregion
    }
}

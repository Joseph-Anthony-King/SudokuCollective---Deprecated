﻿using System;
using System.Threading.Tasks;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Core.Models;
using SudokuCollective.Core.Interfaces.Repositories;
using SudokuCollective.Data.Messages;
using SudokuCollective.Data.Models.ResultModels;

namespace SudokuCollective.Data.Services
{
    public class RolesService : IRolesService
    {
        #region Fields
        private readonly IRolesRepository<Role> _rolesRepository;
        #endregion

        #region Constructor
        public RolesService(IRolesRepository<Role> rolesRepository)
        {
            _rolesRepository = rolesRepository;
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
                if (!(await _rolesRepository.HasRoleLevel(roleLevel)))
                {
                    var role = new Role()
                    {
                        Name = name,
                        RoleLevel = roleLevel
                    };

                    var response = await _rolesRepository.Add(role);

                    if (response.Success)
                    {
                        result.Success = response.Success;
                        result.Message = RolesMessages.RoleCreatedMessage;
                        result.Role = (IRole)response.Object;

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
                        result.Message = RolesMessages.RoleNotCreatedMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = RolesMessages.RoleAlreadyExistsMessage;

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

        public async Task<IRoleResult> Get(int id)
        {
            var result = new RoleResult();

            if (id == 0)
            {
                result.Success = false;
                result.Message = RolesMessages.RoleNotFoundMessage;

                return result;
            }

            try
            {
                var response = await _rolesRepository.Get(id);

                if (response.Success)
                {
                    var role = (Role)response.Object;

                    result.Success = response.Success;
                    result.Message = RolesMessages.RoleFoundMessage;
                    result.Role = role;

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
                    result.Message = RolesMessages.RoleNotFoundMessage;

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
            IUpdateRoleRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var result = new BaseResult();

            if (id == 0)
            {
                result.Success = false;
                result.Message = RolesMessages.RoleNotFoundMessage;

                return result;
            }

            try
            {
                var response = await _rolesRepository.Get(id);

                if (response.Success)
                {
                    ((Role)response.Object).Name = request.Name;

                    var updateResponse = await _rolesRepository
                        .Update((Role)response.Object);

                    if (updateResponse.Success)
                    {
                        result.Success = updateResponse.Success;
                        result.Message = RolesMessages.RoleUpdatedMessage;

                        return result;
                    }
                    else if (!updateResponse.Success && updateResponse.Exception != null)
                    {
                        result.Success = updateResponse.Success;
                        result.Message = updateResponse.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = RolesMessages.RoleNotUpdatedMessage;

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
                    result.Message = RolesMessages.RoleNotFoundMessage;

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
                result.Message = RolesMessages.RoleNotFoundMessage;

                return result;
            }

            try
            {
                var response = await _rolesRepository.Get(id);

                if (response.Success)
                {
                    var deleteResponse = await _rolesRepository.Delete((Role)response.Object);

                    if (deleteResponse.Success)
                    {
                        result.Success = deleteResponse.Success;
                        result.Message = RolesMessages.RoleDeletedMessage;

                        return result;
                    }
                    else if (!deleteResponse.Success && deleteResponse.Exception != null)
                    {
                        result.Success = deleteResponse.Success;
                        result.Message = deleteResponse.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = RolesMessages.RoleNotDeletedMessage;

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
                    result.Message = RolesMessages.RoleNotFoundMessage;

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

        public async Task<IRolesResult> GetRoles()
        {
            var result = new RolesResult();

            try
            {
                var rolesResponse = await _rolesRepository.GetAll();

                if (rolesResponse.Success)
                {
                    var roles = rolesResponse.Objects.ConvertAll(r => (IRole)r);

                    result.Success = rolesResponse.Success;
                    result.Message = RolesMessages.RolesFoundMessage;
                    result.Roles = roles;

                    return result;
                }
                else if (!rolesResponse.Success && rolesResponse.Exception != null)
                {
                    result.Success = rolesResponse.Success;
                    result.Message = rolesResponse.Exception.Message;

                    return result;
                }
                else
                {
                    result.Success = false;
                    result.Message = RolesMessages.RolesNotFoundMessage;

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

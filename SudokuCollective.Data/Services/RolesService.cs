using System;
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
using System.Collections.Generic;

namespace SudokuCollective.Data.Services
{
    public class RolesService : IRolesService
    {
        #region Fields
        private readonly IRolesRepository<Role> rolesRepository;
        #endregion

        #region Constructor
        public RolesService(IRolesRepository<Role> rolesRepo)
        {
            rolesRepository = rolesRepo;
        }
        #endregion

        #region Methods
        public async Task<IRoleResult> GetRole(
            int id, 
            bool fullRecord = true)
        {
            var result = new RoleResult();

            try
            {
                var response = await rolesRepository.GetById(id, fullRecord);

                if (response.Success)
                {
                    var role = (Role)response.Object;

                    if (fullRecord)
                    {
                        foreach (var userRole in role.Users)
                        {
                            userRole.User.Apps = new List<UserApp>();
                            userRole.User.Roles = new List<UserRole>();
                            userRole.User.Games = new List<Game>();
                        }
                    }

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

        public async Task<IRolesResult> GetRoles(
            bool fullRecord = true)
        {
            var result = new RolesResult();

            try
            {
                var rolesResponse = await rolesRepository.GetAll(fullRecord);

                if (rolesResponse.Success)
                {
                    var roles = rolesResponse.Objects.ConvertAll(r => (IRole)r);

                    if (fullRecord)
                    {
                        foreach (var role in roles)
                        {
                            foreach (var userRole in role.Users)
                            {
                                userRole.User.Apps = new List<UserApp>();
                                userRole.User.Roles = new List<UserRole>();
                                userRole.User.Games = new List<Game>();
                            }
                        }
                    }

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

        public async Task<IRoleResult> CreateRole(
            string name,
            RoleLevel roleLevel)
        {
            var result = new RoleResult();

            try
            {
                if (!(await rolesRepository.HasRoleLevel(roleLevel)))
                {

                    var role = new Role()
                    {
                        Name = name,
                        RoleLevel = roleLevel
                    };

                    var response = await rolesRepository.Add(role);

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

        public async Task<IBaseResult> UpdateRole(
            int id, 
            IUpdateRoleRequest updateRoleRO)
        {
            var result = new BaseResult();

            try
            {
                if (await rolesRepository.HasEntity(id))
                {
                    var response = await rolesRepository.GetById(id);

                    if (response.Success)
                    {
                        ((Role)response.Object).Name = updateRoleRO.Name;

                        var updateResponse = await rolesRepository
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

        public async Task<IBaseResult> DeleteRole(int id)
        {
            var result = new BaseResult();

            try
            {
                if (await rolesRepository.HasEntity(id))
                {
                    var response = await rolesRepository.GetById(id);

                    if (response.Success)
                    {
                        var deleteResponse = await rolesRepository.Delete((Role)response.Object);

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
        #endregion
    }
}

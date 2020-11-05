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
    public class RolesService : IRolesService
    {
        private readonly IRolesRepository<Role> rolesRepository;
        private readonly string roleNotFoundMessage;
        private readonly string rolesNotFoundMessage;
        private readonly string unableToAddRoleMessage;
        private readonly string roleAlreadyExistsMessage;
        private readonly string unableToUpdateRoleMessage;
        private readonly string unableToDeleteRoleMessage;

        public RolesService(IRolesRepository<Role> rolesRepo)
        {
            rolesRepository = rolesRepo;
            roleNotFoundMessage = "Role not found";
            rolesNotFoundMessage = "Roles not found";
            unableToAddRoleMessage = "Unable to add role";
            roleAlreadyExistsMessage = "Role already exists";
            unableToUpdateRoleMessage = "Unable to update role";
            unableToDeleteRoleMessage = "Unable to delete role";
        }

        public async Task<IRoleResult> GetRole(
            int id, bool fullRecord = false)
        {
            var result = new RoleResult();

            try
            {
                var roleResponse = await rolesRepository.GetById(id, fullRecord);

                if (roleResponse.Success)
                {
                    result.Success = roleResponse.Success;
                    result.Role = (Role)roleResponse.Object;

                    return result;
                }
                else if (!roleResponse.Success && roleResponse.Exception != null)
                {
                    result.Success = roleResponse.Success;
                    result.Message = roleResponse.Exception.Message;

                    return result;
                }
                else
                {
                    result.Success = false;
                    result.Message = roleNotFoundMessage;

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

        public async Task<IRolesResult> GetRoles(
            bool fullRecord = false)
        {
            var result = new RolesResult();

            try
            {
                var rolesResponse = await rolesRepository.GetAll(fullRecord);

                if (rolesResponse.Success)
                {
                    result.Success = rolesResponse.Success;
                    result.Roles = rolesResponse.Objects.ConvertAll(r => (IRole)r);

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
                    result.Message = rolesNotFoundMessage;

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

        public async Task<IRoleResult> CreateRole(string name,
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

                    var roleResponse = await rolesRepository.Create(role);

                    if (roleResponse.Success)
                    {
                        result.Success = roleResponse.Success;
                        result.Role = (IRole)roleResponse.Object;

                        return result;
                    }
                    else if (!roleResponse.Success && roleResponse.Exception != null)
                    {
                        result.Success = roleResponse.Success;
                        result.Message = roleResponse.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = unableToAddRoleMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = roleAlreadyExistsMessage;

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

        public async Task<IBaseResult> UpdateRole(int id, IUpdateRoleRequest updateRoleRO)
        {
            var result = new BaseResult();

            try
            {
                if (await rolesRepository.HasEntity(id))
                {
                    var roleResponse = await rolesRepository.GetById(id);

                    if (roleResponse.Success)
                    {
                        ((Role)roleResponse.Object).Name = updateRoleRO.Name;

                        var updateDifficultyResponse = await rolesRepository
                            .Update((Role)roleResponse.Object);

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
                            result.Message = unableToUpdateRoleMessage;

                            return result;
                        }

                    }
                    else if (!roleResponse.Success && roleResponse.Exception != null)
                    {
                        result.Success = roleResponse.Success;
                        result.Message = roleResponse.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = roleNotFoundMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = roleNotFoundMessage;

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

        public async Task<IBaseResult> DeleteRole(int id)
        {
            var result = new BaseResult();

            try
            {
                if (await rolesRepository.HasEntity(id))
                {
                    var roleResponse = await rolesRepository.GetById(id);

                    if (roleResponse.Success)
                    {
                        var deleteRoleResponse = await rolesRepository.Delete((Role)roleResponse.Object);

                        if (deleteRoleResponse.Success)
                        {
                            result.Success = deleteRoleResponse.Success;

                            return result;
                        }
                        else if (!deleteRoleResponse.Success && deleteRoleResponse.Exception != null)
                        {
                            result.Success = deleteRoleResponse.Success;
                            result.Message = deleteRoleResponse.Exception.Message;

                            return result;
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = unableToDeleteRoleMessage;

                            return result;
                        }

                    }
                    else if (!roleResponse.Success && roleResponse.Exception != null)
                    {
                        result.Success = roleResponse.Success;
                        result.Message = roleResponse.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = roleNotFoundMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = roleNotFoundMessage;

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

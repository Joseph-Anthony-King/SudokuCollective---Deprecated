using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.APIModels.PageModels;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Data.Models.ResultModels;
using SudokuCollective.Core.Models;
using SudokuCollective.Core.Interfaces.Repositories;
using SudokuCollective.Data.Helpers;

namespace SudokuCollective.Data.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository<User> usersRepository;
        private readonly IAppsRepository<App> appsRepository;
        private readonly IRolesRepository<Role> rolesRepository;
        private readonly string userNotFoundMessage;
        private readonly string usersNotFoundMessage;
        private readonly string userNameUniqueMessage;
        private readonly string emailUniqueMessage;
        private readonly string userNameRequiredMessage;
        private readonly string userNameInvalidMessage;
        private readonly string emailRequiredMessage;
        private readonly string unableToCreateUserMessage;
        private readonly string addRolesMessage;
        private readonly string unableToAddRolesMessage;
        private readonly string removeRolesMessage;
        private readonly string unableToRemoveRolesMessage;
        private readonly string invalidRolesMessage;
        private readonly string userActivatedMessage;
        private readonly string unableToActivateUserMessage;
        private readonly string userDeactivatedMessage;
        private readonly string unableToDeactivateUserMessage;
        private readonly string appNotFoundMessage;
        private readonly string pageNotFoundMessage;
        private readonly string sortValueNotImplementedMessage;

        public UsersService(
            IUsersRepository<User> usersRepo,
            IAppsRepository<App> appsRepo,
            IRolesRepository<Role> rolesApp)
        {
            usersRepository = usersRepo;
            appsRepository = appsRepo;
            rolesRepository = rolesApp;
            userNotFoundMessage = "User not found";
            usersNotFoundMessage = "Users not found";
            userNameUniqueMessage = "User name not unigue";
            emailUniqueMessage = "Email not unique";
            userNameRequiredMessage = "User name required";
            userNameInvalidMessage = "User name accepsts alphanumeric and special characters except double and single quotes";
            emailRequiredMessage = "Email required";
            unableToCreateUserMessage = "Unable to create user";
            addRolesMessage = "Successfully added roles";
            unableToAddRolesMessage = "Unable to add roles";
            removeRolesMessage = "Successfully removed roles";
            unableToRemoveRolesMessage = "Unable to remove roles";
            invalidRolesMessage = "Roles are invalid";
            userActivatedMessage = "User successfully activated";
            unableToActivateUserMessage = "Unable to activate user";
            userDeactivatedMessage = "User successfully deactivated";
            unableToDeactivateUserMessage = "Unable to deactivate user";
            appNotFoundMessage = "App not found";
            pageNotFoundMessage = "Page not found";
            sortValueNotImplementedMessage = "Sorting not implemented for this sort value";
        }

        public async Task<IUserResult> GetUser(int id, bool fullRecord = true)
        {
            var result = new UserResult();

            try
            {
                if (await usersRepository.HasEntity(id))
                {
                    var response = await usersRepository.GetById(id, fullRecord);

                    if (response.Success)
                    {
                        result.Success = response.Success;
                        result.User = (IUser)response.Object;

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
                        result.Message = userNotFoundMessage;

                        return result;
                    }
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

        public async Task<IUsersResult> GetUsers(
            IPageListModel pageListModel,
            bool fullRecord = true)
        {
            var result = new UsersResult();

            try
            {
                var response = await usersRepository.GetAll(fullRecord);

                if (response.Success)
                {
                    if (pageListModel != null)
                    {
                        if (StaticApiHelpers.IsPageValid(pageListModel, response.Objects))
                        {
                            if (pageListModel.SortBy == SortValue.NULL)
                            {
                                result.Users = response.Objects.ConvertAll(u => (IUser)u);
                            }
                            else if (pageListModel.SortBy == SortValue.ID)
                            {
                                if (!pageListModel.OrderByDescending)
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Users.Add((IUser)obj);
                                    }

                                    result.Users = result.Users
                                        .OrderBy(u => u.Id)
                                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                        .Take(pageListModel.ItemsPerPage)
                                        .ToList();
                                }
                                else
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Users.Add((IUser)obj);
                                    }

                                    result.Users = result.Users
                                        .OrderByDescending(u => u.Id)
                                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                        .Take(pageListModel.ItemsPerPage)
                                        .ToList();
                                }
                            }
                            else if (pageListModel.SortBy == SortValue.USERNAME)
                            {
                                if (!pageListModel.OrderByDescending)
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Users.Add((IUser)obj);
                                    }

                                    result.Users = result.Users
                                        .OrderBy(u => u.UserName)
                                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                        .Take(pageListModel.ItemsPerPage)
                                        .ToList();
                                }
                                else
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Users.Add((IUser)obj);
                                    }

                                    result.Users = result.Users
                                        .OrderByDescending(u => u.UserName)
                                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                        .Take(pageListModel.ItemsPerPage)
                                        .ToList();
                                }
                            }
                            else if (pageListModel.SortBy == SortValue.FIRSTNAME)
                            {
                                if (!pageListModel.OrderByDescending)
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Users.Add((IUser)obj);
                                    }

                                    result.Users = result.Users
                                        .OrderBy(u => u.FirstName)
                                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                        .Take(pageListModel.ItemsPerPage)
                                        .ToList();
                                }
                                else
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Users.Add((IUser)obj);
                                    }

                                    result.Users = result.Users
                                        .OrderByDescending(u => u.FirstName)
                                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                        .Take(pageListModel.ItemsPerPage)
                                        .ToList();
                                }
                            }
                            else if (pageListModel.SortBy == SortValue.LASTNAME)
                            {
                                if (!pageListModel.OrderByDescending)
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Users.Add((IUser)obj);
                                    }

                                    result.Users = result.Users
                                        .OrderBy(u => u.LastName)
                                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                        .Take(pageListModel.ItemsPerPage)
                                        .ToList();
                                }
                                else
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Users.Add((IUser)obj);
                                    }

                                    result.Users = result.Users
                                        .OrderByDescending(u => u.LastName)
                                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                        .Take(pageListModel.ItemsPerPage)
                                        .ToList();
                                }
                            }
                            else if (pageListModel.SortBy == SortValue.FULLNAME)
                            {
                                if (!pageListModel.OrderByDescending)
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Users.Add((IUser)obj);
                                    }

                                    result.Users = result.Users
                                        .OrderBy(u => u.FullName)
                                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                        .Take(pageListModel.ItemsPerPage)
                                        .ToList();
                                }
                                else
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Users.Add((IUser)obj);
                                    }

                                    result.Users = result.Users
                                        .OrderByDescending(u => u.FullName)
                                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                        .Take(pageListModel.ItemsPerPage)
                                        .ToList();
                                }
                            }
                            else if (pageListModel.SortBy == SortValue.NICKNAME)
                            {
                                if (!pageListModel.OrderByDescending)
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Users.Add((IUser)obj);
                                    }

                                    result.Users = result.Users
                                        .OrderBy(u => u.NickName)
                                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                        .Take(pageListModel.ItemsPerPage)
                                        .ToList();
                                }
                                else
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Users.Add((IUser)obj);
                                    }

                                    result.Users = result.Users
                                        .OrderByDescending(u => u.NickName)
                                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                        .Take(pageListModel.ItemsPerPage)
                                        .ToList();
                                }
                            }
                            else if (pageListModel.SortBy == SortValue.DATECREATED)
                            {
                                if (!pageListModel.OrderByDescending)
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Users.Add((IUser)obj);
                                    }

                                    result.Users = result.Users
                                        .OrderBy(u => u.DateCreated)
                                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                        .Take(pageListModel.ItemsPerPage)
                                        .ToList();
                                }
                                else
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Users.Add((IUser)obj);
                                    }

                                    result.Users = result.Users
                                        .OrderByDescending(u => u.DateCreated)
                                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                        .Take(pageListModel.ItemsPerPage)
                                        .ToList();
                                }
                            }
                            else if (pageListModel.SortBy == SortValue.DATEUPDATED)
                            {
                                if (!pageListModel.OrderByDescending)
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Users.Add((IUser)obj);
                                    }

                                    result.Users = result.Users
                                        .OrderBy(u => u.DateUpdated)
                                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                        .Take(pageListModel.ItemsPerPage)
                                        .ToList();
                                }
                                else
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Users.Add((IUser)obj);
                                    }

                                    result.Users = result.Users
                                        .OrderByDescending(u => u.DateUpdated)
                                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                        .Take(pageListModel.ItemsPerPage)
                                        .ToList();
                                }
                            }
                            else
                            {
                                result.Success = false;
                                result.Message = sortValueNotImplementedMessage;

                                return result;
                            }
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = pageNotFoundMessage;

                            return result;
                        }
                    }
                    else
                    {
                        result.Users = response.Objects.ConvertAll(u => (IUser)u);
                    }

                    result.Success = response.Success;

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
                    result.Message = usersNotFoundMessage;

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

        public async Task<IUserResult> CreateUser(
            IRegisterRequest registerRequest)
        {
            var result = new UserResult();

            var isUserNameUnique = false;
            var isEmailUnique = false;

            // User name accepsts alphanumeric and special characters except double and single quotes
            var regex = new Regex("^[^-]{1}?[^\"\']*$");

            if (!string.IsNullOrEmpty(registerRequest.UserName))
            {
                isUserNameUnique = await usersRepository.IsUserNameUnique(registerRequest.UserName);
            }

            if (!string.IsNullOrEmpty(registerRequest.Email))
            {
                isEmailUnique = await usersRepository.IsEmailUnique(registerRequest.Email);
            }

            if (string.IsNullOrEmpty(registerRequest.UserName)
                || string.IsNullOrEmpty(registerRequest.Email)
                || !isUserNameUnique
                || !isEmailUnique
                || !regex.IsMatch(registerRequest.UserName))
            {
                if (string.IsNullOrEmpty(registerRequest.UserName))
                {
                    result.Success = false;
                    result.Message = userNameRequiredMessage;

                    return result;
                }
                else if (string.IsNullOrEmpty(registerRequest.Email))
                {
                    result.Success = false;
                    result.Message = emailRequiredMessage;

                    return result;
                }
                else if (!regex.IsMatch(registerRequest.UserName))
                {
                    result.Success = false;
                    result.Message = userNameInvalidMessage;

                    return result;
                }
                else if (!isUserNameUnique)
                {
                    result.Success = isUserNameUnique;
                    result.Message = userNameUniqueMessage;

                    return result;
                }
                else
                {
                    result.Success = isEmailUnique;
                    result.Message = emailUniqueMessage;

                    return result;
                }
            }
            else
            {
                try
                {
                    if (await appsRepository.IsAppLicenseValid(registerRequest.License))
                    {
                        var appResponse = await appsRepository.GetByLicense(registerRequest.License);

                        if (appResponse.Success)
                        {
                            var salt = BCrypt.Net.BCrypt.GenerateSalt();

                            var user = new User(
                                0,
                                registerRequest.UserName,
                                registerRequest.FirstName,
                                registerRequest.LastName,
                                registerRequest.NickName,
                                registerRequest.Email,
                                BCrypt.Net.BCrypt.HashPassword(registerRequest.Password, salt),
                                true,
                                DateTime.UtcNow,
                                DateTime.MinValue);

                            user.Apps.Add(
                                new UserApp() { 
                                    User = user, 
                                    App = (App)appResponse.Object, 
                                    AppId = ((App)appResponse.Object).Id });

                            var userResponse = await usersRepository.Create(user);

                            if (userResponse.Success)
                            {
                                result.Success = userResponse.Success;
                                result.User = (User)userResponse.Object;

                                return result;
                            }
                            else if (!userResponse.Success && userResponse.Exception != null)
                            {
                                result.Success = userResponse.Success;
                                result.Message = userResponse.Exception.Message;

                                return result;
                            }
                            else
                            {
                                result.Success = userResponse.Success;
                                result.Message = unableToCreateUserMessage;

                                return result;
                            }
                        }
                        else if (!appResponse.Success && appResponse.Exception != null)
                        {
                            result.Success = appResponse.Success;
                            result.Message = appResponse.Exception.Message;

                            return result;
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = appNotFoundMessage;

                            return result;
                        }
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = appNotFoundMessage;

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

        public async Task<IUserResult> UpdateUser(
            int id, IUpdateUserRequest updateUserRequest)
        {
            var result = new UserResult();

            // User name accepsts alphanumeric and special characters except double and single quotes
            var regex = new Regex("^[^-]{1}?[^\"\']*$");

            var isUserNameUnique = await usersRepository.IsUserNameUnique(updateUserRequest.UserName);
            var isEmailUnique = await usersRepository.IsEmailUnique(updateUserRequest.Email);

            if (!isUserNameUnique
                || !isEmailUnique
                || !regex.IsMatch(updateUserRequest.UserName)
                || string.IsNullOrEmpty(updateUserRequest.UserName)
                || string.IsNullOrEmpty(updateUserRequest.Email))
            {
                if (!isUserNameUnique)
                {
                    result.Success = isUserNameUnique;
                    result.Message = userNameUniqueMessage;

                    return result;
                }
                else if (!isEmailUnique)
                {
                    result.Success = isEmailUnique;
                    result.Message = emailUniqueMessage;

                    return result;
                }
                else if (!regex.IsMatch(updateUserRequest.UserName))
                {
                    result.Success = false;
                    result.Message = userNameInvalidMessage;

                    return result;
                }
                else if (string.IsNullOrEmpty(updateUserRequest.UserName))
                {
                    result.Success = false;
                    result.Message = userNameRequiredMessage;

                    return result;
                }
                else
                {
                    result.Success = false;
                    result.Message = emailRequiredMessage;

                    return result;
                }
            }
            else
            {
                try
                {
                    if (await usersRepository.HasEntity(id))
                    {
                        var userResponse = await usersRepository.GetById(id, true);

                        if (userResponse.Success)
                        {
                            ((User)userResponse.Object).UserName = updateUserRequest.UserName;
                            ((User)userResponse.Object).FirstName = updateUserRequest.FirstName;
                            ((User)userResponse.Object).LastName = updateUserRequest.LastName;
                            ((User)userResponse.Object).NickName = updateUserRequest.NickName;
                            ((User)userResponse.Object).Email = updateUserRequest.Email;
                            ((User)userResponse.Object).DateUpdated = DateTime.UtcNow;

                            var updateUserResponse = await usersRepository.Update((User)userResponse.Object);

                            if (updateUserResponse.Success)
                            {
                                result.Success = userResponse.Success;
                                result.User = (User)userResponse.Object;

                                return result;
                            }
                            else if (!updateUserResponse.Success && updateUserResponse.Exception != null)
                            {
                                result.Success = userResponse.Success;
                                result.Message = userResponse.Exception.Message;

                                return result;
                            }
                            else
                            {
                                result.Success = false;
                                result.Message = "Unable to update user";

                                return result;
                            }
                        }
                        else if (!userResponse.Success && userResponse.Exception != null)
                        {
                            result.Success = userResponse.Success;
                            result.Message = userResponse.Exception.Message;

                            return result;
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = userNotFoundMessage;

                            return result;
                        }
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
        }

        public async Task<IBaseResult> UpdatePassword(int id, IUpdatePasswordRequest updatePasswordRO)
        {
            var result = new BaseResult();
            var salt = BCrypt.Net.BCrypt.GenerateSalt();

            try
            {
                if (await usersRepository.HasEntity(id))
                {
                    var userResponse = await usersRepository.GetById(id, true);

                    if (userResponse.Success)
                    {
                        if (BCrypt.Net.BCrypt.Verify(updatePasswordRO.OldPassword, ((IUser)userResponse.Object).Password))
                        {
                            ((User)userResponse.Object).Password = BCrypt.Net.BCrypt
                                    .HashPassword(updatePasswordRO.NewPassword, salt);


                            ((User)userResponse.Object).DateUpdated = DateTime.UtcNow;

                            var updateUserResponse = await usersRepository.Update((User)userResponse.Object);

                            if (updateUserResponse.Success)
                            {
                                result.Success = userResponse.Success;

                                return result;
                            }
                            else if (!updateUserResponse.Success && updateUserResponse.Exception != null)
                            {
                                result.Success = userResponse.Success;
                                result.Message = userResponse.Exception.Message;

                                return result;
                            }
                            else
                            {
                                result.Success = false;
                                result.Message = "Unable to update password";

                                return result;
                            }
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = "Old password was incorrect";

                            return result;
                        }
                    }
                    else if (!userResponse.Success && userResponse.Exception != null)
                    {
                        result.Success = false;
                        result.Message = userNotFoundMessage;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = userNotFoundMessage;

                        return result;
                    }
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

        public async Task<IBaseResult> DeleteUser(int id)
        {
            var result = new BaseResult();

            try
            {
                if (await usersRepository.HasEntity(id))
                {
                    var userResponse = await usersRepository.GetById(id);

                    if (userResponse.Success)
                    {
                        var deletionResponse = await usersRepository.Delete((User)userResponse.Object);

                        if (deletionResponse.Success)
                        {
                            result.Success = true;

                            return result;
                        }
                        else if (!deletionResponse.Success && deletionResponse.Exception != null)
                        {
                            result.Success = deletionResponse.Success;
                            result.Message = deletionResponse.Exception.Message;

                            return result;
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = "Unable to delete user";

                            return result;
                        }
                    }
                    else if (!userResponse.Success && userResponse.Exception != null)
                    {
                        result.Success = userResponse.Success;
                        result.Message = userResponse.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = userNotFoundMessage;

                        return result;
                    }
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

        public async Task<IBaseResult> AddUserRoles(int userid, List<int> roleIds)
        {
            var result = new BaseResult();

            try
            {
                if (await usersRepository.HasEntity(userid))
                {
                    if (await rolesRepository.IsListValid(roleIds))
                    {
                        var addRolesResponse = await usersRepository.AddRoles(userid, roleIds);

                        if (addRolesResponse.Success)
                        {
                            result.Success = addRolesResponse.Success;
                            result.Message = addRolesMessage;

                            return result;
                        }
                        else if (!addRolesResponse.Success && addRolesResponse.Exception != null)
                        {
                            result.Success = addRolesResponse.Success;
                            result.Message = addRolesResponse.Exception.Message;

                            return result;
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = unableToAddRolesMessage;

                            return result;
                        }
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = invalidRolesMessage;

                        return result;
                    }
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

        public async Task<IBaseResult> RemoveUserRoles(int userid, List<int> roleIds)
        {
            var result = new BaseResult();

            try
            {
                if (await usersRepository.HasEntity(userid))
                {
                    if (await rolesRepository.IsListValid(roleIds))
                    {
                        var addRolesResponse = await usersRepository.RemoveRoles(userid, roleIds);

                        if (addRolesResponse.Success)
                        {
                            result.Success = addRolesResponse.Success;
                            result.Message = removeRolesMessage;

                            return result;
                        }
                        else if (!addRolesResponse.Success && addRolesResponse.Exception != null)
                        {
                            result.Success = addRolesResponse.Success;
                            result.Message = addRolesResponse.Exception.Message;

                            return result;
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = unableToRemoveRolesMessage;

                            return result;
                        }
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = invalidRolesMessage;

                        return result;
                    }
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

        public async Task<IBaseResult> ActivateUser(int id)
        {
            var result = new BaseResult();

            try
            {
                if (await usersRepository.HasEntity(id))
                {
                    if (await usersRepository.ActivateUser(id))
                    {
                        result.Success = true;
                        result.Message = userActivatedMessage;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = unableToActivateUserMessage;

                        return result;
                    }
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

        public async Task<IBaseResult> DeactivateUser(int id)
        {
            var result = new BaseResult();

            try
            {
                if (await usersRepository.HasEntity(id))
                {
                    if (await usersRepository.DeactivateUser(id))
                    {
                        result.Success = true;
                        result.Message = userDeactivatedMessage;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = unableToDeactivateUserMessage;

                        return result;
                    }
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
    }
}

using System;
using System.IO;
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
using SudokuCollective.Core.Models;
using SudokuCollective.Core.Interfaces.Repositories;
using SudokuCollective.Data.Helpers;
using SudokuCollective.Data.Messages;
using SudokuCollective.Data.Models.ResultModels;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels.UserResults;

namespace SudokuCollective.Data.Services
{
    public class UsersService : IUsersService
    {
        #region Fields
        private readonly IUsersRepository<User> usersRepository;
        private readonly IAppsRepository<App> appsRepository;
        private readonly IRolesRepository<Role> rolesRepository;
        private readonly IEmailConfirmationsRepository<EmailConfirmation> emailConfirmationsRepository;
        private readonly IEmailService emailService;
        #endregion

        #region Constructor
        public UsersService(
            IUsersRepository<User> usersRepo,
            IAppsRepository<App> appsRepo,
            IRolesRepository<Role> rolesRepo,
            IEmailConfirmationsRepository<EmailConfirmation> emailConfirmationsRepo,
            IEmailService emailServ)
        {
            usersRepository = usersRepo;
            appsRepository = appsRepo;
            rolesRepository = rolesRepo;
            emailConfirmationsRepository = emailConfirmationsRepo;
            emailService = emailServ;
        }
        #endregion

        #region Methods
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
                        var user = (User)response.Object;

                        if (fullRecord)
                        {
                            foreach (var userApp in user.Apps)
                            {
                                userApp.App.Users = new List<UserApp>();
                            }

                            foreach (var userRole in user.Roles)
                            {
                                userRole.Role.Users = new List<UserRole>();
                            }

                            foreach (var game in user.Games)
                            {
                                game.User = null;
                                game.SudokuMatrix.Difficulty.Matrices = new List<SudokuMatrix>();
                            }
                        }

                        result.Success = response.Success;
                        result.Message = UsersMessages.UserFoundMessage;
                        result.User = user;

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
                        result.Message = UsersMessages.UserNotFoundMessage;

                        return result;
                    }
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
                        if (StaticDataHelpers.IsPageValid(pageListModel, response.Objects))
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
                        result.Users = response.Objects.ConvertAll(u => (IUser)u);
                    }

                    if (fullRecord)
                    {
                        foreach (var user in result.Users)
                        {
                            foreach (var userApp in user.Apps)
                            {
                                userApp.App.Users = new List<UserApp>();
                            }

                            foreach (var userRole in user.Roles)
                            {
                                userRole.Role.Users = new List<UserRole>();
                            }

                            foreach (var game in user.Games)
                            {
                                game.User = null;
                                game.SudokuMatrix.Difficulty.Matrices = new List<SudokuMatrix>();
                            }
                        }
                    }

                    result.Success = response.Success;
                    result.Message = UsersMessages.UsersFoundMessage;

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
                    result.Message = UsersMessages.UsersNotFoundMessage;

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

        public async Task<IUserResult> CreateUser(
            IRegisterRequest request,
            string baseUrl,
            string emailtTemplatePath)
        {
            var result = new UserResult();

            var isUserNameUnique = false;
            var isEmailUnique = false;

            // User name accepsts alphanumeric and special characters except double and single quotes
            var regex = new Regex("^[^-]{1}?[^\"\']*$");

            if (!string.IsNullOrEmpty(request.UserName))
            {
                isUserNameUnique = await usersRepository.IsUserNameUnique(request.UserName);
            }

            if (!string.IsNullOrEmpty(request.Email))
            {
                isEmailUnique = await usersRepository.IsEmailUnique(request.Email);
            }

            if (string.IsNullOrEmpty(request.UserName)
                || string.IsNullOrEmpty(request.Email)
                || !isUserNameUnique
                || !isEmailUnique
                || !regex.IsMatch(request.UserName))
            {
                if (string.IsNullOrEmpty(request.UserName))
                {
                    result.Success = false;
                    result.Message = UsersMessages.UserNameRequiredMessage;

                    return result;
                }
                else if (string.IsNullOrEmpty(request.Email))
                {
                    result.Success = false;
                    result.Message = UsersMessages.EmailRequiredMessage;

                    return result;
                }
                else if (!regex.IsMatch(request.UserName))
                {
                    result.Success = false;
                    result.Message = UsersMessages.UserNameInvalidMessage;

                    return result;
                }
                else if (!isUserNameUnique)
                {
                    result.Success = isUserNameUnique;
                    result.Message = UsersMessages.UserNameUniqueMessage;

                    return result;
                }
                else
                {
                    result.Success = isEmailUnique;
                    result.Message = UsersMessages.EmailUniqueMessage;

                    return result;
                }
            }
            else
            {
                try
                {
                    if (await appsRepository.IsAppLicenseValid(request.License))
                    {
                        var appResponse = await appsRepository.GetByLicense(request.License);

                        if (appResponse.Success)
                        {
                            var salt = BCrypt.Net.BCrypt.GenerateSalt();

                            var user = new User(
                                0,
                                request.UserName,
                                request.FirstName,
                                request.LastName,
                                request.NickName,
                                request.Email,
                                BCrypt.Net.BCrypt.HashPassword(request.Password, salt),
                                true,
                                false,
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
                                user = (User)userResponse.Object;

                                var emailConfirmation = new EmailConfirmation(
                                    user.Id,
                                    user.Apps.FirstOrDefault().AppId);

                                var emailConfirmationResponse = await emailConfirmationsRepository.Create(emailConfirmation);

                                var html = File.ReadAllText(emailtTemplatePath);
                                var emailConfirmationUrl = string.Format("https://{0}/confirmEmail/{1}", 
                                    baseUrl, 
                                    ((EmailConfirmation)emailConfirmationResponse.Object).Token);
                                var appTitle = string.Empty;
                                var url = string.Empty;

                                if (user.Apps.FirstOrDefault().AppId == 1)
                                {
                                    appTitle = "SudokuCollective.com";
                                }
                                else
                                {
                                    appTitle = user.Apps.FirstOrDefault().App.Name;
                                }

                                if (user.Apps.FirstOrDefault().App.InProduction)
                                {
                                    url = user.Apps.FirstOrDefault().App.LiveUrl;
                                }
                                else
                                {
                                    url = user.Apps.FirstOrDefault().App.DevUrl;
                                }

                                html = html.Replace("{{USER_NAME}}", user.UserName);
                                html = html.Replace("{{CONFIRM_EMAIL_URL}}", emailConfirmationUrl);
                                html = html.Replace("{{APP_TITLE}}", appTitle);
                                html = html.Replace("{{URL}}", url);

                                result.ConfirmationEmailSuccessfullySent = emailService
                                    .Send(user.Email, "Please confirm email address", html);

                                foreach (var userRole in user.Roles)
                                {
                                    userRole.Role.Users = new List<UserRole>();
                                }

                                foreach (var userApp in user.Apps)
                                {
                                    userApp.App.Users = new List<UserApp>();
                                }

                                result.Success = userResponse.Success;
                                result.Message = UsersMessages.UserCreatedMessage;
                                result.User = user;

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
                                result.Message = UsersMessages.UserNotCreatedMessage;

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
                            result.Message = AppsMessages.AppNotFoundMessage;

                            return result;
                        }
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = AppsMessages.AppNotFoundMessage;

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
        }

        public async Task<IUserResult> UpdateUser(
            int id, 
            IUpdateUserRequest request,
            string baseUrl,
            string emailtTemplatePath)
        {
            var result = new UserResult();

            // User name accepsts alphanumeric and special characters except double and single quotes
            var regex = new Regex("^[^-]{1}?[^\"\']*$");

            var isUserNameUnique = await usersRepository.IsUpdatedUserNameUnique(id, request.UserName);
            var isEmailUnique = await usersRepository.IsUpdatedEmailUnique(id, request.Email);

            if (string.IsNullOrEmpty(request.UserName)
                || string.IsNullOrEmpty(request.Email)
                || !isUserNameUnique
                || !isEmailUnique
                || !regex.IsMatch(request.UserName))
            {
                if (string.IsNullOrEmpty(request.UserName))
                {
                    result.Success = false;
                    result.Message = UsersMessages.UserNameRequiredMessage;

                    return result;
                }
                else if (string.IsNullOrEmpty(request.Email))
                {
                    result.Success = false;
                    result.Message = UsersMessages.EmailRequiredMessage;

                    return result;
                }
                else if (!regex.IsMatch(request.UserName))
                {
                    result.Success = false;
                    result.Message = UsersMessages.UserNameInvalidMessage;

                    return result;
                }
                else if (!isUserNameUnique)
                {
                    result.Success = isUserNameUnique;
                    result.Message = UsersMessages.UserNameUniqueMessage;

                    return result;
                }
                else
                {
                    result.Success = isEmailUnique;
                    result.Message = UsersMessages.EmailUniqueMessage;

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
                            var user = (User)userResponse.Object;

                            user.UserName = request.UserName;
                            user.FirstName = request.FirstName;
                            user.LastName = request.LastName;
                            user.NickName = request.NickName;
                            user.DateUpdated = DateTime.UtcNow;

                            if (!user.Email.ToLower().Equals(request.Email.ToLower()))
                            {
                                user.Email = request.Email;

                                var emailConfirmation = new EmailConfirmation(
                                    user.Id, 
                                    request.AppId, 
                                    user.Email, 
                                    request.Email);

                                var emailConfirmationResponse = await emailConfirmationsRepository
                                    .Create(emailConfirmation);

                                var html = File.ReadAllText(emailtTemplatePath);
                                var emailConfirmationUrl = string.Format("https://{0}/confirmEmail/{1}",
                                    baseUrl,
                                    ((EmailConfirmation)emailConfirmationResponse.Object).Token);
                                var appTitle = string.Empty;
                                var url = string.Empty;

                                if (request.AppId == 1)
                                {
                                    appTitle = "SudokuCollective.com";
                                }
                                else
                                {
                                    appTitle = user
                                        .Apps
                                        .Where(ua => ua.AppId == request.AppId)
                                        .Select(ua => ua.App.Name)
                                        .FirstOrDefault();
                                }

                                if (user
                                    .Apps
                                    .Where(ua => ua.AppId == request.AppId)
                                    .Select(ua => ua.App.InProduction)
                                    .FirstOrDefault())
                                {
                                    url = user
                                        .Apps
                                        .Where(ua => ua.AppId == request.AppId)
                                        .Select(ua => ua.App.LiveUrl)
                                        .FirstOrDefault();
                                }
                                else
                                {
                                    url = user
                                        .Apps
                                        .Where(ua => ua.AppId == request.AppId)
                                        .Select(ua => ua.App.DevUrl)
                                        .FirstOrDefault();
                                }

                                html = html.Replace("{{USER_NAME}}", user.UserName);
                                html = html.Replace("{{CONFIRM_EMAIL_URL}}", emailConfirmationUrl);
                                html = html.Replace("{{APP_TITLE}}", appTitle);
                                html = html.Replace("{{URL}}", url);

                                result.ConfirmationEmailSuccessfullySent = emailService
                                    .Send(user.Email, "Please confirm email address", html);
                            }

                            var updateUserResponse = await usersRepository.Update(user);

                            if (updateUserResponse.Success)
                            {
                                user = (User)updateUserResponse.Object;

                                result.Success = userResponse.Success;
                                result.Message = UsersMessages.UserUpdatedMessage;
                                result.User = user;

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
                                result.Message = UsersMessages.UserNotUpdatedMessage;

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
                            result.Message = UsersMessages.UserNotFoundMessage;

                            return result;
                        }
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
        }

        public async Task<IBaseResult> UpdatePassword(int id, IUpdatePasswordRequest request)
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
                        if (BCrypt.Net.BCrypt.Verify(request.OldPassword, ((IUser)userResponse.Object).Password))
                        {
                            ((User)userResponse.Object).Password = BCrypt.Net.BCrypt
                                    .HashPassword(request.NewPassword, salt);


                            ((User)userResponse.Object).DateUpdated = DateTime.UtcNow;

                            var updateUserResponse = await usersRepository.Update((User)userResponse.Object);

                            if (updateUserResponse.Success)
                            {
                                result.Success = userResponse.Success;
                                result.Message = UsersMessages.PasswordUpdatedMessage;

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
                                result.Message = UsersMessages.PasswordNotUpdatedMessage;

                                return result;
                            }
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = UsersMessages.OldPasswordIncorrectMessage;

                            return result;
                        }
                    }
                    else if (!userResponse.Success && userResponse.Exception != null)
                    {
                        result.Success = false;
                        result.Message = UsersMessages.UserNotFoundMessage;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = UsersMessages.UsersFoundMessage;

                        return result;
                    }
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
                            result.Message = UsersMessages.UserDeletedMessage;

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
                            result.Message = UsersMessages.UserNotDeletedMessage;

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
                        result.Message = UsersMessages.UserNotFoundMessage;

                        return result;
                    }
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

        public async Task<IBaseResult> AddUserRoles(int userid, List<int> roleIds)
        {
            var result = new BaseResult();

            try
            {
                if (await usersRepository.HasEntity(userid))
                {
                    if (await rolesRepository.IsListValid(roleIds))
                    {
                        var response = await usersRepository.AddRoles(userid, roleIds);

                        if (response.Success)
                        {
                            result.Success = response.Success;
                            result.Message = UsersMessages.RolesAddedMessage;

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
                            result.Message = UsersMessages.RolesNotAddedMessage;

                            return result;
                        }
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = UsersMessages.RoleInvalidMessage;

                        return result;
                    }
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

        public async Task<IBaseResult> RemoveUserRoles(int userid, List<int> roleIds)
        {
            var result = new BaseResult();

            try
            {
                if (await usersRepository.HasEntity(userid))
                {
                    if (await rolesRepository.IsListValid(roleIds))
                    {
                        var response = await usersRepository.RemoveRoles(userid, roleIds);

                        if (response.Success)
                        {
                            result.Success = response.Success;
                            result.Message = UsersMessages.RolesRemovedMessage;

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
                            result.Message = UsersMessages.RolesNotRemovedMessage;

                            return result;
                        }
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = UsersMessages.RoleInvalidMessage;

                        return result;
                    }
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
                        result.Message = UsersMessages.UserActivatedMessage;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = UsersMessages.UserNotActivatedMessage;

                        return result;
                    }
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
                        result.Message = UsersMessages.UserDeactivatedMessage;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = UsersMessages.UserNotDeactivatedMessage;

                        return result;
                    }
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

        public async Task<IConfirmEmailResult> ConfirmEmail(
            string token,
            string baseUrl,
            string emailtTemplatePath)
        {
            var result = new ConfirmEmailResult();

            try
            {
                var emailConfirmationResponse = await emailConfirmationsRepository.GetByToken(token);

                if (emailConfirmationResponse.Success)
                {
                    var emailConfirmation = (EmailConfirmation)emailConfirmationResponse.Object;

                    if (!emailConfirmation.IsUpdate)
                    {
                        var response = await usersRepository.ConfirmEmail(emailConfirmation);

                        if (response.Success)
                        {
                            var removeEmailConfirmationResponse = await emailConfirmationsRepository.Delete(emailConfirmation);

                            var user = (User)response.Object;

                            result.Success = response.Success;
                            result.FirstName = user.FirstName;

                            if (emailConfirmation.AppId == 1)
                            {
                                result.AppTitle = "SudokuCollective.com";
                            }
                            else
                            {
                                result.AppTitle = user
                                    .Apps
                                    .Where(ua => ua.AppId == emailConfirmation.AppId)
                                    .Select(ua => ua.App.Name)
                                    .FirstOrDefault();
                            }

                            if (user
                                .Apps
                                .Where(ua => ua.AppId == emailConfirmation.AppId)
                                .Select(ua => ua.App.InProduction)
                                .FirstOrDefault())
                            {
                                result.Url = user
                                    .Apps
                                    .Where(ua => ua.AppId == emailConfirmation.AppId)
                                    .Select(ua => ua.App.LiveUrl)
                                    .FirstOrDefault();
                            }
                            else
                            {
                                result.Url = user
                                    .Apps
                                    .Where(ua => ua.AppId == emailConfirmation.AppId)
                                    .Select(ua => ua.App.DevUrl)
                                    .FirstOrDefault();
                            }

                            result.Message = UsersMessages.EmailConfirmedMessage;

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
                            result.Message = UsersMessages.EmailNotConfirmedMessage;

                            return result;
                        }
                    }
                    else if (emailConfirmation.IsUpdate && !emailConfirmation.OldEmailAddressConfirmed)
                    {
                        var response = await usersRepository.UpdateUserEmail(emailConfirmation);
                        var user = (User)response.Object;
                        var app = (App)(await appsRepository.GetById(emailConfirmation.AppId)).Object;

                        if (response.Success)
                        {
                            var html = File.ReadAllText(emailtTemplatePath);
                            var emailConfirmationUrl = string.Format("https://{0}/confirmEmail/{1}",
                                baseUrl,
                                emailConfirmation.Token);
                            var appTitle = string.Empty;
                            var url = string.Empty;

                            if (emailConfirmation.AppId == 1)
                            {
                                appTitle = "SudokuCollective.com";
                            }
                            else
                            {
                                appTitle = app.Name;
                            }

                            if (app.InProduction)
                            {
                                url = app.LiveUrl;
                            }
                            else
                            {
                                url = app.DevUrl;
                            }

                            html = html.Replace("{{USER_NAME}}", user.UserName);
                            html = html.Replace("{{CONFIRM_EMAIL_URL}}", emailConfirmationUrl);
                            html = html.Replace("{{APP_TITLE}}", appTitle);
                            html = html.Replace("{{URL}}", url);

                            result.ConfirmationEmailSuccessfullySent = emailService
                                .Send(user.Email, "Please confirm email address", html);

                            emailConfirmation.OldEmailAddressConfirmed = true;

                            emailConfirmation = (EmailConfirmation)(await emailConfirmationsRepository.Update(emailConfirmation)).Object;

                            result.Success = response.Success;
                            result.FirstName = user.FirstName;
                            result.IsUpdate = emailConfirmation.IsUpdate;
                            result.AppTitle = appTitle;
                            result.Url = url;
                            result.Message = UsersMessages.OldEmailConfirmedMessage;

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
                            result.Message = UsersMessages.OldEmailNotConfirmedMessage;

                            return result;
                        }
                    }
                    else
                    {
                        var response = await usersRepository.ConfirmEmail(emailConfirmation);

                        if (response.Success)
                        {
                            var removeEmailConfirmationResponse = await emailConfirmationsRepository.Delete(emailConfirmation);

                            var user = (User)response.Object;

                            result.Success = response.Success;
                            result.FirstName = user.FirstName;
                            result.IsUpdate = emailConfirmation.IsUpdate;
                            result.NewEmailAddressConfirmed = true;

                            if (emailConfirmation.AppId == 1)
                            {
                                result.AppTitle = "SudokuCollective.com";
                            }
                            else
                            {
                                result.AppTitle = user
                                    .Apps
                                    .Where(ua => ua.AppId == emailConfirmation.AppId)
                                    .Select(ua => ua.App.Name)
                                    .FirstOrDefault();
                            }

                            if (user
                                .Apps
                                .Where(ua => ua.AppId == emailConfirmation.AppId)
                                .Select(ua => ua.App.InProduction)
                                .FirstOrDefault())
                            {
                                result.Url = user
                                    .Apps
                                    .Where(ua => ua.AppId == emailConfirmation.AppId)
                                    .Select(ua => ua.App.LiveUrl)
                                    .FirstOrDefault();
                            }
                            else
                            {
                                result.Url = user
                                    .Apps
                                    .Where(ua => ua.AppId == emailConfirmation.AppId)
                                    .Select(ua => ua.App.DevUrl)
                                    .FirstOrDefault();
                            }

                            result.Message = UsersMessages.EmailConfirmedMessage;

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
                            result.Message = UsersMessages.EmailNotConfirmedMessage;

                            return result;
                        }
                    }
                }
                else if (!emailConfirmationResponse.Success && emailConfirmationResponse.Exception != null)
                {
                    result.Success = emailConfirmationResponse.Success;
                    result.Message = emailConfirmationResponse.Exception.Message;

                    return result;
                }
                else
                {
                    result.Success = false;
                    result.Message = UsersMessages.EmailNotConfirmedMessage;

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

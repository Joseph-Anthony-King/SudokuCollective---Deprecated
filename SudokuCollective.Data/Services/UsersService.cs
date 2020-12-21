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
        private readonly IPasswordUpdatesRepository<PasswordUpdate> passwordUpdatesRepository;
        private readonly IEmailService emailService;
        #endregion

        #region Constructor
        public UsersService(
            IUsersRepository<User> usersRepo,
            IAppsRepository<App> appsRepo,
            IRolesRepository<Role> rolesRepo,
            IEmailConfirmationsRepository<EmailConfirmation> emailConfirmationsRepo,
            IPasswordUpdatesRepository<PasswordUpdate> passwordUpdatesRepo,
            IEmailService emailServ)
        {
            usersRepository = usersRepo;
            appsRepository = appsRepo;
            rolesRepository = rolesRepo;
            emailConfirmationsRepository = emailConfirmationsRepo;
            passwordUpdatesRepository = passwordUpdatesRepo;
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
                                false,
                                false,
                                BCrypt.Net.BCrypt.HashPassword(request.Password, salt),
                                false,
                                true,
                                DateTime.UtcNow,
                                DateTime.MinValue);

                            var app = (App)appResponse.Object;

                            user.Apps.Add(
                                new UserApp() { 
                                    User = user, 
                                    App = app,
                                    AppId = app.Id });

                            var userResponse = await usersRepository.Create(user);

                            if (userResponse.Success)
                            {
                                user = (User)userResponse.Object;

                                var emailConfirmation = new EmailConfirmation(
                                    user.Id,
                                    app.Id);

                                emailConfirmation = await EnsureEmailConfirmationTokenIsUnique(emailConfirmation);

                                emailConfirmation = (EmailConfirmation)(await emailConfirmationsRepository.Create(emailConfirmation))
                                    .Object;

                                string emailConfirmationUrl;

                                if (app.UseCustomEmailConfirmationUrl)
                                {
                                    if (app.InDevelopment)
                                    {
                                        emailConfirmationUrl = string.Format("{0}{1}",
                                            app.CustomEmailConfirmationDevUrl,
                                            emailConfirmation.Token);
                                    }
                                    else
                                    {
                                        emailConfirmationUrl = string.Format("{0}{1}",
                                            app.CustomPasswordUpdateLiveUrl,
                                            emailConfirmation.Token);
                                    }
                                }
                                else
                                {
                                    emailConfirmationUrl = string.Format("https://{0}/confirmEmail/{1}",
                                        baseUrl,
                                        emailConfirmation.Token);
                                }

                                var html = File.ReadAllText(emailtTemplatePath);
                                var appTitle = app.Name;
                                var url = string.Empty;

                                if (app.InDevelopment)
                                {
                                    url = app.DevUrl;
                                }
                                else
                                {
                                    url = app.LiveUrl;
                                }

                                html = html.Replace("{{USER_NAME}}", user.UserName);
                                html = html.Replace("{{CONFIRM_EMAIL_URL}}", emailConfirmationUrl);
                                html = html.Replace("{{APP_TITLE}}", appTitle);
                                html = html.Replace("{{URL}}", url);

                                var emailSubject = string.Format("Greetings from {0}: Please Confirm Email", appTitle);

                                result.ConfirmationEmailSuccessfullySent = emailService
                                    .Send(user.Email, emailSubject, html);

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
                            var app = (App)(await appsRepository.GetById(request.AppId)).Object;

                            user.UserName = request.UserName;
                            user.FirstName = request.FirstName;
                            user.LastName = request.LastName;
                            user.NickName = request.NickName;
                            user.DateUpdated = DateTime.UtcNow;

                            if (!user.Email.ToLower().Equals(request.Email.ToLower()))
                            {
                                user.ReceivedRequestToUpdateEmail = true;

                                var emailConfirmation = new EmailConfirmation(
                                    user.Id, 
                                    request.AppId, 
                                    user.Email, 
                                    request.Email);

                                emailConfirmation = await EnsureEmailConfirmationTokenIsUnique(emailConfirmation);

                                var emailConfirmationResponse = await emailConfirmationsRepository
                                    .Create(emailConfirmation);

                                string emailConfirmationUrl;

                                if (app.UseCustomEmailConfirmationUrl)
                                {
                                    if (app.InDevelopment)
                                    {
                                        emailConfirmationUrl = string.Format("{0}{1}", 
                                            app.CustomEmailConfirmationDevUrl, 
                                            emailConfirmation.Token);
                                    }
                                    else
                                    {
                                        emailConfirmationUrl = string.Format("{0}{1}",
                                            app.CustomPasswordUpdateLiveUrl,
                                            emailConfirmation.Token);
                                    }
                                }
                                else
                                {
                                    emailConfirmationUrl = string.Format("https://{0}/confirmEmail/{1}",
                                        baseUrl,
                                        ((EmailConfirmation)emailConfirmationResponse.Object).Token);
                                }

                                var html = File.ReadAllText(emailtTemplatePath);
                                var appTitle = app.Name;
                                var url = string.Empty;

                                if (app.InDevelopment)
                                {
                                    url = app.DevUrl;
                                }
                                else
                                {
                                    url = app.LiveUrl;
                                }

                                html = html.Replace("{{USER_NAME}}", user.UserName);
                                html = html.Replace("{{CONFIRM_EMAIL_URL}}", emailConfirmationUrl);
                                html = html.Replace("{{APP_TITLE}}", appTitle);
                                html = html.Replace("{{URL}}", url);

                                var emailSubject = string.Format("Greetings from {0}: Please Confirm Old Email", appTitle);

                                result.ConfirmationEmailSuccessfullySent = emailService
                                    .Send(user.Email, emailSubject, html);
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

        public async Task<IBaseResult> RequestPasswordUpdate(
            IRequestPasswordUpdateRequest request,
            string baseUrl,
            string emailtTemplatePath)
        {
            var result = new BaseResult();

            try
            {
                var appResult = await appsRepository.GetByLicense(request.License);

                if (appResult.Success)
                {
                    var userResult = await usersRepository.GetByEmail(request.Email);

                    if (userResult.Success)
                    {
                        var app = (App)appResult.Object;
                        var user = (User)userResult.Object;

                        if (user.Apps.Any(ua => ua.AppId == app.Id))
                        {
                            if (!user.EmailConfirmed)
                            {
                                result.Success = false;
                                result.Message = UsersMessages.UserEmailNotConfirmed;

                                return result;
                            }

                            var passwordUpdate = new PasswordUpdate(user.Id, app.Id);

                            passwordUpdate = await EnsurePasswordUpdateTokenIsUnique(passwordUpdate);

                            var passwordUpdateResult = await passwordUpdatesRepository.Create(passwordUpdate);

                            if (passwordUpdateResult.Success)
                            {
                                user.ReceivedRequestToUpdatePassword = true;

                                user = (User)(await usersRepository.Update(user)).Object;

                                string emailConfirmationUrl;

                                if (app.UseCustomPasswordUpdateUrl)
                                {
                                    if (app.InDevelopment)
                                    {
                                        emailConfirmationUrl = string.Format("{0}{1}", 
                                            app.CustomPasswordUpdateDevUrl, 
                                            passwordUpdate.Token);
                                    }
                                    else
                                    {
                                        emailConfirmationUrl = string.Format("{0}{1}",
                                            app.CustomPasswordUpdateLiveUrl,
                                            passwordUpdate.Token);
                                    }
                                }
                                else
                                {
                                    emailConfirmationUrl = string.Format("https://{0}/passwordUpdate/{1}",
                                        baseUrl,
                                        passwordUpdate.Token);
                                }

                                var html = File.ReadAllText(emailtTemplatePath);
                                var appTitle = app.Name;
                                var url = string.Empty;

                                if (app.InDevelopment)
                                {
                                    url = app.DevUrl;
                                }
                                else
                                {
                                    url = app.LiveUrl;
                                }

                                html = html.Replace("{{USER_NAME}}", user.UserName);
                                html = html.Replace("{{CONFIRM_EMAIL_URL}}", emailConfirmationUrl);
                                html = html.Replace("{{APP_TITLE}}", appTitle);
                                html = html.Replace("{{URL}}", url);

                                var emailSubject = string.Format("Greetings from {0}: Password Update Request Received", appTitle);

                                result.Success = emailService
                                    .Send(user.Email, emailSubject, html);

                                if (result.Success)
                                {
                                    result.Message = UsersMessages.ProcessedPasswordRequest;

                                    return result;
                                }
                                else
                                {
                                    result.Message = UsersMessages.UnableToProcessPasswordRequest;

                                    return result;
                                }
                            }
                            else if (!passwordUpdateResult.Success && passwordUpdateResult.Exception != null)
                            {
                                result.Success = passwordUpdateResult.Success;
                                result.Message = passwordUpdateResult.Exception.Message;

                                return result;
                            }
                            else
                            {
                                result.Success = userResult.Success;
                                result.Message = UsersMessages.UnableToProcessPasswordRequest;

                                return result;
                            }
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = AppsMessages.UserNotSignedUpToApp;

                            return result;
                        }
                    }
                    else if (!userResult.Success && userResult.Exception != null)
                    {
                        result.Success = userResult.Success;
                        result.Message = userResult.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.Success = userResult.Success;
                        result.Message = AppsMessages.AppNotFoundMessage;

                        return result;
                    }
                }
                else if (!appResult.Success && appResult.Exception != null)
                {
                    result.Success = appResult.Success;
                    result.Message = appResult.Exception.Message;

                    return result;
                }
                else
                {
                    result.Success = appResult.Success;
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

        public async Task<IInitiatePasswordUpdateResult> InitiatePasswordUpdate(string token)
        {
            var result = new InitiatePasswordUpdateResult();

            try
            {
                var passwordUpdateResult = await passwordUpdatesRepository.Get(token);

                if (passwordUpdateResult.Success)
                {
                    var passwordUpdate = (PasswordUpdate)passwordUpdateResult.Object;

                    var userResult = await usersRepository.GetById(passwordUpdate.UserId);

                    if (userResult.Success)
                    {
                        var user = (User)userResult.Object;

                        var appResult = await appsRepository.GetById(passwordUpdate.AppId);

                        if (appResult.Success)
                        {
                            var app = (App)appResult.Object;

                            if (user.Apps.Any(ua => ua.AppId == app.Id))
                            {
                                if (user.ReceivedRequestToUpdatePassword)
                                {
                                    result.Success = true;
                                    result.Message = UsersMessages.UserFoundMessage;
                                    result.User = user;
                                    result.App = app;

                                    return result;
                                }
                                else
                                {
                                    result.Success = false;
                                    result.Message = UsersMessages.NoOutstandingRequestToUpdatePassword;

                                    return result;
                                }
                            }
                            else
                            {
                                result.Success = false;
                                result.Message = AppsMessages.UserNotSignedUpToApp;

                                return result;
                            }
                        }
                        else if (!appResult.Success && appResult.Exception != null)
                        {
                            result.Success = passwordUpdateResult.Success;
                            result.Message = passwordUpdateResult.Exception.Message;

                            return result;
                        }
                        else
                        {
                            result.Success = appResult.Success;
                            result.Message = AppsMessages.AppNotFoundMessage;

                            return result;
                        }
                    }
                    else if (!userResult.Success && userResult.Exception != null)
                    {
                        result.Success = passwordUpdateResult.Success;
                        result.Message = passwordUpdateResult.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.Success = userResult.Success;
                        result.Message = UsersMessages.UserNotFoundMessage;

                        return result;
                    }
                }
                else if (!passwordUpdateResult.Success && passwordUpdateResult.Exception != null)
                {
                    result.Success = passwordUpdateResult.Success;
                    result.Message = passwordUpdateResult.Exception.Message;

                    return result;
                }
                else
                {
                    result.Success = passwordUpdateResult.Success;
                    result.Message = UsersMessages.ProcessPasswordRequestNotFound;

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

        public async Task<IBaseResult> UpdatePassword(IUpdatePasswordRequest request)
        {
            var result = new BaseResult();
            var salt = BCrypt.Net.BCrypt.GenerateSalt();

            try
            {
                if (await usersRepository.HasEntity(request.UserId))
                {
                    var userResponse = await usersRepository.GetById(request.UserId, true);

                    if (userResponse.Success)
                    {
                        var user = (User)userResponse.Object;

                        if (user.ReceivedRequestToUpdatePassword)
                        {
                            if (BCrypt.Net.BCrypt.Verify(request.OldPassword, user.Password))
                            {
                                user.Password = BCrypt.Net.BCrypt
                                        .HashPassword(request.NewPassword, salt);

                                user.DateUpdated = DateTime.UtcNow;

                                user.ReceivedRequestToUpdatePassword = false;

                                var updateUserResponse = await usersRepository.Update(user);

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
                        else
                        {
                            result.Success = false;
                            result.Message = UsersMessages.NoOutstandingRequestToUpdatePassword;

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
                var emailConfirmationResponse = await emailConfirmationsRepository.Get(token);

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
                            result.UserName = user.UserName;

                            result.AppTitle = user
                                .Apps
                                .Where(ua => ua.AppId == emailConfirmation.AppId)
                                .Select(ua => ua.App.Name)
                                .FirstOrDefault();

                            if (user
                                .Apps
                                .Where(ua => ua.AppId == emailConfirmation.AppId)
                                .Select(ua => ua.App.InDevelopment)
                                .FirstOrDefault())
                            {
                                result.Url = user
                                    .Apps
                                    .Where(ua => ua.AppId == emailConfirmation.AppId)
                                    .Select(ua => ua.App.DevUrl)
                                    .FirstOrDefault();
                            }
                            else
                            {
                                result.Url = user
                                    .Apps
                                    .Where(ua => ua.AppId == emailConfirmation.AppId)
                                    .Select(ua => ua.App.LiveUrl)
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
                    else if (emailConfirmation.IsUpdate && !(bool)emailConfirmation.OldEmailAddressConfirmed)
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
                            var appTitle = app.Name;
                            var url = string.Empty;

                            if (app.InDevelopment)
                            {
                                url = app.DevUrl;
                            }
                            else
                            {
                                url = app.LiveUrl;
                            }

                            html = html.Replace("{{USER_NAME}}", user.UserName);
                            html = html.Replace("{{CONFIRM_EMAIL_URL}}", emailConfirmationUrl);
                            html = html.Replace("{{APP_TITLE}}", appTitle);
                            html = html.Replace("{{URL}}", url);

                            var emailSubject = string.Format("Greetings from {0}: Please Confirm New Email", appTitle);

                            result.ConfirmationEmailSuccessfullySent = emailService
                                .Send(user.Email, emailSubject, html);

                            emailConfirmation.OldEmailAddressConfirmed = true;

                            emailConfirmation = (EmailConfirmation)(await emailConfirmationsRepository.Update(emailConfirmation)).Object;

                            result.Success = response.Success;
                            result.UserName = user.UserName;
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
                            result.UserName = user.UserName;
                            result.IsUpdate = emailConfirmation.IsUpdate;
                            result.NewEmailAddressConfirmed = true;
                            result.AppTitle = user
                                .Apps
                                .Where(ua => ua.AppId == emailConfirmation.AppId)
                                .Select(ua => ua.App.Name)
                                .FirstOrDefault();


                            if (user
                                .Apps
                                .Where(ua => ua.AppId == emailConfirmation.AppId)
                                .Select(ua => ua.App.InDevelopment)
                                .FirstOrDefault())
                            {
                                result.Url = user
                                    .Apps
                                    .Where(ua => ua.AppId == emailConfirmation.AppId)
                                    .Select(ua => ua.App.DevUrl)
                                    .FirstOrDefault();
                            }
                            else
                            {
                                result.Url = user
                                    .Apps
                                    .Where(ua => ua.AppId == emailConfirmation.AppId)
                                    .Select(ua => ua.App.LiveUrl)
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

        private async Task<EmailConfirmation> EnsureEmailConfirmationTokenIsUnique(EmailConfirmation emailConfirmation)
        {
            var emailConfirmationResposnse = await emailConfirmationsRepository.GetAll();

            if (emailConfirmationResposnse.Success)
            {
                bool tokenNotUnique;

                var emailConfirmations = emailConfirmationResposnse
                    .Objects
                    .ConvertAll(ec => (EmailConfirmation)ec);

                do
                {
                    if (emailConfirmations
                        .Any(ec => ec.Token.ToLower()
                        .Equals(emailConfirmation.Token.ToLower())))
                    {
                        tokenNotUnique = true;

                        emailConfirmation.Token = Guid.NewGuid().ToString();
                    }
                    else
                    {
                        tokenNotUnique = false;
                    }

                } while (tokenNotUnique);
            }

            return emailConfirmation;
        }

        private async Task<PasswordUpdate> EnsurePasswordUpdateTokenIsUnique(PasswordUpdate passwordUpdate)
        {
            var passwordUpdateResponse = await passwordUpdatesRepository.GetAll();

            if (passwordUpdateResponse.Success)
            {
                bool tokenNotUnique;

                var passwordUpdates = passwordUpdateResponse
                    .Objects
                    .ConvertAll(pu => (PasswordUpdate)pu);

                do
                {
                    if (passwordUpdates
                        .Any(ec => ec.Token.ToLower()
                        .Equals(passwordUpdate.Token.ToLower())))
                    {
                        tokenNotUnique = true;

                        passwordUpdate.Token = Guid.NewGuid().ToString();
                    }
                    else
                    {
                        tokenNotUnique = false;
                    }

                } while (tokenNotUnique);
            }

            return passwordUpdate;
        }
        #endregion
    }
}

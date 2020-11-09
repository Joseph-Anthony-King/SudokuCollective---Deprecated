using System;
using System.Collections.Generic;
using System.Linq;
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

namespace SudokuCollective.Data.Services
{
    public class AppsService : IAppsService
    {
        #region Fields
        private readonly IAppsRepository<App> appsRepository;
        private readonly IUsersRepository<User> usersRepository;
        private readonly string appNotFoundMessage;
        private readonly string appsNotFoundMessage;
        private readonly string errorCreatingAppMessage;
        private readonly string appUpdatedMessage;
        private readonly string failedAppUpdatedMessage;
        private readonly string failedAddUserMessage;
        private readonly string failedRemoveUserMessage;
        private readonly string appResetMessage;
        private readonly string failedAppResetMessage;
        private readonly string appDeletedMessage;
        private readonly string failedAppDeletedMessage;
        private readonly string appActivatedMessage;
        private readonly string failedAppActivatedMessage;
        private readonly string appDeactivatedMessage;
        private readonly string failedAppDeactivatedMessage;
        private readonly string usersNotFoundMessage;
        private readonly string userDoesNotExistMessage;
        private readonly string sortValueNotImplementedMessage;
        #endregion

        #region Constructor
        public AppsService(IAppsRepository<App> appRepo, IUsersRepository<User> userRepo)
        {
            appsRepository = appRepo;
            usersRepository = userRepo;
            appNotFoundMessage = "App not found";
            appsNotFoundMessage = "Apps not found";
            errorCreatingAppMessage = "Error creating app";
            appUpdatedMessage = "App successfully updated";
            failedAppUpdatedMessage = "Unable to update app";
            failedAddUserMessage = "Unable to add user to app";
            failedRemoveUserMessage = "Unable to remove user from app";
            appResetMessage = "App successfully reset";
            failedAppResetMessage = "Unable to reset app";
            appDeletedMessage = "App successfully deleted";
            failedAppDeletedMessage = "Unable to delete app";
            appActivatedMessage = "App activated";
            failedAppActivatedMessage = "Unable to activate app";
            appDeactivatedMessage = "App deactivated";
            failedAppDeactivatedMessage = "Unable to deactivate app";
            usersNotFoundMessage = "Users not found";
            userDoesNotExistMessage = "User does not exist";
            sortValueNotImplementedMessage = "Sorting not implemented for this sort value";
        }
        #endregion

        #region Methods
        public async Task<IAppResult> GetApp(int id, bool fullRecord = false)
        {
            var result = new AppResult();

            try
            {
                if (await appsRepository.HasEntity(id))
                {
                    var response = await appsRepository.GetById(id, fullRecord);

                    if (response.Success)
                    {
                        result.Success = response.Success;
                        result.App = (IApp)response.Object;

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

        public async Task<IAppResult> GetAppByLicense(string license, bool fullRecord = false)
        {
            var result = new AppResult();

            try
            {
                if (await appsRepository.IsAppLicenseValid(license))
                {
                    var response = await appsRepository.GetByLicense(license, fullRecord);

                    if (response.Success)
                    {
                        result.Success = response.Success;
                        result.App = (IApp)response.Object;

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

        public async Task<IAppsResult> GetApps(
            IPageListModel pageListModel,
            bool fullRecord = false)
        {
            var result = new AppsResult();

            try
            {
                var response = await appsRepository.GetAll(fullRecord);

                if (response.Success)
                {
                    if (pageListModel.SortBy == SortValue.NULL)
                    {
                        result.Apps = response.Objects.ConvertAll(a => (IApp)a);
                    }
                    else if (pageListModel.SortBy == SortValue.ID)
                    {
                        if (!pageListModel.OrderByDescending)
                        {
                            result.Apps = (List<IApp>)response.Objects
                                .ConvertAll(a => (IApp)a)
                                .OrderBy(a => a.Id)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage);
                        }
                        else
                        {
                            result.Apps = (List<IApp>)response.Objects
                                .ConvertAll(a => (IApp)a)
                                .OrderByDescending(a => a.Id)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage);
                        }
                    }
                    else if (pageListModel.SortBy == SortValue.GAMECOUNT)
                    {
                        if (!pageListModel.OrderByDescending)
                        {
                            result.Apps = (List<IApp>)response.Objects
                                .ConvertAll(a => (IApp)a)
                                .OrderBy(a => a.GameCount)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage);
                        }
                        else
                        {
                            result.Apps = (List<IApp>)response.Objects
                                .ConvertAll(a => (IApp)a)
                                .OrderByDescending(a => a.GameCount)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage);
                        }
                    }
                    else if (pageListModel.SortBy == SortValue.NAME)
                    {
                        if (!pageListModel.OrderByDescending)
                        {
                            result.Apps = (List<IApp>)response.Objects
                                .ConvertAll(a => (IApp)a)
                                .OrderBy(a => a.Name)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage);
                        }
                        else
                        {
                            result.Apps = (List<IApp>)response.Objects
                                .ConvertAll(a => (IApp)a)
                                .OrderByDescending(a => a.Name)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage);
                        }
                    }
                    else if (pageListModel.SortBy == SortValue.DATECREATED)
                    {
                        if (!pageListModel.OrderByDescending)
                        {
                            result.Apps = (List<IApp>)response.Objects
                                .ConvertAll(a => (IApp)a)
                                .OrderBy(a => a.DateCreated)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage);
                        }
                        else
                        {
                            result.Apps = (List<IApp>)response.Objects
                                .ConvertAll(a => (IApp)a)
                                .OrderByDescending(a => a.DateCreated)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage);
                        }
                    }
                    else if (pageListModel.SortBy == SortValue.DATEUPDATED)
                    {
                        if (!pageListModel.OrderByDescending)
                        {
                            result.Apps = (List<IApp>)response.Objects
                                .ConvertAll(a => (IApp)a)
                                .OrderBy(a => a.DateUpdated)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage);
                        }
                        else
                        {
                            result.Apps = (List<IApp>)response.Objects
                                .ConvertAll(a => (IApp)a)
                                .OrderByDescending(a => a.DateUpdated)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage);
                        }
                    }
                    else if (pageListModel.SortBy == SortValue.USERCOUNT)
                    {
                        if (!pageListModel.OrderByDescending)
                        {
                            result.Apps = (List<IApp>)response.Objects
                                .ConvertAll(a => (IApp)a)
                                .OrderBy(a => a.Users.Count)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage);
                        }
                        else
                        {
                            result.Apps = (List<IApp>)response.Objects
                                .ConvertAll(a => (IApp)a)
                                .OrderByDescending(a => a.Users.Count)
                                .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                .Take(pageListModel.ItemsPerPage);
                        }
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = sortValueNotImplementedMessage;

                        return result;
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
                    result.Message = appsNotFoundMessage;

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

        public async Task<IUsersResult> GetAppUsers(
            int id,
            IPageListModel pageListModel,
            bool fullRecord = false)
        {
            var result = new UsersResult();

            try
            {
                if (await appsRepository.HasEntity(id))
                {
                    var response = await appsRepository.GetAppUsers(id, fullRecord);

                    if (response.Success)
                    {

                        if (pageListModel.SortBy == SortValue.NULL)
                        {
                            result.Users = response.Objects.ConvertAll(u => (IUser)u);
                        }
                        else if (pageListModel.SortBy == SortValue.ID)
                        {
                            if (!pageListModel.OrderByDescending)
                            {
                                result.Users = (List<IUser>)response.Objects
                                    .ConvertAll(u => (IUser)u)
                                    .OrderBy(u => u.Id)
                                    .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                    .Take(pageListModel.ItemsPerPage);
                            }
                            else
                            {
                                result.Users = (List<IUser>)response.Objects
                                    .ConvertAll(u => (IUser)u)
                                    .OrderByDescending(u => u.Id)
                                    .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                    .Take(pageListModel.ItemsPerPage);
                            }
                        }
                        else if (pageListModel.SortBy == SortValue.USERNAME)
                        {
                            if (!pageListModel.OrderByDescending)
                            {
                                result.Users = (List<IUser>)response.Objects
                                    .ConvertAll(u => (IUser)u)
                                    .OrderBy(u => u.UserName)
                                    .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                    .Take(pageListModel.ItemsPerPage);
                            }
                            else
                            {
                                result.Users = (List<IUser>)response.Objects
                                    .ConvertAll(u => (IUser)u)
                                    .OrderByDescending(u => u.UserName)
                                    .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                    .Take(pageListModel.ItemsPerPage);
                            }
                        }
                        else if (pageListModel.SortBy == SortValue.FIRSTNAME)
                        {
                            if (!pageListModel.OrderByDescending)
                            {
                                result.Users = (List<IUser>)response.Objects
                                    .ConvertAll(u => (IUser)u)
                                    .OrderBy(u => u.FirstName)
                                    .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                    .Take(pageListModel.ItemsPerPage);
                            }
                            else
                            {
                                result.Users = (List<IUser>)response.Objects
                                    .ConvertAll(u => (IUser)u)
                                    .OrderByDescending(u => u.FirstName)
                                    .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                    .Take(pageListModel.ItemsPerPage);
                            }
                        }
                        else if (pageListModel.SortBy == SortValue.LASTNAME)
                        {
                            if (!pageListModel.OrderByDescending)
                            {
                                result.Users = (List<IUser>)response.Objects
                                    .ConvertAll(u => (IUser)u)
                                    .OrderBy(u => u.LastName)
                                    .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                    .Take(pageListModel.ItemsPerPage);
                            }
                            else
                            {
                                result.Users = (List<IUser>)response.Objects
                                    .ConvertAll(u => (IUser)u)
                                    .OrderByDescending(u => u.LastName)
                                    .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                    .Take(pageListModel.ItemsPerPage);
                            }
                        }
                        else if (pageListModel.SortBy == SortValue.FULLNAME)
                        {
                            if (!pageListModel.OrderByDescending)
                            {
                                result.Users = (List<IUser>)response.Objects
                                    .ConvertAll(u => (IUser)u)
                                    .OrderBy(u => u.FullName)
                                    .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                    .Take(pageListModel.ItemsPerPage);
                            }
                            else
                            {
                                result.Users = (List<IUser>)response.Objects
                                    .ConvertAll(u => (IUser)u)
                                    .OrderByDescending(u => u.FullName)
                                    .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                    .Take(pageListModel.ItemsPerPage);
                            }
                        }
                        else if (pageListModel.SortBy == SortValue.NICKNAME)
                        {
                            if (!pageListModel.OrderByDescending)
                            {
                                result.Users = (List<IUser>)response.Objects
                                    .ConvertAll(u => (IUser)u)
                                    .OrderBy(u => u.NickName)
                                    .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                    .Take(pageListModel.ItemsPerPage);
                            }
                            else
                            {
                                result.Users = (List<IUser>)response.Objects
                                    .ConvertAll(u => (IUser)u)
                                    .OrderByDescending(u => u.NickName)
                                    .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                    .Take(pageListModel.ItemsPerPage);
                            }
                        }
                        else if (pageListModel.SortBy == SortValue.GAMECOUNT)
                        {
                            if (!pageListModel.OrderByDescending)
                            {
                                result.Users = (List<IUser>)response.Objects
                                    .ConvertAll(u => (IUser)u)
                                    .OrderBy(u => u.Games.Count)
                                    .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                    .Take(pageListModel.ItemsPerPage);
                            }
                            else
                            {
                                result.Users = (List<IUser>)response.Objects
                                    .ConvertAll(u => (IUser)u)
                                    .OrderByDescending(u => u.Games.Count)
                                    .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                    .Take(pageListModel.ItemsPerPage);
                            }
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = sortValueNotImplementedMessage;

                            return result;
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

        public async Task<IAppResult> CreateApp(ILicenseRequest licenseRequest)
        {
            var result = new AppResult();

            try
            {
                if (await usersRepository.IsUserRegistered(licenseRequest.OwnerId))
                {
                    var generatingGuid = true;
                    var license = new Guid();

                    /* Ensure the license is unique by pulling all apps from the repository
                     * and checking that the new license is unique */
                    var checkAppsResponse = await appsRepository.GetAll();

                    do
                    {
                        license = Guid.NewGuid();

                        if (!checkAppsResponse.Objects.ConvertAll(a => (IApp)a).Any(a => a.License.Equals(license.ToString())))
                        {
                            generatingGuid = false;
                        }
                        else
                        {
                            generatingGuid = true;
                        }

                    } while (generatingGuid);

                    var app = new App(
                        licenseRequest.Name,
                        license.ToString(),
                        licenseRequest.OwnerId,
                        licenseRequest.DevUrl,
                        licenseRequest.LiveUrl);

                    var addAppResponse = await appsRepository.Create(app);

                    if (addAppResponse.Success)
                    {
                        result.Success = addAppResponse.Success;
                        result.App = (IApp)addAppResponse.Object;

                        return result;
                    }
                    else if (!addAppResponse.Success && addAppResponse.Exception != null)
                    {
                        result.Success = addAppResponse.Success;
                        result.Message = addAppResponse.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = errorCreatingAppMessage;

                        return result;
                    }

                }
                else
                {
                    result.Success = false;
                    result.Message = userDoesNotExistMessage;

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

        public async Task<ILicenseResult> GetLicense(int id)
        {
            var result = new LicenseResult();

            try
            {
                if (await appsRepository.HasEntity(id))
                {
                    result.Success = true;
                    result.License = await appsRepository.GetLicense(id);

                    return result;
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

        public async Task<IBaseResult> UpdateApp(IAppRequest appRequest)
        {
            var result = new BaseResult();

            try
            {
                if (await appsRepository.HasEntity(appRequest.AppId))
                {
                    var getAppResponse = await appsRepository.GetById(appRequest.AppId);

                    if (getAppResponse.Success)
                    {
                        ((IApp)getAppResponse.Object).Name = appRequest.Name;
                        ((IApp)getAppResponse.Object).DevUrl = appRequest.DevUrl;
                        ((IApp)getAppResponse.Object).LiveUrl = appRequest.LiveUrl;

                        var updateAppResponse = await appsRepository
                            .Update((App)getAppResponse.Object);

                        if (updateAppResponse.Success)
                        {
                            result.Success = true;
                            result.Message = appUpdatedMessage;

                            return result;
                        }
                        else if (!updateAppResponse.Success && updateAppResponse.Exception != null)
                        {
                            result.Success = updateAppResponse.Success;
                            result.Message = updateAppResponse.Exception.Message;

                            return result;
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = failedAppUpdatedMessage;

                            return result;
                        }
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = getAppResponse.Exception.Message;

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

        public async Task<IBaseResult> AddAppUser(int userId, IBaseRequest baseRequest)
        {
            var result = new BaseResult();

            try
            {
                if (await appsRepository.IsAppLicenseValid(baseRequest.License))
                {
                    var addUserToAppResponse = await appsRepository.AddAppUser(
                        userId,
                        baseRequest.License);

                    if (addUserToAppResponse.Success)
                    {
                        result.Success = addUserToAppResponse.Success;

                        return result;
                    }
                    else if (!addUserToAppResponse.Success && addUserToAppResponse.Exception != null)
                    {
                        result.Success = addUserToAppResponse.Success;
                        result.Message = addUserToAppResponse.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = failedAddUserMessage;

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
                result.Message = e.Message;

                return result;
            }
        }

        public async Task<IBaseResult> RemoveAppUser(int userId, IBaseRequest baseRequest)
        {
            var result = new BaseResult();

            try
            {
                if (await appsRepository.IsAppLicenseValid(baseRequest.License))
                {
                    var addUserToAppResponse = await appsRepository.RemoveAppUser(
                        userId,
                        baseRequest.License);

                    if (addUserToAppResponse.Success)
                    {
                        result.Success = addUserToAppResponse.Success;

                        return result;
                    }
                    else if (!addUserToAppResponse.Success && addUserToAppResponse.Exception != null)
                    {
                        result.Success = addUserToAppResponse.Success;
                        result.Message = addUserToAppResponse.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = failedRemoveUserMessage;

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
                result.Message = e.Message;

                return result;
            }
        }

        public async Task<IBaseResult> DeleteOrResetApp(int id, bool isReset = false)
        {
            var result = new AppResult();

            try
            {
                if (await appsRepository.HasEntity(id))
                {
                    var getAppResponse = await appsRepository.GetById(id, true);

                    if (isReset)
                    {
                        if (getAppResponse.Success)
                        {
                            var resetAppResponse = await appsRepository.ResetApp((App)getAppResponse.Object);

                            if (resetAppResponse.Success)
                            {
                                result.Success = resetAppResponse.Success;
                                result.Message = appResetMessage;

                                return result;
                            }
                            else if (!resetAppResponse.Success && resetAppResponse.Exception != null)
                            {
                                result.Success = resetAppResponse.Success;
                                result.Message = resetAppResponse.Exception.Message;

                                return result;
                            }
                            else
                            {
                                result.Success = false;
                                result.Message = failedAppResetMessage;

                                return result;
                            }
                        }
                        else if (!getAppResponse.Success && getAppResponse.Exception != null)
                        {
                            result.Success = getAppResponse.Success;
                            result.Message = getAppResponse.Exception.Message;

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

                        if (getAppResponse.Success)
                        {
                            var deleteAppResponse = await appsRepository.Delete((App)getAppResponse.Object);

                            if (deleteAppResponse.Success)
                            {
                                result.Success = deleteAppResponse.Success;
                                result.Message = appDeletedMessage;

                                return result;
                            }
                            else if (!deleteAppResponse.Success && deleteAppResponse.Exception != null)
                            {
                                result.Success = deleteAppResponse.Success;
                                result.Message = deleteAppResponse.Exception.Message;

                                return result;
                            }
                            else
                            {
                                result.Success = false;
                                result.Message = failedAppDeletedMessage;

                                return result;
                            }
                        }
                        else if (!getAppResponse.Success && getAppResponse.Exception != null)
                        {
                            result.Success = getAppResponse.Success;
                            result.Message = getAppResponse.Exception.Message;

                            return result;
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = appNotFoundMessage;

                            return result;
                        }
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

        public async Task<IBaseResult> ActivateApp(int id)
        {
            var result = new BaseResult();

            try
            {
                if (await appsRepository.HasEntity(id))
                {
                    var activateAppResponse = await appsRepository.ActivateApp(id);

                    if (activateAppResponse.Success)
                    {
                        result.Success = activateAppResponse.Success;
                        result.Message = appActivatedMessage;

                        return result;
                    }
                    else if (!activateAppResponse.Success && activateAppResponse.Exception != null)
                    {
                        result.Success = activateAppResponse.Success;
                        result.Message = activateAppResponse.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = failedAppActivatedMessage;

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

        public async Task<IBaseResult> DeactivateApp(int id)
        {
            var result = new BaseResult();

            try
            {
                if (await appsRepository.HasEntity(id))
                {
                    var activateAppResponse = await appsRepository.DeactivateApp(id);

                    if (activateAppResponse.Success)
                    {
                        result.Success = activateAppResponse.Success;
                        result.Message = appDeactivatedMessage;

                        return result;
                    }
                    else if (!activateAppResponse.Success && activateAppResponse.Exception != null)
                    {
                        result.Success = activateAppResponse.Success;
                        result.Message = activateAppResponse.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = failedAppDeactivatedMessage;

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

        public async Task<bool> IsRequestValidOnThisLicense(int id, string license, int userId)
        {
            var requestor = (User)(await usersRepository.GetById(userId, true)).Object;
            var validLicense = await appsRepository.IsAppLicenseValid(license);
            var requestorRegisteredToApp = await appsRepository.IsUserRegisteredToApp(id, license, userId);

            if (requestorRegisteredToApp && validLicense)
            {
                return true;
            }
            else if (requestor.IsSuperUser && validLicense)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> IsOwnerOfThisLicense(int id, string license, int userId)
        {
            var requestor = await usersRepository.GetById(userId, false);
            var validLicense = await appsRepository.IsAppLicenseValid(license);
            var requestorOwnerOfThisApp = await appsRepository.IsUserOwnerOfApp(id, license, userId);

            if (requestorOwnerOfThisApp && validLicense)
            {
                return true;
            }
            else if (((User)requestor).IsSuperUser && validLicense)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.APIModels.PageModels;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Interfaces.DataModels;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Core.Models;
using SudokuCollective.Core.Interfaces.Repositories;
using SudokuCollective.Data.Helpers;
using SudokuCollective.Data.Messages;
using SudokuCollective.Data.Models.ResultModels;
using SudokuCollective.Data.Resiliency;
using SudokuCollective.Data.Models.DataModels;

namespace SudokuCollective.Data.Services
{
    public class AppsService : IAppsService
    {
        #region Fields
        private readonly IAppsRepository<App> _appsRepository;
        private readonly IUsersRepository<User> _usersRepository;
        private readonly IAppAdminsRepository<AppAdmin> _appAdminsRepository;
        private readonly IRolesRepository<Role> _rolesRepository;
        private readonly IDistributedCache _distributedCache;
        #endregion

        #region Constructor
        public AppsService(
            IAppsRepository<App> appRepository, 
            IUsersRepository<User> userRepository,
            IAppAdminsRepository<AppAdmin> appAdminsRepository,
            IRolesRepository<Role> rolesRepository,
            IDistributedCache distributedCache)
        {
            _appsRepository = appRepository;
            _usersRepository = userRepository;
            _appAdminsRepository = appAdminsRepository;
            _rolesRepository = rolesRepository;
            _distributedCache = distributedCache;
        }
        #endregion

        #region Methods
        public async Task<IAppResult> Create(ILicenseRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var result = new AppResult();

            try
            {
                // Ensure the intended owner exists by pull the records from the repository
                var cacheFactoryResponse = await CacheFactory.GetWithCacheAsync<User>(
                       _usersRepository,
                       _distributedCache,
                       string.Format(CacheKeys.GetUserCacheKey, request.OwnerId),
                       DateTime.Now.AddMinutes(5),
                       request.OwnerId);

                var userResponse = (RepositoryResponse)cacheFactoryResponse.Item1;

                if (userResponse.Success)
                {
                    var user = (User)userResponse.Object;

                    var generatingGuid = true;
                    var license = new Guid();

                    /* Ensure the license is unique by pulling all apps from the repository
                     * and checking that the new license is unique */
                    cacheFactoryResponse = await CacheFactory.GetAllWithCacheAsync<App>(
                        _appsRepository,
                        _distributedCache,
                        string.Format(CacheKeys.GetAppsCacheKey),
                        DateTime.Now.AddMinutes(5));

                    var checkAppsResponse = (RepositoryResponse)cacheFactoryResponse.Item1;

                    do
                    {
                        license = Guid.NewGuid();

                        if (!checkAppsResponse
                            .Objects
                            .ConvertAll(a => (IApp)a)
                            .Any(a => a.License.Equals(license.ToString())))
                        {
                            generatingGuid = false;
                        }
                        else
                        {
                            generatingGuid = true;
                        }

                    } while (generatingGuid);

                    var app = new App(
                        request.Name,
                        license.ToString(),
                        request.OwnerId,
                        request.DevUrl,
                        request.LiveUrl);

                    var addAppResponse = await _appsRepository.Add(app);

                    if (addAppResponse.Success)
                    {
                        if (user.Roles.Any(ur => ur.Role.RoleLevel == RoleLevel.ADMIN))
                        {
                            var appAdmin = new AppAdmin(app.Id, user.Id);

                            _ = await _appAdminsRepository.Add(appAdmin);
                        }

                        result.Success = addAppResponse.Success;
                        result.Message = AppsMessages.AppCreatedMessage;
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
                        result.Message = AppsMessages.AppNotCreatedMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = UsersMessages.UserDoesNotExistMessage;

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

        public async Task<IAppResult> Get(
            int id, 
            int requestorId)
        {
            var result = new AppResult();

            if (id == 0 || requestorId == 0)
            {
                result.Success = false;
                result.Message = UsersMessages.UserNotFoundMessage;

                return result;
            }

            try
            {
                var cacheFactoryResponse = await CacheFactory.GetWithCacheAsync<App>(
                    _appsRepository,
                    _distributedCache,
                    string.Format(CacheKeys.GetAppCacheKey, id),
                    DateTime.Now.AddMinutes(5),
                    id,
                    result);

                var response = (RepositoryResponse)cacheFactoryResponse.Item1;
                result = (AppResult)cacheFactoryResponse.Item2;

                if (response.Success)
                {
                    var app = (App)response.Object;

                    cacheFactoryResponse = await CacheFactory.GetWithCacheAsync<User>(
                        _usersRepository,
                        _distributedCache,
                        string.Format(CacheKeys.GetUserCacheKey, id),
                        DateTime.Now.AddMinutes(5),
                        requestorId);

                    var requestor = (User)((RepositoryResponse)cacheFactoryResponse.Item1).Object;

                    if (requestor != null && !requestor.IsSuperUser)
                    {
                        /* Filter out user emails from the frontend if 
                         * requestor is not a super user... */
                        foreach (var userApp in app.Users)
                        {
                            var emailConfirmed = userApp.User.EmailConfirmed;
                            userApp.User.Email = null;
                            userApp.User.EmailConfirmed = emailConfirmed;
                        }
                    }

                    result.Success = response.Success;
                    result.Message = AppsMessages.AppFoundMessage;
                    result.App = app;

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

        public async Task<IAppResult> Update(int id, IAppRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var result = new AppResult();

            if (id == 0)
            {
                result.Success = false;
                result.Message = AppsMessages.AppNotFoundMessage;

                return result;
            }

            try
            {
                var getAppResponse = await _appsRepository.Get(id);

                if (getAppResponse.Success)
                {
                    if (getAppResponse.Success)
                    {
                        ((IApp)getAppResponse.Object).Name = request.Name;
                        ((IApp)getAppResponse.Object).DevUrl = request.DevUrl;
                        ((IApp)getAppResponse.Object).LiveUrl = request.LiveUrl;
                        ((IApp)getAppResponse.Object).IsActive = request.IsActive;
                        ((IApp)getAppResponse.Object).InDevelopment = request.InDevelopment;
                        ((IApp)getAppResponse.Object).PermitSuperUserAccess = request.PermitSuperUserAccess;
                        ((IApp)getAppResponse.Object).PermitCollectiveLogins = request.PermitCollectiveLogins;
                        ((IApp)getAppResponse.Object).DisableCustomUrls = request.DisableCustomUrls;
                        ((IApp)getAppResponse.Object).CustomEmailConfirmationAction = request.CustomEmailConfirmationAction;
                        ((IApp)getAppResponse.Object).CustomPasswordResetAction = request.CustomPasswordResetAction;
                        ((IApp)getAppResponse.Object).TimeFrame = request.TimeFrame;
                        ((IApp)getAppResponse.Object).AccessDuration = request.AccessDuration;
                        ((IApp)getAppResponse.Object).DateUpdated = DateTime.UtcNow;

                        var updateAppResponse = await _appsRepository
                            .Update((App)getAppResponse.Object);

                        if (updateAppResponse.Success)
                        {
                            result.Success = true;
                            result.Message = AppsMessages.AppUpdatedMessage;
                            result.App = (App)updateAppResponse.Object;

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
                            result.Message = AppsMessages.AppNotUpdatedMessage;

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

        public async Task<IBaseResult> DeleteOrReset(int id, bool isReset = false)
        {
            var result = new AppResult();

            if (id == 0)
            {
                result.Success = false;
                result.Message = AppsMessages.AppNotFoundMessage;

                return result;
            }

            try
            {
                var getAppResponse = await _appsRepository.Get(id);

                if (getAppResponse.Success)
                {
                    if (isReset)
                    {
                        if (getAppResponse.Success)
                        {
                            var resetAppResponse = await _appsRepository.Reset((App)getAppResponse.Object);

                            if (resetAppResponse.Success)
                            {
                                result.Success = resetAppResponse.Success;
                                result.Message = AppsMessages.AppResetMessage;

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
                                result.Message = AppsMessages.AppNotFoundMessage;

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
                            result.Message = AppsMessages.AppNotFoundMessage;

                            return result;
                        }
                    }
                    else
                    {
                        if (id == 1)
                        {
                            result.Success = false;
                            result.Message = AppsMessages.AdminAppCannotBeDeletedMessage;

                            return result;
                        }

                        if (getAppResponse.Success)
                        {
                            var deleteAppResponse = await _appsRepository.Delete((App)getAppResponse.Object);

                            if (deleteAppResponse.Success)
                            {
                                result.Success = deleteAppResponse.Success;
                                result.Message = AppsMessages.AppDeletedMessage;

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
                                result.Message = AppsMessages.AppNotDeletedMessage;

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
                            result.Message = AppsMessages.AppNotFoundMessage;

                            return result;
                        }
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

        public async Task<IAppResult> GetAppByLicense(
            string license, 
            int requestorId)
        {
            var result = new AppResult();

            if (string.IsNullOrEmpty(license) || requestorId == 0)
            {
                result.Success = false;
                result.Message = UsersMessages.UserNotFoundMessage;

                return result;
            }

            try
            {
                var cacheFactoryResponse = await CacheFactory.GetAppByLicenseWithCacheAsync(
                    _appsRepository,
                    _distributedCache,
                    string.Format(CacheKeys.GetAppByLicenseCacheKey, license),
                    DateTime.Now.AddMinutes(5),
                    license,
                    result);

                var response = (RepositoryResponse)cacheFactoryResponse.Item1;
                result = (AppResult)cacheFactoryResponse.Item2;

                if (response.Success)
                {
                    var app = (IApp)response.Object;

                    result.Success = response.Success;
                    result.Message = AppsMessages.AppFoundMessage;
                    result.App = app;

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

        public async Task<IAppsResult> GetApps(
            IPaginator paginator,
            int requestorId)
        {
            if (paginator == null) throw new ArgumentNullException(nameof(paginator));

            var result = new AppsResult();

            if (requestorId == 0)
            {
                result.Success = false;
                result.Message = UsersMessages.UserNotFoundMessage;

                return result;
            }

            try
            {
                var cacheFactoryResponse = await CacheFactory.GetAllWithCacheAsync<App>(
                    _appsRepository,
                    _distributedCache,
                    string.Format(CacheKeys.GetAppsCacheKey),
                    DateTime.Now.AddMinutes(5),
                    result);

                var response = (RepositoryResponse)cacheFactoryResponse.Item1;
                result = (AppsResult)cacheFactoryResponse.Item2;

                if (response.Success)
                {
                    if (StaticDataHelpers.IsPageValid(paginator, response.Objects))
                    {
                        if (paginator.SortBy == SortValue.NULL)
                        {
                            result.Apps = response.Objects.ConvertAll(a => (IApp)a);
                        }
                        else if (paginator.SortBy == SortValue.ID)
                        {
                            if (!paginator.OrderByDescending)
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Apps.Add((IApp)obj);
                                }

                                result.Apps = result.Apps
                                    .OrderBy(a => a.Id)
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
                                    .ToList();
                            }
                            else
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Apps.Add((IApp)obj);
                                }

                                result.Apps = result.Apps
                                    .OrderByDescending(a => a.Id)
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
                                    .ToList();
                            }
                        }
                        else if (paginator.SortBy == SortValue.USERCOUNT)
                        {
                            if (!paginator.OrderByDescending)
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Apps.Add((IApp)obj);
                                }

                                result.Apps = result.Apps
                                    .OrderBy(a => a.UserCount)
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
                                    .ToList();
                            }
                            else
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Apps.Add((IApp)obj);
                                }

                                result.Apps = result.Apps
                                    .OrderByDescending(a => a.UserCount)
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
                                    .ToList();
                            }
                        }
                        else if (paginator.SortBy == SortValue.NAME)
                        {
                            if (!paginator.OrderByDescending)
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Apps.Add((IApp)obj);
                                }

                                result.Apps = result.Apps
                                    .OrderBy(a => a.Name)
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
                                    .ToList();
                            }
                            else
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Apps.Add((IApp)obj);
                                }

                                result.Apps = result.Apps
                                    .OrderByDescending(a => a.Name)
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
                                    .ToList();
                            }
                        }
                        else if (paginator.SortBy == SortValue.DATECREATED)
                        {
                            if (!paginator.OrderByDescending)
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Apps.Add((IApp)obj);
                                }

                                result.Apps = result.Apps
                                    .OrderBy(a => a.DateCreated)
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
                                    .ToList();
                            }
                            else
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Apps.Add((IApp)obj);
                                }

                                result.Apps = result.Apps
                                    .OrderByDescending(a => a.DateCreated)
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
                                    .ToList();
                            }
                        }
                        else if (paginator.SortBy == SortValue.DATEUPDATED)
                        {
                            if (!paginator.OrderByDescending)
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Apps.Add((IApp)obj);
                                }

                                result.Apps = result.Apps
                                    .OrderBy(a => a.DateUpdated)
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
                                    .ToList();
                            }
                            else
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Apps.Add((IApp)obj);
                                }

                                result.Apps = result.Apps
                                    .OrderByDescending(a => a.DateUpdated)
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
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

                    result.Success = response.Success;
                    result.Message = AppsMessages.AppsFoundMessage;

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
                    result.Message = AppsMessages.AppsNotFoundMessage;

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

        public async Task<IAppsResult> GetMyApps(
            int ownerId, 
            IPaginator paginator)
        {
            if (paginator == null) throw new ArgumentNullException(nameof(paginator));

            var result = new AppsResult();

            if (ownerId == 0)
            {
                result.Success = false;
                result.Message = UsersMessages.UserNotFoundMessage;

                return result;
            }

            try
            {
                var cacheFactoryResponse = await CacheFactory.GetMyAppsWithCacheAsync(
                    _appsRepository,
                    _distributedCache,
                    string.Format(string.Format(CacheKeys.GetMyAppsCacheKey, ownerId)),
                    DateTime.Now.AddMinutes(5),
                    ownerId,
                    result);

                var response = (RepositoryResponse)cacheFactoryResponse.Item1;
                result = (AppsResult)cacheFactoryResponse.Item2;

                if (response.Success)
                {
                    if (StaticDataHelpers.IsPageValid(paginator, response.Objects))
                    {
                        if (paginator.SortBy == SortValue.NULL)
                        {
                            result.Apps = response
                                .Objects
                                .ConvertAll(a => (IApp)a)
                                .Where(a => a.OwnerId == ownerId)
                                .ToList();
                        }
                        else if (paginator.SortBy == SortValue.ID)
                        {
                            if (!paginator.OrderByDescending)
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Apps.Add((IApp)obj);
                                }

                                result.Apps = result.Apps
                                    .Where(a => a.OwnerId == ownerId)
                                    .OrderBy(a => a.Id)
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
                                    .ToList();
                            }
                            else
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Apps.Add((IApp)obj);
                                }

                                result.Apps = result.Apps
                                    .Where(a => a.OwnerId == ownerId)
                                    .OrderByDescending(a => a.Id)
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
                                    .ToList();
                            }
                        }
                        else if (paginator.SortBy == SortValue.USERCOUNT)
                        {
                            if (!paginator.OrderByDescending)
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Apps.Add((IApp)obj);
                                }

                                result.Apps = result.Apps
                                    .Where(a => a.OwnerId == ownerId)
                                    .OrderBy(a => a.UserCount)
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
                                    .ToList();
                            }
                            else
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Apps.Add((IApp)obj);
                                }

                                result.Apps = result.Apps
                                    .Where(a => a.OwnerId == ownerId)
                                    .OrderByDescending(a => a.UserCount)
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
                                    .ToList();
                            }
                        }
                        else if (paginator.SortBy == SortValue.NAME)
                        {
                            if (!paginator.OrderByDescending)
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Apps.Add((IApp)obj);
                                }

                                result.Apps = result.Apps
                                    .Where(a => a.OwnerId == ownerId)
                                    .OrderBy(a => a.Name)
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
                                    .ToList();
                            }
                            else
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Apps.Add((IApp)obj);
                                }

                                result.Apps = result.Apps
                                    .Where(a => a.OwnerId == ownerId)
                                    .OrderByDescending(a => a.Name)
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
                                    .ToList();
                            }
                        }
                        else if (paginator.SortBy == SortValue.DATECREATED)
                        {
                            if (!paginator.OrderByDescending)
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Apps.Add((IApp)obj);
                                }

                                result.Apps = result.Apps
                                    .Where(a => a.OwnerId == ownerId)
                                    .OrderBy(a => a.DateCreated)
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
                                    .ToList();
                            }
                            else
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Apps.Add((IApp)obj);
                                }

                                result.Apps = result.Apps
                                    .Where(a => a.OwnerId == ownerId)
                                    .OrderByDescending(a => a.DateCreated)
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
                                    .ToList();
                            }
                        }
                        else if (paginator.SortBy == SortValue.DATEUPDATED)
                        {
                            if (!paginator.OrderByDescending)
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Apps.Add((IApp)obj);
                                }

                                result.Apps = result.Apps
                                    .Where(a => a.OwnerId == ownerId)
                                    .OrderBy(a => a.DateUpdated)
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
                                    .ToList();
                            }
                            else
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Apps.Add((IApp)obj);
                                }

                                result.Apps = result.Apps
                                    .Where(a => a.OwnerId == ownerId)
                                    .OrderByDescending(a => a.DateUpdated)
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
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

                    result.Success = response.Success;
                    result.Message = AppsMessages.AppsFoundMessage;

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
                    result.Message = AppsMessages.AppsNotFoundMessage;

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

        public async Task<IAppsResult> GetRegisteredApps(
            int userId,
            IPaginator paginator)
        {
            if (paginator == null) throw new ArgumentNullException(nameof(paginator));

            var result = new AppsResult();

            if (userId == 0)
            {
                result.Success = false;
                result.Message = UsersMessages.UserNotFoundMessage;

                return result;
            }

            try
            {
                var cacheFactoryResponse = await CacheFactory.GetMyRegisteredAppsWithCacheAsync(
                    _appsRepository,
                    _distributedCache,
                    string.Format(string.Format(CacheKeys.GetMyRegisteredCacheKey, userId)),
                    DateTime.Now.AddMinutes(5),
                    userId,
                    result);

                var response = (RepositoryResponse)cacheFactoryResponse.Item1;
                result = (AppsResult)cacheFactoryResponse.Item2;

                if (response.Success)
                {
                    if (StaticDataHelpers.IsPageValid(paginator, response.Objects))
                    {
                        if (paginator.SortBy == SortValue.NULL)
                        {
                            result.Apps = response
                                .Objects
                                .ConvertAll(a => (IApp)a)
                                .ToList();
                        }
                        else if (paginator.SortBy == SortValue.ID)
                        {
                            if (!paginator.OrderByDescending)
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Apps.Add((IApp)obj);
                                }

                                result.Apps = result.Apps
                                    .OrderBy(a => a.Id)
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
                                    .ToList();
                            }
                            else
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Apps.Add((IApp)obj);
                                }

                                result.Apps = result.Apps
                                    .OrderByDescending(a => a.Id)
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
                                    .ToList();
                            }
                        }
                        else if (paginator.SortBy == SortValue.USERCOUNT)
                        {
                            if (!paginator.OrderByDescending)
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Apps.Add((IApp)obj);
                                }

                                result.Apps = result.Apps
                                    .OrderBy(a => a.UserCount)
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
                                    .ToList();
                            }
                            else
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Apps.Add((IApp)obj);
                                }

                                result.Apps = result.Apps
                                    .OrderByDescending(a => a.UserCount)
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
                                    .ToList();
                            }
                        }
                        else if (paginator.SortBy == SortValue.NAME)
                        {
                            if (!paginator.OrderByDescending)
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Apps.Add((IApp)obj);
                                }

                                result.Apps = result.Apps
                                    .OrderBy(a => a.Name)
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
                                    .ToList();
                            }
                            else
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Apps.Add((IApp)obj);
                                }

                                result.Apps = result.Apps
                                    .OrderByDescending(a => a.Name)
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
                                    .ToList();
                            }
                        }
                        else if (paginator.SortBy == SortValue.DATECREATED)
                        {
                            if (!paginator.OrderByDescending)
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Apps.Add((IApp)obj);
                                }

                                result.Apps = result.Apps
                                    .OrderBy(a => a.DateCreated)
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
                                    .ToList();
                            }
                            else
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Apps.Add((IApp)obj);
                                }

                                result.Apps = result.Apps
                                    .OrderByDescending(a => a.DateCreated)
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
                                    .ToList();
                            }
                        }
                        else if (paginator.SortBy == SortValue.DATEUPDATED)
                        {
                            if (!paginator.OrderByDescending)
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Apps.Add((IApp)obj);
                                }

                                result.Apps = result.Apps
                                    .OrderBy(a => a.DateUpdated)
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
                                    .ToList();
                            }
                            else
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Apps.Add((IApp)obj);
                                }

                                result.Apps = result.Apps
                                    .OrderByDescending(a => a.DateUpdated)
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
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

                    result.Success = response.Success;
                    result.Message = AppsMessages.AppsFoundMessage;

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
                    result.Message = AppsMessages.AppsNotFoundMessage;

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

        public async Task<IUsersResult> GetAppUsers(
            int id,
            int requestorId,
            IPaginator paginator,
            bool appUsers = true)
        {
            if (paginator == null) throw new ArgumentNullException(nameof(paginator));

            var result = new UsersResult();

            if (id == 0)
            {
                result.Success = false;
                result.Message = AppsMessages.AppNotFoundMessage;

                return result;
            }

            if (requestorId == 0)
            {
                result.Success = false;
                result.Message = UsersMessages.UserNotFoundMessage;
            }

            try
            {
                var cacheFactoryResponse = await CacheFactory.GetWithCacheAsync<App>(
                    _appsRepository,
                    _distributedCache,
                    string.Format(CacheKeys.GetAppCacheKey, id),
                    DateTime.Now.AddMinutes(5),
                    id);

                var app = (App)((RepositoryResponse)cacheFactoryResponse.Item1).Object;

                if (app != null)
                {
                    IRepositoryResponse response;

                    if (appUsers)
                    {
                        cacheFactoryResponse = await CacheFactory.GetAppUsersWithCacheAsync(
                            _appsRepository,
                            _distributedCache,
                            string.Format(string.Format(CacheKeys.GetAppUsersCacheKey, id)),
                            DateTime.Now.AddMinutes(5),
                            id,
                            result);

                        response = (RepositoryResponse)cacheFactoryResponse.Item1;
                        result = (UsersResult)cacheFactoryResponse.Item2;
                    }
                    else
                    {
                        cacheFactoryResponse = await CacheFactory.GetNonAppUsersWithCacheAsync(
                            _appsRepository,
                            _distributedCache,
                            string.Format(string.Format(CacheKeys.GetNonAppUsersCacheKey, id)),
                            DateTime.Now.AddMinutes(5),
                            id,
                            result);

                        response = (RepositoryResponse)cacheFactoryResponse.Item1;
                        result = (UsersResult)cacheFactoryResponse.Item2;
                    }

                    if (response.Success)
                    {
                        if (paginator.SortBy == SortValue.NULL)
                        {
                            result.Users = response.Objects.ConvertAll(u => (IUser)u);
                        }
                        else if (paginator.SortBy == SortValue.ID)
                        {
                            if (!paginator.OrderByDescending)
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Users.Add((IUser)obj);
                                }

                                result.Users = result.Users
                                    .OrderBy(u => u.Id)
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
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
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
                                    .ToList();
                            }
                        }
                        else if (paginator.SortBy == SortValue.USERNAME)
                        {
                            if (!paginator.OrderByDescending)
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Users.Add((IUser)obj);
                                }

                                result.Users = result.Users
                                    .OrderBy(u => u.UserName)
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
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
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
                                    .ToList();
                            }
                        }
                        else if (paginator.SortBy == SortValue.FIRSTNAME)
                        {
                            if (!paginator.OrderByDescending)
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Users.Add((IUser)obj);
                                }

                                result.Users = result.Users
                                    .OrderBy(u => u.FirstName)
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
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
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
                                    .ToList();
                            }
                        }
                        else if (paginator.SortBy == SortValue.LASTNAME)
                        {
                            if (!paginator.OrderByDescending)
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Users.Add((IUser)obj);
                                }

                                result.Users = result.Users
                                    .OrderBy(u => u.LastName)
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
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
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
                                    .ToList();
                            }
                        }
                        else if (paginator.SortBy == SortValue.FULLNAME)
                        {
                            if (!paginator.OrderByDescending)
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Users.Add((IUser)obj);
                                }

                                result.Users = result.Users
                                    .OrderBy(u => u.FullName)
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
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
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
                                    .ToList();
                            }
                        }
                        else if (paginator.SortBy == SortValue.NICKNAME)
                        {
                            if (!paginator.OrderByDescending)
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Users.Add((IUser)obj);
                                }

                                result.Users = result.Users
                                    .OrderBy(u => u.NickName)
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
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
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
                                    .ToList();
                            }
                        }
                        else if (paginator.SortBy == SortValue.GAMECOUNT)
                        {
                            if (!paginator.OrderByDescending)
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Users.Add((IUser)obj);
                                }

                                result.Users = result.Users
                                    .OrderBy(u => u.Games.Count)
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
                                    .ToList();
                            }
                            else
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Users.Add((IUser)obj);
                                }

                                result.Users = result.Users
                                    .OrderByDescending(u => u.Games.Count)
                                    .Skip((paginator.Page - 1) * paginator.ItemsPerPage)
                                    .Take(paginator.ItemsPerPage)
                                    .ToList();
                            }
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = ServicesMesages.SortValueNotImplementedMessage;

                            return result;
                        }

                        var requestor = (User)(await _usersRepository.Get(requestorId)).Object;

                        if (requestor != null && !requestor.IsSuperUser)
                        {
                            // Filter out user emails from the frontend...
                            foreach (var user in result.Users)
                            {
                                var emailConfirmed = user.EmailConfirmed;
                                user.Email = null;
                                user.EmailConfirmed = emailConfirmed;
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

        public async Task<ILicenseResult> GetLicense(int id)
        {
            var result = new LicenseResult();

            if (id == 0)
            {
                result.Success = false;
                result.Message = AppsMessages.AppNotFoundMessage;

                return result;
            }

            try
            {
                if (await CacheFactory.HasEntityWithCacheAsync<App>(
                    _appsRepository,
                    _distributedCache,
                    string.Format(CacheKeys.HasAppCacheKey, id),
                    DateTime.Now.AddHours(1),
                    id))
                {
                    var response = await CacheFactory.GetLicenseWithCacheAsync(
                        _appsRepository,
                        _distributedCache,
                        string.Format(CacheKeys.GetAppLicenseCacheKey, id),
                        DateTime.Now.AddHours(1),
                        id,
                        result);

                    result.Success = true;
                    result.FromCache = response.Item2.FromCache;
                    result.Message = AppsMessages.AppFoundMessage;
                    result.License = response.Item1;

                    return result;
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

        public async Task<IBaseResult> AddAppUser(int appId, int userId)
        {
            var result = new BaseResult();

            if (appId == 0)
            {
                result.Success = false;
                result.Message = AppsMessages.AppNotFoundMessage;

                return result;
            }

            if (userId == 0)
            {
                result.Success = false;
                result.Message = UsersMessages.UserNotFoundMessage;

                return result;
            }

            try
            {
                var cacheFactoryResponse = await CacheFactory.GetWithCacheAsync<App>(
                    _appsRepository,
                    _distributedCache,
                    string.Format(CacheKeys.GetAppCacheKey, appId),
                    DateTime.Now.AddMinutes(5),
                    appId);

                var appResponse = (RepositoryResponse)cacheFactoryResponse.Item1;

                if (appResponse.Success)
                {
                    cacheFactoryResponse = await CacheFactory.GetWithCacheAsync<User>(
                        _usersRepository,
                        _distributedCache,
                        string.Format(CacheKeys.GetAppCacheKey, appId),
                        DateTime.Now.AddMinutes(5),
                        userId);

                    var userResponse = (RepositoryResponse)cacheFactoryResponse.Item1;

                    if (userResponse.Success)
                    {
                        var addUserToAppResponse = await _appsRepository.AddAppUser(
                            userId,
                            ((App)appResponse.Object).License);

                        if (addUserToAppResponse.Success)
                        {
                            result.Success = addUserToAppResponse.Success;
                            result.Message = AppsMessages.UserAddedToAppMessage;

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
                            result.Message = AppsMessages.UserNotAddedToAppMessage;

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
                else
                {
                    result.Success = false;
                    result.Message = AppsMessages.AppNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IBaseResult> RemoveAppUser(int appId, int userId)
        {
            var result = new BaseResult();

            if (appId == 0)
            {
                result.Success = false;
                result.Message = AppsMessages.AppNotFoundMessage;

                return result;
            }

            if (userId == 0)
            {
                result.Success = false;
                result.Message = UsersMessages.UserNotFoundMessage;

                return result;
            }

            try
            {
                var cacheFactoryResponse = await CacheFactory.GetWithCacheAsync<App>(
                    _appsRepository,
                    _distributedCache,
                    string.Format(CacheKeys.GetAppCacheKey, appId),
                    DateTime.Now.AddMinutes(5),
                    appId);

                var appResponse = (RepositoryResponse)cacheFactoryResponse.Item1;

                if (appResponse.Success)
                {
                    if (await CacheFactory.HasEntityWithCacheAsync<User>(
                        _usersRepository,
                        _distributedCache,
                        string.Format(CacheKeys.HasUserCacheKey, userId),
                        DateTime.Now.AddHours(1),
                        userId))
                    {
                        if (((App)appResponse.Object).OwnerId == userId)
                        {
                            result.Success = false;
                            result.Message = AppsMessages.UserIsTheAppOwnerMessage;

                            return result;
                        }

                        var addUserToAppResponse = await _appsRepository.RemoveAppUser(
                            userId,
                            ((App)appResponse.Object).License);

                        if (addUserToAppResponse.Success)
                        {
                            result.Success = addUserToAppResponse.Success;
                            result.Message = AppsMessages.UserRemovedFromAppMessage;

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
                            result.Message = AppsMessages.UserNotRemovedFromAppMessage;

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
                else
                {
                    result.Success = false;
                    result.Message = AppsMessages.AppNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IBaseResult> Activate(int id)
        {
            var result = new BaseResult();

            if (id == 0)
            {
                result.Success = false;
                result.Message = AppsMessages.AppNotFoundMessage;

                return result;
            }

            try
            {
                var activateAppResponse = await _appsRepository.Activate(id);

                if (activateAppResponse.Success)
                {
                    result.Success = activateAppResponse.Success;
                    result.Message = AppsMessages.AppActivatedMessage;

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
                    result.Message = AppsMessages.AppNotActivatedMessage;

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

        public async Task<IBaseResult> Deactivate(int id)
        {
            var result = new BaseResult();

            if (id == 0)
            {
                result.Success = false;
                result.Message = AppsMessages.AppNotFoundMessage;

                return result;
            }

            try
            {
                var activateAppResponse = await _appsRepository.Deactivate(id);

                if (activateAppResponse.Success)
                {
                    result.Success = activateAppResponse.Success;
                    result.Message = AppsMessages.AppDeactivatedMessage;

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
                    result.Message = AppsMessages.AppNotDeactivatedMessage;

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

        public async Task<IUserResult> ActivateAdminPrivileges(int appId, int userId)
        {
            var result = new UserResult();

            if (appId == 0)
            {
                result.Success = false;
                result.Message = AppsMessages.AppNotFoundMessage;

                return result;
            }

            if (userId == 0)
            {
                result.Success = false;
                result.Message = UsersMessages.UserNotFoundMessage;

                return result;
            }

            try
            {
                var cacheFactoryResponse = await CacheFactory.GetWithCacheAsync<App>(
                    _appsRepository,
                    _distributedCache,
                    string.Format(CacheKeys.GetAppCacheKey, appId),
                    DateTime.Now.AddMinutes(5),
                    appId);

                var appResponse = (RepositoryResponse)cacheFactoryResponse.Item1;

                if (appResponse.Success)
                {
                    cacheFactoryResponse = await CacheFactory.GetWithCacheAsync<User>(
                        _usersRepository,
                        _distributedCache,
                        string.Format(CacheKeys.GetAppCacheKey, userId),
                        DateTime.Now.AddMinutes(5),
                        userId);

                    var userReponse = (RepositoryResponse)cacheFactoryResponse.Item1;

                    if (userReponse.Success)
                    {
                        var app = (App)appResponse.Object;
                        var user = (User)userReponse.Object;

                        if (user.IsSuperUser)
                        {
                            result.Success = false;
                            result.Message = UsersMessages.SuperUserCannotBePromotedMessage;

                            return result;
                        }

                        if (!await _appsRepository.IsUserRegisteredToApp(
                            app.Id, 
                            app.License, 
                            user.Id))
                        {
                            _ = await _appsRepository.AddAppUser(user.Id, app.License);
                        }
                        else
                        {
                            if (await _appAdminsRepository.HasAdminRecord(app.Id, user.Id))
                            {
                                var adminRecord = (AppAdmin)(await _appAdminsRepository
                                    .GetAdminRecord(app.Id, user.Id)).Object;

                                if (adminRecord.IsActive)
                                {
                                    result.Success = false;
                                    result.Message = UsersMessages.UserIsAlreadyAnAdminMessage;

                                    return result;
                                }
                                else
                                {
                                    adminRecord.IsActive = true;

                                    var adminRecordUpdateResult = await _appAdminsRepository
                                        .Update(adminRecord);

                                    result.Success = adminRecordUpdateResult.Success;
                                    result.Message = UsersMessages.UserHasBeenPromotedToAdminMessage;

                                    return result;
                                }
                            }
                        }


                        if (!user.IsAdmin)
                        {
                            var adminRole = (await _rolesRepository.GetAll())
                                .Objects
                                .ConvertAll(r => (Role)r)
                                .FirstOrDefault(r => r.RoleLevel == RoleLevel.ADMIN);

                            user.Roles.Add(new UserRole {
                                UserId = user.Id,
                                User = user,
                                RoleId = adminRole.Id,
                                Role = adminRole}) ;

                            user = (User)(await _usersRepository.Update(user)).Object;
                        }

                        var appAdmin = new AppAdmin(app.Id, user.Id);

                        var appAdminResult = await _appAdminsRepository.Add(appAdmin);

                        if (appAdminResult.Success)
                        {
                            result.User = (User)
                                (await _usersRepository.Get(userId))
                                .Object;
                            result.Success = appAdminResult.Success;
                            result.Message = UsersMessages.UserHasBeenPromotedToAdminMessage;

                            return result;
                        }
                        else if (!appAdminResult.Success && appAdminResult.Exception != null)
                        {
                            result.Success = appAdminResult.Success;
                            result.Message = appAdminResult.Exception.Message;

                            return result;
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = UsersMessages.UserHasNotBeenPromotedToAdminMessage;

                            return result;
                        }
                    }
                    else if (!userReponse.Success && userReponse.Exception != null)
                    {
                        result.Success = userReponse.Success;
                        result.Message = userReponse.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.Success = userReponse.Success;
                        result.Message = UsersMessages.UserNotFoundMessage;

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
                    result.Success = appResponse.Success;
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

        public async Task<IUserResult> DeactivateAdminPrivileges(int appId, int userId)
        {
            var result = new UserResult();

            if (appId == 0)
            {
                result.Success = false;
                result.Message = AppsMessages.AppNotFoundMessage;

                return result;
            }

            if (userId == 0)
            {
                result.Success = false;
                result.Message = UsersMessages.UserNotFoundMessage;

                return result;
            }

            try
            {
                var cacheFactoryResponse = await CacheFactory.GetWithCacheAsync<App>(
                    _appsRepository,
                    _distributedCache,
                    string.Format(CacheKeys.GetAppCacheKey, appId),
                    DateTime.Now.AddMinutes(5),
                    appId);

                var appResponse = (RepositoryResponse)cacheFactoryResponse.Item1;

                if (appResponse.Success)
                {
                    cacheFactoryResponse = await CacheFactory.GetWithCacheAsync<User>(
                        _usersRepository,
                        _distributedCache,
                        string.Format(CacheKeys.GetAppCacheKey, userId),
                        DateTime.Now.AddMinutes(5),
                        userId);

                    var userResponse = (RepositoryResponse)cacheFactoryResponse.Item1;

                    if (userResponse.Success)
                    {
                        var app = (App)appResponse.Object;
                        var user = (User)userResponse.Object;

                        if (!user.IsAdmin)
                        {
                            result.Success = false;
                            result.Message = UsersMessages.UserDoesNotHaveAdminPrivilegesMessage;

                            return result;
                        }

                        if (!await _appAdminsRepository.HasAdminRecord(app.Id, user.Id))
                        {
                            result.Success = false;
                            result.Message = AppsMessages.UserIsNotAnAssignedAdminMessage;

                            return result;
                        }

                        var appAdmin = (AppAdmin)
                            (await _appAdminsRepository.GetAdminRecord(app.Id, user.Id))
                            .Object;

                        appAdmin.IsActive = false;

                        var appAdminResult = await _appAdminsRepository.Update(appAdmin);

                        if (appAdminResult.Success)
                        {
                            result.User = (User)
                                (await _usersRepository.Get(user.Id))
                                .Object;
                            result.Success = appAdminResult.Success;
                            result.Message = AppsMessages.AdminPrivilegesDeactivatedMessage;

                            return result;
                        }
                        else if (!appAdminResult.Success && appAdminResult.Exception != null)
                        {
                            result.Success = appAdminResult.Success;
                            result.Message = appAdminResult.Exception.Message;

                            return result;
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = AppsMessages.DeactivationOfAdminPrivilegesFailedMessage;

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
                        result.Success = userResponse.Success;
                        result.Message = UsersMessages.UserNotFoundMessage;

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
                    result.Success = appResponse.Success;
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

        public async Task<bool> IsRequestValidOnThisLicense(int id, string license, int userId)
        {
            if (string.IsNullOrEmpty(license)) throw new ArgumentNullException(nameof(license));

            if (id == 0 || userId == 0)
            {
                return false;
            }

            var cacheFactoryResponse = await CacheFactory.GetWithCacheAsync<User>(
                _usersRepository,
                _distributedCache,
                string.Format(CacheKeys.GetUserCacheKey, userId),
                DateTime.Now.AddMinutes(5),
                userId);

            var userResponse = (RepositoryResponse)cacheFactoryResponse.Item1;

            cacheFactoryResponse = await CacheFactory.GetWithCacheAsync<App>(
                _appsRepository,
                _distributedCache,
                string.Format(CacheKeys.GetAppCacheKey, id),
                DateTime.Now.AddMinutes(5),
                id);

            var appResponse = (RepositoryResponse)cacheFactoryResponse.Item1;

            var validLicense = await CacheFactory.IsAppLicenseValidWithCacheAsync(
                _appsRepository,
                _distributedCache,
                string.Format(CacheKeys.IsAppLicenseValidCacheKey, license),
                DateTime.Now.AddHours(1),
                license);

            if (userResponse.Success && appResponse.Success && validLicense)
            {
                bool userPermittedAccess;

                if (!((App)appResponse.Object).PermitCollectiveLogins)
                {
                    userPermittedAccess = await _appsRepository
                        .IsUserRegisteredToApp(id, license, userId);
                }
                else
                {
                    userPermittedAccess = true;
                }

                if (userPermittedAccess && validLicense)
                {
                    if (((App)appResponse.Object).IsActive)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (((User)userResponse.Object).IsSuperUser && validLicense)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> IsOwnerOfThisLicense(int id, string license, int userId)
        {
            if (string.IsNullOrEmpty(license)) throw new ArgumentNullException(nameof(license));

            if (id == 0 || userId == 0)
            {
                return false;
            }

            var cacheFactoryResponse = await CacheFactory.GetWithCacheAsync<User>(
                _usersRepository,
                _distributedCache,
                string.Format(CacheKeys.GetUserCacheKey, userId),
                DateTime.Now.AddMinutes(5),
                userId);

            var userResponse = (RepositoryResponse)cacheFactoryResponse.Item1;

            var validLicense = await CacheFactory.IsAppLicenseValidWithCacheAsync(
                _appsRepository,
                _distributedCache,
                string.Format(CacheKeys.IsAppLicenseValidCacheKey, license),
                DateTime.Now.AddHours(1),
                license);

            if (userResponse.Success && validLicense)
            {
                var requestorOwnerOfThisApp = await _appsRepository.IsUserOwnerOfApp(id, license, userId);

                if (requestorOwnerOfThisApp && validLicense)
                {
                    return true;
                }
                else if (((User)userResponse.Object).IsSuperUser && validLicense)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
